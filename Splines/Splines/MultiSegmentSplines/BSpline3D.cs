using System.Numerics;
using Splines.Extensions;

namespace Splines.Splines.MultiSegmentSplines;

public sealed class BSpline3D
{
    public Vector3[] Points { get; }
    public float[] Knots { get; }
    public int Degree { get; }

    /// <summary>The Order of this curve (degree+1)</summary>
    public int Order => Degree + 1;

    /// <summary>The number of control points in the B-spline hull</summary>
    public int PointCount => Points.Length;

    /// <summary>Creates a B-spline of the given degree, from a set of points and a knot vector</summary>
    /// <param name="points">The B-spline control points</param>
    /// <param name="knots">The knot vector defining the parameter space of this B-spline. Note: the number of knots has to be exactly degree+pointCount+1</param>
    /// <param name="degree">The degree of the spline</param>
    public BSpline3D(Vector3[] points, float[] knots, int degree = 3)
    {
        Points = points;
        Knots = knots;
        Degree = degree;
        _evalBuffer = new Vector3[degree + 1];
        int expectedKnotCount = SplineUtils.BSplineKnotCount(Points.Length, Degree);
        if (knots.Length != expectedKnotCount)
            throw new ArgumentException($"The knots array has to be of length (degree+pointCount+1). Got an array of {knots.Length} knots, expected {expectedKnotCount}", nameof(knots));
    }

    /// <summary>Creates a uniform B-spline of the given degree, automatically configuring the knot vector to be uniform</summary>
    /// <param name="points">The B-spline control points</param>
    /// <param name="degree">The degree of the curve</param>
    /// <param name="open">Whether it should be open. Open means the curve passes through its endpoints</param>
    public BSpline3D(Vector3[] points, int degree = 3, bool open = false)
    {
        Points = points;
        Degree = degree.AtLeast(1);
        _evalBuffer = new Vector3[degree + 1];
        Knots = SplineUtils.GenerateUniformKnots(Degree, Points.Length, open);
    }

    /// <summary>The number of knots in the B-spline parameter space</summary>
    public int KnotCount => Knots.Length;

    /// <summary>The number of curve segments. Note: some of these curve segments may have a length of 0 depending on knot multiplicity</summary>
    public int SegmentCount => InternalKnotCount - 1;

    /// <summary>The first knot index of the internal parameter space</summary>
    public int InternalKnotIndexStart => Degree;

    /// <summary>The last knot index of the internal parameter space</summary>
    public int InternalKnotIndexEnd => KnotCount - Degree - 1;

    /// <summary>The parameter space knot value at the start of the internal parameter space</summary>
    public float InternalKnotValueStart => Knots[InternalKnotIndexStart];

    /// <summary>The parameter space knot value at the end of the internal parameter space</summary>
    public float InternalKnotValueEnd => Knots[InternalKnotIndexEnd];

    /// <summary>The number of knots in the internal parameter space</summary>
    public int InternalKnotCount => KnotCount - Degree * 2;

    /// <summary>Returns the parameter space knot u-value, given a t-value along the whole spline</summary>
    /// <param name="t">A value from 0-1, representing a percentage along the whole spline</param>
    public float GetKnotValueAt(float t) => Mathfs.Lerp(Knots[Degree], Knots[Knots.Length - Degree - 1], t);

    /// <summary>Returns whether this B-spline is open, which means it will pass through its endpoints.
    /// A B-spline is open if the first degree+1 knots are equal, and the last degree+1 knots are equal</summary>
    public bool Open
    {
        get
        {
            int kc = KnotCount;
            for (int i = 0; i < Degree; i++)
            {
                if (Knots[i] != Knots[i + 1])
                    return false;

                if (Knots[kc - 1 - i] != Knots[kc - i - 2])
                    return false;
            }

            return true;
        }
    }

    /// <summary>Returns the derivative of this B-spline, which is a B-spline in and of itself</summary>
    public BSpline3D Differentiate()
    {
        float[] dKnots = new float[KnotCount];
        Knots.CopyTo(dKnots, 0);

        // one point less
        Vector3[] dPts = new Vector3[PointCount - 1];
        for (int i = 0; i < dPts.Length; i++) {
            Vector3 num = Points[i + 1] - Points[i];
            float den = Knots[i + Degree + 1] - Knots[i + 1];
            float scale = den == 0 ? 0 : Degree / den;
            dPts[i] = num * scale;
        }

        return new BSpline3D(dPts, dKnots, Degree - 1);
    }

    /// <summary>Returns the point at the given t-value of a specific B-spline segment, by index</summary>
    /// <param name="segment">The segment to get a point from</param>
    /// <param name="t">The t-value along the segment to evaluate</param>
    public Vector3 GetSegmentPoint(int segment, float t)
    {
        if (segment < 0 || segment >= SegmentCount)
        {
            throw new IndexOutOfRangeException($"B-Spline segment index {segment} is out of range. Valid indices: 0 to {SegmentCount - 1}");
        }

        float knotMin = Knots[Degree + segment];
        float knotMax = Knots[Degree + segment + 1];
        float u = Mathfs.Lerp(knotMin, knotMax, t);
        return Eval(Degree + segment, u);
    }

    /// <summary>Returns the point at the given t-value in the spline</summary>
    /// <param name="t">A value from 0-1, representing a percentage along the whole spline</param>
    public Vector3 GetPoint(float t)
    {
        float u = GetKnotValueAt(t); // remap 0-1 to knot space
        return GetPointByKnotValue(u);
    }

    /// <summary>Returns the point at the given knot by index</summary>
    /// <param name="i">The index of the knot to get the position of</param>
    public Vector3 GetPointByKnotIndex(int i)
    {
        int kRef = i.Clamp(InternalKnotIndexStart, InternalKnotIndexEnd - 1);
        return Eval(kRef, Knots[i]);
    }

    /// <summary>Returns the point at the given parameter space u-value of the spline</summary>
    /// <param name="u">A value in parameter space. Note: this value has to be within the internal knot interval</param>
    public Vector3 GetPointByKnotValue(float u)
    {
        int i = 0;
        if (u >= Knots[InternalKnotIndexEnd])
        {
            i = InternalKnotIndexEnd - 1; // to handle the t = 1 special case
        }
        else
        {
            for (int j = 0; j < Knots.Length; j++)
            { // find relevant interval
                if (Knots[j] <= u && u < Knots[j + 1])
                {
                    i = j;
                    break;
                }
            }
        }

        return Eval(i, u);
    }

    [NonSerialized]
    private Vector3[] _evalBuffer;

    /// <summary>Returns the point at the given De-Boor recursion depth, knot interval and parameter space u-value</summary>
    /// <param name="k">The index of the knot interval our u-value is inside</param>
    /// <param name="u">A value in parameter space. Note: this value has to be within the internal knot interval</param>
    /// <remarks>Based on <a href="https://en.wikipedia.org/wiki/De_Boor%27s_algorithm">De Boor's algorithm</a></remarks>
    public Vector3 Eval(int k, float u)
    {
        // make sure our buffer is ready
        if (_evalBuffer == null || _evalBuffer.Length != Degree + 1)
            _evalBuffer = new Vector3[Degree + 1];

        // populate points in the buffer
        for (int i = 0; i < Degree + 1; i++)
            _evalBuffer[i] = Points[i + k - Degree];

        // calculate each layer until we've got only one point left
        for (int r = 1; r < Degree + 1; r++)
        {
            for (int j = Degree; j > r - 1; j--)
            {
                float alpha = Mathfs.InverseLerpSafe(Knots[j + k - Degree], Knots[j + 1 + k - r], u);
                _evalBuffer[j] = Vector3.Lerp(_evalBuffer[j - 1], _evalBuffer[j], alpha);
            }
        }

        return _evalBuffer[Degree];
    }

    /// <summary>Returns the basis curve of a given point (by index), at the given parameter space u-value</summary>
    /// <param name="point">The point to get the basis curve of</param>
    /// <param name="u">A value in parameter space. Note: this value has to be within the internal knot interval</param>
    public float GetPointWeightAtKnotValue(int point, float u) => EvalBasis(point, Order, u);

    /// <summary>
    /// Cox–de Boor recursion
    /// </summary>
    /// <remarks>
    /// See <a href="https://demonstrations.wolfram.com/GeneratingABSplineCurveByTheCoxDeBoorAlgorithm/">
    /// Wolfram Demonstrations: Generating a B-Spline Curve by the Cox-De Boor Algorithm
    /// </a>
    /// </remarks>
    float EvalBasis(int p, int k, float u)
    {
        // p = the point to get the basis curve for
        // k = depth of recursion, where 0 = base knots. generally you start with k = Order
        // u = knot value
        k--;
        if (k == 0)
        {
            if (p == Knots.Length - 2) // todo: verify this, I just hacked it in, seems sus af
            {
                return Knots[p] <= u && u <= Knots[p + 1] ? 1 : 0;
            }

            return Knots[p] <= u && u < Knots[p + 1] ? 1 : 0;
        }

        return W(p, k, u) * EvalBasis(p, k, u) + (1f - W(p + 1, k, u)) * EvalBasis(p + 1, k, u);
    }

    private float W(int i, int k, float t)
    {
        float den = Knots[i + k] - Knots[i];
        if (den == 0)
        {
            return 0;
        }

        return (t - Knots[i]) / den;
    }

    public Vector3 GetPointKnotAndWeightByLocalSpan(int point, float t)
    {
        float knotStart = Knots[point];
        float knotEnd = Knots[point + Degree + 1];
        float u = Mathfs.Lerp(knotStart, knotEnd, t);
        float v = EvalBasis(point, Order, u);
        return new Vector3(u, v, 0); // Adding 0 for the Z-coordinate
    }
}
