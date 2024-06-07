using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.GeometricAlgebra;

namespace Splines.Extensions;

public static class Vector3Extensions
{
    /// <summary>Returns the curvature of a point in a 3D curve, as a Bivector.
    /// The magnitude is the curvature in radians per distance unit,
    /// casting it to a Vector3 gives you the axis of curvature</summary>
    /// <param name="velocity">The first derivative of the point in the curve</param>
    /// <param name="acceleration">The second derivative of the point in the curve</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bivector3 GetCurvature(Vector3 velocity, Vector3 acceleration)
    {
        float dMag = velocity.Magnitude();
        return Mathfs.Wedge(velocity, acceleration) / (dMag * dMag * dMag);
    }

    [Pure]
    public static (Vector3 normal, float magnitude) GetDirectionAndMagnitude(this Vector3 vector)
    {
        float magnitude = vector.Length();
        Vector3 normal = Vector3.Normalize(vector);
        return (normal, magnitude);
    }

    [Pure]
    public static float Magnitude(this Vector3 vector)
        => (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 Normalized(this Vector3 vector)
        => Vector3.Normalize(vector);

    /// <summary>
    /// Calculate the squared magnitude of the vector
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SqrMagnitude(this Vector3 vector)
        => vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 LerpUnclamped(this Vector3 a, Vector3 b, float t)
    {
        return new(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t,
            a.Z + (b.Z - a.Z) * t
       );
    }

    /// <summary>
    /// This method returns the vector as a Vector2 by truncating the Z component
    /// </summary>
    [Pure]
    public static Vector2 ToVector2(this Vector3 vector)
        => new(vector.X, vector.Y);

    [Pure]
    public static Vector3 SlerpUnclamped(this Vector3 a, Vector3 b, float t)
    {
        float dot = Vector3.Dot(a, b);
        dot = dot.Clamp(-1f, 1f);
        float theta = (float)Math.Acos(dot) * t;
        Vector3 relative = b - a * dot;
        relative = Vector3.Normalize(relative);
        return a * (float)Math.Cos(theta) + relative * (float)Math.Sin(theta);
    }

    [Pure]
    public static Vector3 Rotate90CCW(this Vector3 vector)
    {
        // 90-degree rotation around Z-axis in counter-clockwise direction
        return new Vector3(-vector.Y, vector.X, vector.Z);
    }
}
