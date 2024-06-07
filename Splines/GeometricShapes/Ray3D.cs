using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>
/// Representation of a ray in 3D space.
/// </summary>
public partial struct Ray3D : ILinear3D
{
    private Vector3 _direction;

    /// <summary>
    /// Creates a ray starting at <paramref name="origin"/> along <paramref name="direction"/>.
    /// </summary>
    /// <param name="origin">The origin point of the ray.</param>
    /// <param name="direction">The direction of the ray.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Ray3D(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        _direction = direction.Normalized();
    }

    /// <summary>
    /// The origin point of the ray.
    /// </summary>
    public Vector3 Origin
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    /// <summary>
    /// The direction of the ray.
    /// </summary>
    public Vector3 Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _direction;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set { _direction = value.Normalized(); }
    }

    /// <summary>
    /// Returns a point at <paramref name="distance"/> units along the ray.
    /// </summary>
    /// <param name="distance">The distance along the ray.</param>
    /// <returns>A point at the specified distance along the ray.</returns>
    [Pure]
    public Vector3 GetPoint(float distance) => Origin + Direction * distance;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool ILinear3D.IsValidTValue(float t) => t >= 0f;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    float ILinear3D.ClampTValue(float t) => t < 0f ? 0f : t;
}
