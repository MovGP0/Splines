namespace Splines.Splines.NonUniformSplineSegments;

/// <summary>
/// Specifies the calculation modes for knot vectors in spline curve generation.
/// </summary>
[EnumExtensions]
public enum KnotCalcMode
{
    /// <summary>
    /// The knots are manually specified by the user.
    /// </summary>
    Manual,

    /// <summary>
    /// The knots are automatically calculated based on the input points.
    /// </summary>
    Auto,

    /// <summary>
    /// The knots are automatically calculated, normalized to the unit interval [0, 1].
    /// </summary>
    AutoUnitInterval
}
