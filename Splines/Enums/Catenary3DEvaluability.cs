namespace Splines.Enums;

/// <summary>
/// Represents the evaluability state of a 3D catenary.
/// </summary>
/// <remarks>
/// This enum is used to indicate whether a 3D catenary is ready to be evaluated or not.
/// </remarks>
[EnumExtensions]
public enum Catenary3DEvaluability
{
    /// <summary>
    /// The 3D catenary is not ready to be evaluated.
    /// </summary>
    NotReady,

    /// <summary>
    /// The 3D catenary is ready to be evaluated.
    /// </summary>
    Ready
}
