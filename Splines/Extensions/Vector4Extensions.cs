using System.Numerics;
using System.Runtime.CompilerServices;

namespace Splines.Extensions;

public static class Vector4Extensions
{
    /// <summary>
    /// Returns the direction (normalized vector) and magnitude of a 4D vector.
    /// </summary>
    /// <param name="vector">The 4D vector.</param>
    /// <returns>A tuple containing the normalized vector and its magnitude.</returns>
    [Pure]
    public static (Vector4 normal, float magnitude) GetDirectionAndMagnitude(this Vector4 vector)
    {
        float magnitude = vector.Length();
        Vector4 normal = Vector4.Normalize(vector);
        return (normal, magnitude);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Normalized(this Vector4 vector)
        => Vector4.Normalize(vector);

    /// <summary>
    /// Linearly interpolates between two vectors without clamping the interpolant
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 LerpUnclamped(this Vector4 left, Vector4 right, float t)
    {
        return new(
            left.X + (right.X - left.X) * t,
            left.Y + (right.Y - left.Y) * t,
            left.Z + (right.Z - left.Z) * t,
            left.W + (right.W - left.W) * t);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ToVector3(this Vector4 v) => new(v.X, v.Y, v.Z);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SqrMagnitude(this Vector4 v)
        => v.X * v.X + v.Y * v.Y + v.Z * v.Z + v.W * v.W;

    /// <summary>
    /// Returns a vector that is perpendicular to this vector.
    /// Note: In 4D, there are infinitely many perpendicular vectors. This method returns one possible perpendicular vector.
    /// </summary>
    public static Vector4 Perpendicular(this Vector4 v)
    {
        if (v.X != 0 || v.Y != 0)
        {
            // If the first two components are non-zero, create a perpendicular vector in the XY plane
            return new Vector4(-v.Y, v.X, 0, 0);
        }

        if (v.Z != 0)
        {
            // If only the Z component is non-zero, create a perpendicular vector in the ZW plane
            return new Vector4(0, 0, -v.W, v.Z);
        }

        // If only the W component is non-zero, create a perpendicular vector in the ZW plane
        return new Vector4(0, 0, -v.W, v.Z);
    }
}
