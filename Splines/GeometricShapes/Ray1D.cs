using System.Runtime.CompilerServices;

namespace Splines.GeometricShapes;

/// <summary>
/// Similar to Unity's Ray2D, except this one allows you to not normalize the direction
/// which saves performance as well as allows you to work at different scales in 1D
/// </summary>
[Serializable]
public partial struct Ray1D : ILinear1D
{
    private float _direction;

    /// <summary>The origin of the ray</summary>
    public float Origin { get; set; }

    /// <summary>The direction of the ray. Note: Ray1D allows non-normalized direction values</summary>
    public float Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _direction;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { _direction = value; }
    }

    /// <summary>Returns a normalized version of this ray. Normalized rays ensure t-values correspond to distance</summary>
    [Pure]
    public Ray1D Normalized => new(Origin, Direction / Math.Abs(Direction));

    /// <summary>Creates a 1D Ray. Note: direction does not have to be normalized, but if it is, the t-value will correspond to distance along the ray</summary>
    /// <param name="origin">The origin of the ray</param>
    /// <param name="direction">The direction of the ray. It does not have to be normalized, but if it is, the t-value when sampling will correspond to distance along the ray</param>
    public Ray1D(float origin, float direction) => (Origin, _direction) = (origin, direction);

    /// <summary>Returns a point at <paramref name="distance"/> units along the ray.</summary>
    /// <param name="distance">The distance along the ray.</param>
    /// <returns>A point at the specified distance along the ray.</returns>
    [Pure]
    public float GetPoint(float distance) => Origin + Direction * distance;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool ILinear1D.IsValidTValue(float t) => t >= 0f;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    float ILinear1D.ClampTValue(float t) => t < 0f ? 0f : t;

    float ILinear1D.Origin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Origin;
    }

    float ILinear1D.Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Direction;
    }
}
