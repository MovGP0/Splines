using System.Numerics;
using Splines.Enums;
using Splines.Extensions;
using Splines.Numerics;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>A catenary curve from the origin to a point P in 3D space</summary>
public struct CatenaryToPoint3D
{
    const int INTERVAL_SEARCH_ITERATIONS = 12;
    const int BISECT_REFINE_COUNT = 14;

    // data
    Vector3 p;
    float s;

    // cached state
    CatenaryToPointEvaluability _catenaryToPointEvaluability;
    float a;
    Vector3 delta;
    float arcLenSampleOffset;

    public CatenaryToPoint3D(Vector3 p, float s)
    {
        (this.p, this.s) = (p, s);
        a = default;
        delta = default;
        arcLenSampleOffset = default;
        _catenaryToPointEvaluability = CatenaryToPointEvaluability.Unknown;
    }

    public Vector3 P
    {
        get => p;
        set
        {
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

    public bool IsVertical => Math.Abs(p.X) < 0.001f && Math.Abs(p.Z) < 0.001f;
    public bool IsStraightLine => s <= p.Length() * 1.00005f;

    /// <summary>Evaluates a position on this catenary curve at the given arc length of <c>sEval</c></summary>
    /// <param name="sEval">The arc length along the curve to sample, relative to the first point</param>
    /// <param name="nthDerivative">The derivative to sample. 1 = first derivative, 2 = second derivative</param>
    public Vector3 Eval(float sEval, int nthDerivative = 0)
    {
        ReadyForEvaluation();
        return nthDerivative switch
        {
            0 => _catenaryToPointEvaluability switch
            {
                CatenaryToPointEvaluability.Catenary => EvalCatPosByArcLength(sEval),
                CatenaryToPointEvaluability.LineSegment => EvalStraightLineByArcLength(sEval),
                CatenaryToPointEvaluability.LinearVertical => EvalVerticalLinearApproxByArcLength(sEval),
                CatenaryToPointEvaluability.Unknown or _ => throw new Exception("Failed to evaluate catenary, couldn't calculate evaluability")
            },
            _ => _catenaryToPointEvaluability switch
            {
                CatenaryToPointEvaluability.Catenary => EvalCatDerivByArcLength(sEval, nthDerivative),
                CatenaryToPointEvaluability.LineSegment => nthDerivative == 1 ? p.Normalized() : Vector3.Zero,
                CatenaryToPointEvaluability.LinearVertical => new Vector3(0, nthDerivative == 1 ? (sEval < -(p.Y - s) / 2 ? -1 : 1) : 0, 0),
                CatenaryToPointEvaluability.Unknown or _ => throw new Exception("Failed to evaluate catenary, couldn't calculate evaluability")
            }
        };
    }

    // straight line from p0 to p1
    Vector3 EvalStraightLineByArcLength(float sEval) => p * (sEval / s);

    // almost completely vertical line when p0.x is approx. equal to p1.x and p0.z is approx. equal to p1.z
    Vector3 EvalVerticalLinearApproxByArcLength(float sEval)
    {
        float x = Mathf.Lerp(0, p.X, sEval / s); // just to make it not snap to x=0
        float z = Mathf.Lerp(0, p.Z, sEval / s); // just to make it not snap to z=0
        float b = (p.Y - s) / 2; // bottom
        float seg0 = -b;
        float y = (sEval < seg0) ? -sEval : -2 * seg0 + sEval;
        return new Vector3(x, y, z);
    }

    // evaluates the position of the catenary at the given arc length, relative to the first point
    Vector3 EvalCatPosByArcLength(float sEval)
    {
        sEval *= Mathf.Sign(p.X); // since we go backwards when p0.x < p1.x
        float x = Catenary1D.EvalXByArcLength(sEval + arcLenSampleOffset, a) + delta.X;
        float y = EvalPassingThrough0(x);
        return new Vector3(x, y, 0); // Assuming catenary is in X-Y plane
    }

    /// <summary>Evaluates the n-th derivative of the catenary at the given arc length</summary>
    /// <param name="sEval">The arc length, relative to the first point</param>
    /// <param name="n">The derivative to evaluate</param>
    Vector3 EvalCatDerivByArcLength(float sEval, int n = 1)
    {
        if (n == 0)
            return EvalCatPosByArcLength(sEval);
        sEval *= Mathf.Sign(p.X); // since we go backwards when p0.x < p1.x
        var deriv = Catenary1D.EvalDerivateByArcLength(sEval + arcLenSampleOffset, a, n);
        return new Vector3(deriv.X, deriv.Y, 0); // Assuming catenary is in X-Y plane
    }

    // Evaluate passing through the origin and p
    float EvalPassingThrough0(float x) => Catenary1D.Eval(x - delta.X, a) + delta.Y;

    // calculates p, a, delta, arcLenSampleOffset, and which evaluation method to use
    void ReadyForEvaluation()
    {
        if (_catenaryToPointEvaluability != CatenaryToPointEvaluability.Unknown)
            return;

        // CASE 1:
        // first, test if it's a line segment
        if (IsStraightLine)
        {
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.LineSegment;
            return;
        }

        // CASE 2:
        // check if it's basically a fully vertical hanging chain
        if (IsVertical)
        {
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.LinearVertical;
            return;
        }

        // CASE 3:
        // Now we've got a catenary on our hands unless something explodes.
        float c = Mathf.Sqrt(s * s - p.Y * p.Y);
        float pAbsX = Mathf.Abs(p.X); // solve only in x > 0

        // find bounds of the root
        float xRoot = (p.X * p.X) / (2 * s); // initial guess based on freya's flawless heuristics
        if (TryFindRootBounds(pAbsX, c, xRoot, out FloatRange xRange))
        {
            // refine range, if necessary (which is very likely)
            if (!(Mathf.Abs(xRange.Length) < 0))
            {
                RootFindBisections(pAbsX, c, ref xRange, BISECT_REFINE_COUNT); // Catenary seems valid, with roots inside, refine the range
            }

            a = xRange.Center; // set a to the middle of the latest range
            delta = CalcCatenaryDelta(a, p); // find delta to pass through both points
            arcLenSampleOffset = CalcArcLenSampleOffset(delta.X, a);
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.Catenary;
        }
        else
        {
            // CASE 4:
            // something exploded, couldn't find a range, so let's use a straight line as a fallback
            _catenaryToPointEvaluability = CatenaryToPointEvaluability.LineSegment;
        }
    }

    // root solve function
    static float R(float a, float pAbsX, float c) => 2 * a * Mathf.Sinh(pAbsX / (2 * a)) - c;

    // Calculates the arc length offset so that it's relative to the start of the chain when evaluating by arc length
    static float CalcArcLenSampleOffset(float deltaX, float a) => Catenary1D.EvalArcLength(-deltaX, a);

    // Calculates the required offset to make a catenary pass through the origin and a point p
    static Vector3 CalcCatenaryDelta(float a, Vector3 p)
    {
        Vector3 d;
        d.X = p.X / 2 - a * Mathf.Asinh(p.Y / (2 * a * Mathf.Sinh(p.X / (2 * a))));
        d.Y = -Catenary1D.Eval(d.X, a); // technically -d.x but because of symmetry d.x works too
        d.Z = p.Z / 2; // Assuming catenary is in X-Y plane
        return d;
    }

    // presumes a decreasing function with one root in x > 0
    // g = initial guess
    static bool TryFindRootBounds(float pAbsX, float c, float g, out FloatRange xRange)
    {
        float y = R(g, pAbsX, c);
        xRange = new FloatRange(g, g);
        if (Mathf.Abs(y) < 0.0001) // somehow landed *on* our root in our initial guess
            return true;

        bool findingUpper = y > 0;

        for (int n = 1; n <= INTERVAL_SEARCH_ITERATIONS; n++)
        {
            if (findingUpper)
            {
                // It's positive - we found our lower bound
                // exponentially search for upper bound
                xRange.Start = xRange.End;
                xRange.End = g * Mathf.Pow(2, n);
                y = R(xRange.End, pAbsX, c);
                if (y < 0)
                    return true; // upper bound found!
            }
            else
            {
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
