using System.Numerics;
using Splines.Curves;

namespace Splines.Splines.MultiSegmentSplines;

/// <summary>
/// Represents a node in a multi-segment spline, containing the position, knot value,
/// and polynomial curve associated with the node.
/// </summary>
public partial struct Node3
{
    /// <summary>
    /// Gets or sets the position of the node.
    /// </summary>
    /// <value>The 3D vector representing the position of the node.</value>
    public Vector3 Position
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the knot value of the node.
    /// </summary>
    /// <value>The knot value as a float.</value>
    /// <remarks>
    /// The knot value is used to parameterize the spline segment associated with this node.
    /// It helps in mapping the global parameter to a local context within the node, ensuring
    /// continuity and smoothness across multiple spline segments.
    /// </remarks>
    public float Knot
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the polynomial curve associated with the node.
    /// </summary>
    /// <value>The 3D polynomial curve.</value>
    public Polynomial3D Curve
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Evaluates the position on the curve at a given parameter <paramref name="u"/>.
    /// </summary>
    /// <param name="u">The parameter value at which to evaluate the curve.</param>
    /// <returns>The position on the curve as a 3D vector.</returns>
    [Pure]
    public Vector3 EvalPoint(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.Eval(u);
    }

    /// <summary>
    /// Evaluates the first derivative of the curve at a given parameter <paramref name="u"/>.
    /// </summary>
    /// <param name="u">The parameter value at which to evaluate the first derivative of the curve.</param>
    /// <returns>The first derivative of the curve as a 3D vector.</returns>
    [Pure]
    public Vector3 EvalDerivative(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.EvalDerivative(u);
    }

    /// <summary>
    /// Evaluates the second derivative of the curve at a given parameter <paramref name="u"/>.
    /// </summary>
    /// <param name="u">The parameter value at which to evaluate the second derivative of the curve.</param>
    /// <returns>The second derivative of the curve as a 3D vector.</returns>
    [Pure]
    public Vector3 EvalSecondDerivative(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.EvalSecondDerivative(u);
    }

    /// <summary>
    /// Evaluates the third derivative of the curve.
    /// </summary>
    /// <returns>The third derivative of the curve as a 3D vector.</returns>
    [Pure]
    public Vector3 EvalThirdDerivative() => Curve.EvalThirdDerivative();
}
