using Splines.Curves;

namespace Splines.Splines.MultiSegmentSplines;

/// <summary>
/// Represents a node in a multi-segment spline, containing the position, knot value,
/// and polynomial curve associated with the node.
/// </summary>
public partial struct Node1
{
    /// <summary>
    /// Gets or sets the position of the node.
    /// </summary>
    /// <value>The float value representing the position of the node.</value>
    public float Position
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
    /// <value>The 1D polynomial curve.</value>
    public Polynomial1D Curve
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Evaluates the position on the curve at a given parameter <paramref name="u"/>.
    /// </summary>
    /// <param name="u">The parameter value at which to evaluate the curve.</param>
    /// <returns>The position on the curve as a float.</returns>
    [Pure]
    public float EvalPoint(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.Eval(u);
    }

    /// <summary>
    /// Evaluates the first derivative of the curve at a given parameter <paramref name="u"/>.
    /// </summary>
    /// <param name="u">The parameter value at which to evaluate the first derivative of the curve.</param>
    /// <returns>The first derivative of the curve as a float.</returns>
    [Pure]
    public float EvalDerivative(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.EvalDerivative(u);
    }

    [Pure]
    public float EvalSecondDerivative(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.EvalSecondDerivative(u);
    }

    [Pure]
    public float EvalThirdDerivative(float u)
    {
        u -= Knot; // transform to local knot value
        return Curve.EvalThirdDerivative(u);
    }
}
