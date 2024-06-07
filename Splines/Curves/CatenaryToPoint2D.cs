using System.Numerics;
using Splines.Enums;
using Splines.Extensions;
using Splines.Numerics;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>A catenary curve from the origin to a point P</summary>
public struct CatenaryToPoint2D
{
    const int INTERVAL_SEARCH_ITERATIONS = 12;
    const int BISECT_REFINE_COUNT = 14;

    // data
    Vector2 p;
    float s;

    // cached state
    CatenaryToPointEvaluability _catenaryToPointEvaluability;
    float a;
    Vector2 delta;
    float arcLenSampleOffset;

    public CatenaryToPoint2D(Vector2 p, float s) {
        (this.p, this.s) = (p, s);
        a = default;
        delta = default;
        arcLenSampleOffset = default;
        _catenaryToPointEvaluability = CatenaryToPointEvaluability.Unknown;
    }

    public Vector2 P {
        get => p;
        set {
            if (value != p)
                (p, _catenaryToPointEvaluability) = (value, CatenaryToPointEvaluability.Unknown);
        }
    }

    public float Length
    {
        get => s;
        set
        {
            if (value != s)
                (s, _catenaryToPointEvaluability) = (value, CatenaryToPointEvaluability.Unknown);
        }
    }

    public bool IsVertical => Mathf.Abs(p.X) < 0.001f;
    public bool IsStraightLine => s <= p.Magnitude() * 1.00005f;

    /// <summary>Evaluates a position on this catenary curve at the given arc length of <c>sEval</c></summary>
    /// <param name="sEval">The arc length along the curve to sample, relative to the first point</param>
    /// <param name="nthDerivative">The derivative to sample. 1 = first derivative, 2 = second derivative</param>
    public Vector2 Eval(float sEval, int nthDerivative = 0) {
        ReadyForEvaluation();
        return nthDerivative switch {
            0 => _catenaryToPointEvaluability switch {
                CatenaryToPointEvaluability.Catenary       => EvalCatPosByArcLength(sEval),
                CatenaryToPointEvaluability.LineSegment    => EvalStraightLineByArcLength(sEval),
                CatenaryToPointEvaluability.LinearVertical => EvalVerticalLinearApproxByArcLength(sEval),
                CatenaryToPointEvaluability.Unknown or _   => throw new Exception("Failed to evaluate catenary, couldn't calculate evaluability")
            },
            _ => _catenaryToPointEvaluability switch {
                CatenaryToPointEvaluability.Catenary       => EvalCatDerivByArcLength(sEval, nthDerivative),
                CatenaryToPointEvaluability.LineSegment    => nthDerivative == 1 ? p.Normalized() : Vector2.Zero,
                CatenaryToPointEvaluability.LinearVertical => new Vector2(0, nthDerivative == 1 ? (sEval < -(p.Y - s) / 2 ? -1 : 1) : 0),
                CatenaryToPointEvaluability.Unknown or _   => throw new Exception("Failed to evaluate catenary, couldn't calculate evaluability")
            }
        };
    }

    // straight line from p0 to p1
    Vector2 EvalStraightLineByArcLength(float sEval) => p * (sEval / s);

    // almost completely vertical line when p0.x is approx. equal to p1.x
    Vector2 EvalVerticalLinearApproxByArcLength(float sEval) {
        float x = Mathfs.Lerp(0, p.X, sEval / s); // just to make it not snap to x=0
        float b = (p.Y - s) / 2; // bottom
        float seg0 = -b;
        float y = (sEval < seg0) ? -sEval : -2 * seg0 + sEval;
        return new Vector2(x, y);
    }

    // evaluates the position of the catenary at the given arc length, relative to the first point
    Vector2 EvalCatPosByArcLength(float sEval) {
        sEval *= p.X.Sign(); // since we go backwards when p0.x < p1.x
        float x = Catenary1D.EvalXByArcLength(sEval + arcLenSampleOffset, a) + delta.X;
        float y = EvalPassingThrough0(x);
        return new Vector2(x, y);
    }

    /// <summary>Evaluates the n-th derivative of the catenary at the given arc length</summary>
    /// <param name="sEval">The arc length, relative to the first point</param>
    /// <param name="n">The derivative to evaluate</param>
    Vector2 EvalCatDerivByArcLength(float sEval, int n = 1) {
        if (n == 0)
            return EvalCatPosByArcLength(sEval);
        sEval *= p.X.Sign(); // since we go backwards when p0.x < p1.x
        return Catenary1D.EvalDerivateByArcLength(sEval + arcLenSampleOffset, a, n);
    }

    // Evaluate passing through the origin and p
    float EvalPassingThrough0(float x) => Catenary1D.Eval(x - delta.X, a) + delta.Y;

    // calculates p, a, delta, arcLenSampleOffset, and which evaluation method to use
    void ReadyForEvaluation() {
        if (_catenaryToPointEvaluability != CatenaryToPointEvaluability.Unknown)
            return;

        // CASE 1:
        // first, test if it's a line segment
        if (IsStraightLine) {
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.LineSegment;
            return;
        }

        // CASE 2:
        // check if it's basically a fully vertical hanging chain
        if (IsVertical) {
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.LinearVertical;
            return;
        }

        // CASE 3:
        // Now we've got a catenary on our hands unless something explodes.
        float c = Mathf.Sqrt(s * s - p.Y * p.Y);
        float pAbsX = p.X.Abs(); // solve only in x > 0

        // find bounds of the root
        float xRoot = (p.X * p.X) / (2 * s); // intial guess based on freya's flawless heuristics
        if (TryFindRootBounds(pAbsX, c, xRoot, out FloatRange xRange)) {
            // refine range, if necessary (which is very likely)
            if (Mathfs.Approximately(xRange.Length, 0) == false)
                RootFindBisections(pAbsX, c, ref xRange, BISECT_REFINE_COUNT); // Catenary seems valid, with roots inside, refine the range
            a = xRange.Center; // set a to the middle of the latest range
            delta = CalcCatenaryDelta(a, p); // find delta to pass through both points
            arcLenSampleOffset = CalcArcLenSampleOffset(delta.X, a);
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.Catenary;
        } else {
            // CASE 4:
            // something exploded, couldn't find a range, so let's use a straight line as a fallback
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.LineSegment;
        }
    }

    // root solve function
    static float R(float a, float pAbsX, float c) => 2 * a * Mathfs.Sinh(pAbsX / (2 * a)) - c;

    // Calculates the arc length offset so that it's relative to the start of the chain when evaluating by arc length
    static float CalcArcLenSampleOffset(float deltaX, float a) => Catenary1D.EvalArcLength(-deltaX, a);

    // Calculates the required offset to make a catenary pass through the origin and a point p
    static Vector2 CalcCatenaryDelta(float a, Vector2 p) {
        Vector2 d;
        d.X = p.X / 2 - a * Mathfs.Asinh(p.Y / (2 * a * Mathfs.Sinh(p.X / (2 * a))));
        d.Y = -Catenary1D.Eval(d.X, a); // technically -d.x but because of symmetry d.x works too
        return d;
    }

    // presumes a decreasing function with one root in x > 0
    // g = initial guess
    static bool TryFindRootBounds(float pAbsX, float c, float g, out FloatRange xRange) {
        float y = R(g, pAbsX, c);
        xRange = new FloatRange(g, g);
        if (Mathfs.Approximately(y, 0)) // somehow landed *on* our root in our initial guess
            return true;

        bool findingUpper = y > 0;

        for (int n = 1; n <= INTERVAL_SEARCH_ITERATIONS; n++) {
            if (findingUpper) {
                // It's positive - we found our lower bound
                // exponentially search for upper bound
                xRange.Start = xRange.End;
                xRange.End = g * Mathf.Pow(2, n);
                y = R(xRange.End, pAbsX, c);
                if (y < 0)
                    return true; // upper bound found!
            } else {
                // It's negative - we found our upper bound
                // exponentially search for lower bound
                xRange.End = xRange.Start;
                xRange.Start = g * Mathf.Pow(2, -n);
                y = R(xRange.Start, pAbsX, c);
                if (y > 0)
                    return true; // lower bound found!
            }
        }

        return false; // no root found
    }

    static void RootFindBisections(float pAbsX, float c, ref FloatRange xRange, int iterationCount)
    {
        for (int i = 0; i < iterationCount; i++)
            RootFindBisection(pAbsX, c, ref xRange);
    }

    static void RootFindBisection(float pAbsX, float c, ref FloatRange xRange)
    {
        float xInter = xRange.Center; // bisection
        float yInter = R(xInter, pAbsX, c);
        if (yInter > 0)
            xRange.Start = xInter; // adjust left bound
        else
            xRange.End = xInter; // adjust right bound
    }
}
