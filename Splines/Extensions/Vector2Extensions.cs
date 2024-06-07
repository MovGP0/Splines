using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Unity;

namespace Splines.Extensions;

public static class Vector2Extensions
{
    /// <summary>Returns the signed curvature at a point in a curve, in radians per distance unit (equivalent to the reciprocal radius of the osculating circle)</summary>
    /// <param name="velocity">The first derivative of the point in the curve</param>
    /// <param name="acceleration">The second derivative of the point in the curve</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetCurvature(this Vector2 velocity, Vector2 acceleration)
    {
        float dMag = velocity.Magnitude();
        return Mathfs.Determinant(velocity, acceleration) / (dMag * dMag * dMag);
    }

    [Pure]
    public static Vector2 ToVector2(this Point p)
        => new((float)p.X, (float)p.Y);

    [Pure]
    public static Vector2 ToVector2(this Size p)
        => new((float)p.Width, (float)p.Height);

    [Pure]
    public static Point ToPoint(this Vector2 p)
        => new(p.X, p.Y);

    [Pure]
    public static Size ToSize(this Vector2 p)
        => new(p.X, p.Y);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SqrMagnitude(this Vector2 vector)
        => vector.X * vector.X + vector.Y * vector.Y;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Normalized(this Vector2 vector)
    {
        return vector == Vector2.Zero ? vector : Vector2.Normalize(vector);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 ToVector3(this Vector2 vector, float z = 0.0f)
        => new(vector.X, vector.Y, z);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 LerpUnclamped(this Vector2 a, Vector2 b, float t)
    {
        return new(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t
       );
    }

    [Pure]
    public static float Magnitude(this Vector2 vector)
    {
        return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
    }

    /// <summary>Returns the angle of this vector, in radians</summary>
    /// <param name="v">The vector to get the angle of. It does not have to be normalized</param>
    /// <seealso cref="Mathfs.DirToAng"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Angle(this Vector2 v)
        => Mathf.Atan2(v.Y, v.X);

    /// <summary>Rotates the vector 90 degrees clockwise (negative Z axis rotation)</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Rotate90CW(this Vector2 v)
        => new(v.Y, -v.X);

    /// <summary>Rotates the vector 90 degrees counter-clockwise (positive Z axis rotation)</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Rotate90CCW(this Vector2 v)
        => new(-v.Y, v.X);

    /// <summary>Rotates the vector around <c>pivot</c> with the given angle (in radians)</summary>
    /// <param name="v">The vector to rotate</param>
    /// <param name="pivot">The point to rotate around</param>
    /// <param name="angRad">The angle to rotate by, in radians</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 RotateAround(this Vector2 v, Vector2 pivot, float angRad)
        => Rotate(v - pivot, angRad) + pivot;

    /// <summary>Rotates the vector around <c>(0,0)</c> with the given angle (in radians)</summary>
    /// <param name="v">The vector to rotate</param>
    /// <param name="angRad">The angle to rotate by, in radians</param>
    [Pure]
    public static Vector2 Rotate(this Vector2 v, float angRad)
    {
        float ca = Mathf.Cos(angRad);
        float sa = Mathf.Sin(angRad);
        return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
    }

    /// <summary>Returns X and Y as a Vector2, equivalent to <c>new Vector2(v.x,v.y)</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 XY(this Vector2 v) => new Vector2(v.X, v.Y);

    /// <summary>Returns Y and X as a Vector2, equivalent to <c>new Vector2(v.y,v.x)</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 YX(this Vector2 v) => new Vector2(v.Y, v.X);

    /// <summary>Returns this vector as a Vector3, slotting X into X, and Y into Z, and the input value y into Y.
    /// Equivalent to <c>new Vector3(v.x,y,v.y)</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XZtoXYZ(this Vector2 v, float y = 0) => new Vector3(v.X, y, v.Y);

    /// <summary>Returns this vector as a Vector3, slotting X into X, and Y into Y, and the input value z into Z.
    /// Equivalent to <c>new Vector3(v.x,v.y,z)</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 XYtoXYZ(this Vector2 v, float z = 0) => new Vector3(v.X, v.Y, z);

    /// <summary>Sets X to 0</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 FlattenX(this Vector2 v) => new Vector2(0f, v.Y);

    /// <summary>Sets Y to 0</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 FlattenY(this Vector2 v) => new Vector2(v.X, 0f);

    /// <inheritdoc cref="ChebyshevMagnitude(Vector3)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ChebyshevMagnitude(this Vector2 v) => Mathfs.Max(Abs(v.X), Abs(v.Y));

    /// <inheritdoc cref="TaxicabMagnitude(Vector3)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float TaxicabMagnitude(this Vector2 v) => Abs(v.X) + Abs(v.Y);

    [Pure]
    private static float Abs(float value) => value < 0 ? -value : value;

    /// <summary>Returns a vector with the same direction, but with the given magnitude.
    /// Equivalent to <c>v.normalized*mag</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithMagnitude(this Vector2 v, float mag) => v.Normalized() * mag;

    /// <summary>Returns a vector with the same direction, but extending the magnitude by the given amount</summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 AddMagnitude(this Vector2 v, float extraMagnitude) => v * (1 + extraMagnitude / v.Magnitude());

    /// <summary>Returns the vector going from one position to another, also known as the displacement.
    /// Equivalent to <c>target-v</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 To(this Vector2 v, Vector2 target) => target - v;

    /// <summary>Returns the normalized direction from this vector to the target.
    /// Equivalent to <c>(target-v).normalized</c> or <c>v.To(target).normalized</c></summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 DirTo(this Vector2 v, Vector2 target) => (target - v).Normalized();

    /// <summary>Mirrors this vector around another point. Equivalent to rotating the vector 180° around the point</summary>
    /// <param name="p">The point to mirror</param>
    /// <param name="pivot">The point to mirror around</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 MirrorAround(this Vector2 p, Vector2 pivot) => new(2 * pivot.X - p.X, 2 * pivot.Y - p.Y);

    /// <summary>Mirrors this vector around an x coordinate</summary>
    /// <param name="p">The point to mirror</param>
    /// <param name="xPivot">The x coordinate to mirror around</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 MirrorAroundX(this Vector2 p, float xPivot) => new(2 * xPivot - p.X, p.Y);

    /// <summary>Mirrors this vector around a y coordinate</summary>
    /// <param name="p">The point to mirror</param>
    /// <param name="yPivot">The y coordinate to mirror around</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 MirrorAroundY(this Vector2 p, float yPivot) => new(p.X, 2 * yPivot - p.Y);

    /// <summary>Scale the point <c>p</c> around <c>pivot</c> by <c>scale</c></summary>
    /// <param name="p">The point to scale</param>
    /// <param name="pivot">The pivot to scale around</param>
    /// <param name="scale">The scale to scale by</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ScaleAround(this Vector2 p, Vector2 pivot, Vector2 scale) => new(pivot.X + (p.X - pivot.X) * scale.X, pivot.Y + (p.Y - pivot.Y) * scale.Y);

    /// <inheritdoc cref="Mathfs.Sqrt(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Sqrt(this Vector2 value) => Mathfs.Sqrt(value);

    /// <inheritdoc cref="Mathfs.Abs(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Abs(this Vector2 v) => Mathfs.Abs(v);

    /// <inheritdoc cref="Mathfs.Clamp(Vector2,Vector2,Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp(this Vector2 v, Vector2 min, Vector2 max) => Mathfs.Clamp(v, min, max);

    /// <inheritdoc cref="Mathfs.Clamp01(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Clamp01(this Vector2 v) => Mathfs.Clamp01(v);

    /// <inheritdoc cref="Mathfs.ClampNeg1to1(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ClampNeg1to1(this Vector2 v) => Mathfs.ClampNeg1to1(v);

    /// <inheritdoc cref="Mathfs.Min(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(this Vector2 v) => Mathfs.Min(v);

    /// <inheritdoc cref="Mathfs.Max(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(this Vector2 v) => Mathfs.Max(v);

    /// <inheritdoc cref="Mathfs.Sign(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Sign(this Vector2 value) => Mathfs.Sign(value);

    /// <inheritdoc cref="Mathfs.SignWithZero(Vector2,float)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 SignWithZero(this Vector2 value, float zeroThreshold = 0.000001f) => Mathfs.SignWithZero(value, zeroThreshold);

    /// <inheritdoc cref="Mathfs.Floor(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Floor(this Vector2 value) => Mathfs.Floor(value);

    /// <inheritdoc cref="Mathfs.FloorToInt(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int FloorToInt(this Vector2 value) => Mathfs.FloorToInt(value);

    /// <inheritdoc cref="Mathfs.Ceil(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Ceil(this Vector2 value)
        => Mathfs.Ceil(value);

    /// <inheritdoc cref="Mathfs.CeilToInt(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int CeilToInt(this Vector2 value) => Mathfs.CeilToInt(value);

    /// <inheritdoc cref="Mathfs.Round(Vector2,MidpointRounding)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(this Vector2 value, MidpointRounding midpointRounding = MidpointRounding.ToEven)
        => Mathfs.Round(value, midpointRounding);

    /// <inheritdoc cref="Mathfs.Round(Vector2,MidpointRounding)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Round(this Vector2 value, float snapInterval, MidpointRounding midpointRounding = MidpointRounding.ToEven)
        => Mathfs.Round(value, snapInterval, midpointRounding);

    /// <inheritdoc cref="Mathfs.RoundToInt(Vector2,MidpointRounding)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int RoundToInt(this Vector2 value, MidpointRounding midpointRounding = MidpointRounding.ToEven)
        => Mathfs.RoundToInt(value, midpointRounding);

    /// <inheritdoc cref="Mathfs.Frac(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Frac(this Vector2 v) => Mathfs.Frac(v);

    /// <inheritdoc cref="Mathfs.GetDirAndMagnitude(Vector2)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (Vector2 dir, float magnitude) GetDirAndMagnitude(this Vector2 v)
        => Mathfs.GetDirAndMagnitude(v);

    /// <inheritdoc cref="Mathfs.ClampMagnitude(Vector2,float,float)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 ClampMagnitude(this Vector2 v, float min, float max)
        => Mathfs.ClampMagnitude(v, min, max);
}
