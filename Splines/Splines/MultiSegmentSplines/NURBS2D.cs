using System.Numerics;

namespace Splines.Splines.MultiSegmentSplines;

/// <summary>
/// Represents a Non-Uniform Rational B-Spline (NURBS) curve in 2D space.
/// </summary>
public sealed partial class NURBS2D
{
    /// <summary>
    /// Gets the control points of the NURBS curve.
    /// </summary>
    public Vector2[] Points { get; }

    /// <summary>
    /// Gets the knot vector of the NURBS curve.
    /// </summary>
    public float[] Knots { get; }

    /// <summary>
    /// Gets the weights associated with the control points. Null if the curve is not rational.
    /// </summary>
    public float[]? Weights { get; }

    /// <summary>
    /// Gets the degree of the NURBS curve.
    /// </summary>
    public int Degree { get; }

    /// <summary>
    /// Gets a value indicating whether the NURBS curve is rational.
    /// </summary>
    public bool Rational => Weights != null;

    /// <summary>
    /// Gets the number of control points.
    /// </summary>
    public int PointCount => Points.Length;

    /// <summary>
    /// Gets the order of the NURBS curve, which is degree + 1.
    /// </summary>
    public int Order => Degree + 1;

    /// <summary>
    /// Gets the number of knots in the knot vector.
    /// </summary>
    public int KnotCount => Degree + PointCount + 1;

    /// <summary>
    /// Gets the number of segments in the NURBS curve.
    /// </summary>
    public int SegmentCount => KnotCount - Degree * 2 - 1;

    /// <summary>
    /// Creates a uniform B-Spline curve.
    /// </summary>
    /// <param name="points">The control points of the B-Spline.</param>
    /// <param name="degree">The degree of the B-Spline.</param>
    /// <param name="open">A value indicating whether the B-Spline should be open or closed.</param>
    /// <returns>A new instance of the <see cref="NURBS2D"/> class representing a uniform B-Spline.</returns>
    public static NURBS2D GetUniformBSpline(Vector2[] points, int degree = 3, bool open = true)
    {
        int ptCount = points.Length;
        float[] knots = SplineUtils.GenerateUniformKnots(degree, ptCount, open);
        return new NURBS2D(points, knots, null, degree);
    }

    /// <summary>
    /// Generates an array of unweighted weights (all weights equal to 1).
    /// </summary>
    /// <param name="count">The number of weights to generate.</param>
    /// <returns>An array of unweighted weights.</returns>
    public static float[] GetUnweightedWeights(int count)
    {
        float[] weights = new float[count];
        for (int i = 0; i < count; i++)
        {
            weights[i] = 1;
        }

        return weights;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NURBS2D"/> class.
    /// </summary>
    /// <param name="points">The control points of the NURBS curve.</param>
    /// <param name="knots">The knot vector of the NURBS curve.</param>
    /// <param name="weights">The weights of the control points. Null if the curve is not rational.</param>
    /// <param name="degree">The degree of the NURBS curve.</param>
    /// <exception cref="ArgumentException">Thrown when the length of the weights array does not match the number of points, or the length of the knots array does not match the required length.</exception>
    public NURBS2D(Vector2[] points, float[] knots, float[]? weights, int degree = 3)
    {
        if (weights != null && weights.Length != points.Length)
        {
            throw new ArgumentException($"The weights array has to match the number of points. Got an array of {weights.Length} weights, expected {points.Length}", nameof(weights));
        }

        Points = points;
        Knots = knots;
        Degree = degree;
        Weights = weights;
        Knots = knots;

        if (knots.Length != KnotCount)
        {
            throw new ArgumentException($"The knots array has to be of length (degree + pointCount + 1). Got an array of {knots.Length} knots, expected {KnotCount}", nameof(knots));
        }
    }

    /// <summary>
    /// Gets a point on the NURBS curve corresponding to a given knot value.
    /// </summary>
    /// <param name="t">The knot value.</param>
    /// <returns>The point on the NURBS curve corresponding to the given knot value.</returns>
    public Vector2 GetPointByKnotValue(float t)
    {
        bool weighted = Rational;
        Vector2 sum = default;

        float norm = 0;
        for (int i = 0; i < PointCount; i++)
        {
            float basis = Basis(i, Order, t);
            if (weighted) norm += (basis *= Weights[i]);
            sum += Points[i] * basis;
        }

        if (weighted)
        {
            sum /= norm;
        }

        return sum;
    }

    /// <summary>
    /// Gets a point on the NURBS curve corresponding to a parameter value between 0 and 1.
    /// </summary>
    /// <param name="t">The parameter value, where 0 is the start and 1 is the end of the curve.</param>
    /// <returns>The point on the NURBS curve corresponding to the parameter value.</returns>
    public Vector2 GetPoint(float t)
    {
        t = Mathfs.Lerp(Knots[Degree], Knots[Knots.Length - Degree - 1], t);
        return GetPointByKnotValue(t);
    }

    /// <summary>
    /// Computes the weight function for the given index, degree, and parameter value.
    /// </summary>
    /// <param name="i">The index of the control point.</param>
    /// <param name="k">The degree of the weight function.</param>
    /// <param name="t">The parameter value.</param>
    /// <returns>The weight value.</returns>
    private float W(int i, int k, float t)
    {
        float den = Knots[i + k] - Knots[i];
        if (den == 0)
        {
            return 0;
        }

        return (t - Knots[i]) / den;
    }

    /// <summary>
    /// Computes the basis function for the given index, degree, and parameter value.
    /// </summary>
    /// <param name="i">The index of the control point.</param>
    /// <param name="k">The degree of the basis function.</param>
    /// <param name="t">The parameter value.</param>
    /// <returns>The basis value.</returns>
    public float Basis(int i, int k, float t)
    {
        k--;
        if (k == 0)
        {
            if (i == Knots.Length - 2)
            {
                return Knots[i] <= t && t <= Knots[i + 1] ? 1 : 0;
            }

            return Knots[i] <= t && t < Knots[i + 1] ? 1 : 0;
        }

        return W(i, k, t) * Basis(i, k, t) + (1f - W(i + 1, k, t)) * Basis(i + 1, k, t);
    }

    /// <summary>
    /// Computes the weighted basis function for the given index, degree, and parameter value.
    /// </summary>
    /// <param name="i">The index of the control point.</param>
    /// <param name="k">The degree of the basis function.</param>
    /// <param name="t">The parameter value.</param>
    /// <returns>The weighted basis value.</returns>
    public float WeightedBasis(int i, int k, float t)
    {
        bool weighted = Rational;
        float norm = 0;
        float targetBasis = 0;

        for (int j = 0; j < PointCount; j++)
        {
            float basis = Basis(j, Order, t);
            if (weighted)
            {
                norm += (basis *= Weights[j]);
            }

            if (j == i)
            {
                targetBasis = basis;
            }
        }

        return targetBasis / norm;
    }
}
