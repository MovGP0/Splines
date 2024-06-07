namespace Splines.Enums;

/// <summary>
/// Represents the evaluability state of a catenary to a point.
/// </summary>
/// <remarks>
/// This enum is used to indicate the type of evaluation that can be performed for a catenary to a point.
/// </remarks>
[EnumExtensions]
internal enum CatenaryToPointEvaluability
{
    /// <summary>
    /// The evaluability state is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The evaluation is in the form of a catenary.
    /// </summary>
    Catenary,

    /// <summary>
    /// The evaluation is in the form of a linear vertical segment.
    /// </summary>
    LinearVertical,

    /// <summary>
    /// The evaluation is in the form of a line segment.
    /// </summary>
    LineSegment
}
