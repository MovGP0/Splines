namespace Splines.GeometricShapes;

/// <summary>
/// Represents the state of a point relative to a clipping line in the polygon clipping process.
/// </summary>
[EnumExtensions]
internal enum PointSideState
{
    /// <summary>
    /// The point is on the side of the clipping line where it should be discarded.
    /// </summary>
    Discard = -1,

    /// <summary>
    /// The point is exactly on the clipping line.
    /// </summary>
    Edge = 0,

    /// <summary>
    /// The point is on the side of the clipping line where it should be kept.
    /// </summary>
    Keep = 1,

    /// <summary>
    /// The point has been handled during the polygon section extraction process.
    /// </summary>
    Handled = 2
}
