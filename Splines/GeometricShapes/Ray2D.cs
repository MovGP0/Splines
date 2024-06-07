using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>
/// Similar to Unity's Ray2D, except this one allows you to not normalize the direction
/// which saves performance as well as allows you to work at different scales
/// </summary>
[Serializable]
public partial struct Ray2D : ILinear2D
{
    /// <summary>The origin of the ray</summary>
    public Vector2 Origin { get; set; }

    /// <summary>The direction of the ray. Note: Ray2D allows non-normalized direction vectors</summary>
    public Vector2 Direction { get; set; }

    /// <summary>Returns a normalized version of this ray. Normalized rays ensure t-values correspond to distance</summary>
    [Pure]
    public Line2D Normalized => new(Origin, Direction);

    /// <summary>Creates a 2D Ray. Note: direction does not have to be normalized, but if it is, the t-value will correspond to distance along the ray</summary>
    /// <param name="origin">The origin of the ray</param>
    /// <param name="dir">The direction of the ray. It does not have to be normalized, but if it is, the t-value when sampling will correspond to distance along the ray</param>
    public Ray2D(Vector2 origin, Vector2 dir) => (Origin, Direction) = (origin, dir);

    /// <summary>Implicitly casts a Unity ray to a Mathfs ray</summary>
    /// <param name="ray3D">The ray to cast to a Unity ray</param>
    [Pure]
    public static implicit operator Ray2D(Ray3D ray3D) => new(ray3D.Origin.ToVector2(), ray3D.Direction.ToVector2());

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool ILinear2D.IsValidTValue(float t) => t >= 0f;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    float ILinear2D.ClampTValue(float t) => t < 0f ? 0f : t;

    Vector2 ILinear2D.Origin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Origin;
    }

    Vector2 ILinear2D.Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Direction;
    }
}
