namespace Splines.Splines.MultiSegmentSplines;

/// <summary>
/// Specifies how the endpoints of a spline are handled.
/// </summary>
[EnumExtensions]
public enum EndpointMode
{
    /// <summary>
    /// No special handling of the endpoints.
    /// </summary>
    None,

    /// <summary>
    /// Extrapolate the spline beyond the first and last control points.
    /// </summary>
    Extrapolate,

    /// <summary>
    /// Collapse the endpoints to the first and last control points.
    /// </summary>
    Collapse
}
