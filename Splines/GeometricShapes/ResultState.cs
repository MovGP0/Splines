namespace Splines.GeometricShapes;

/// <summary>
/// Represents the result state of the polygon clipping operation.
/// </summary>
[EnumExtensions]
public enum ResultState
{
    /// <summary>
    /// Indicates that the original polygon was left intact, as no points needed to be discarded.
    /// </summary>
    OriginalLeftIntact,

    /// <summary>
    /// Indicates that the polygon was successfully clipped according to the clipping line.
    /// </summary>
    Clipped,

    /// <summary>
    /// Indicates that the entire polygon was discarded as it was on the discard side of the clipping line.
    /// </summary>
    FullyDiscarded
}
