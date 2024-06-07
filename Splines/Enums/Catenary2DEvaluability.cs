namespace Splines.Enums;

/// <summary>
/// Represents the evaluability state of a 2D catenary.
/// </summary>
/// <remarks>
/// This enum is used to indicate whether a 2D catenary is ready to be evaluated or not.
/// </remarks>
[EnumExtensions]
public enum Catenary2DEvaluability
{
    /// <summary>
    /// The 2D catenary is not ready to be evaluated.
    /// </summary>
    NotReady,

    /// <summary>
    /// The 2D catenary is ready to be evaluated.
    /// </summary>
    Ready
}
