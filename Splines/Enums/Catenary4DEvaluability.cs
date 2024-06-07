namespace Splines.Enums;

/// <summary>
/// Represents the evaluability state of a 4D catenary.
/// </summary>
/// <remarks>
/// This enum is used to indicate whether a 4D catenary is ready to be evaluated or not.
/// </remarks>
[EnumExtensions]
public enum Catenary4DEvaluability
{
    /// <summary>
    /// The 4D catenary is not ready to be evaluated.
    /// </summary>
    NotReady,

    /// <summary>
    /// The 4D catenary is ready to be evaluated.
    /// </summary>
    Ready
}
