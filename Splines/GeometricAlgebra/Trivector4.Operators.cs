using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Trivector4
{
    /// <summary>
    /// Adds two trivectors.
    /// </summary>
    /// <param name="a">The first trivector.</param>
    /// <param name="b">The second trivector.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector4 operator +(Trivector4 a, Trivector4 b) =>
        new(a.XYZ + b.XYZ, a.XYW + b.XYW, a.XZW + b.XZW, a.YZW + b.YZW);

    /// <summary>
    /// Subtracts one trivector from another.
    /// </summary>
    /// <param name="a">The first trivector.</param>
    /// <param name="b">The second trivector.</param>
    /// <returns>The resulting trivector after subtraction.</returns>
    [Pure]
    public static Trivector4 operator -(Trivector4 a, Trivector4 b) =>
        new Trivector4(a.XYZ - b.XYZ, a.XYW - b.XYW, a.XZW - b.XZW, a.YZW - b.YZW);

    /// <summary>
    /// Negates a trivector.
    /// </summary>
    /// <param name="t">The trivector to negate.</param>
    /// <returns>The negated trivector.</returns>
    [Pure]
    public static Trivector4 operator -(Trivector4 t) =>
        new Trivector4(-t.XYZ, -t.XYW, -t.XZW, -t.YZW);

    /// <summary>
    /// Multiplies a trivector by a scalar.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector4 operator *(Trivector4 a, float b) =>
        new(a.XYZ * b, a.XYW * b, a.XZW * b, a.YZW * b);

    /// <summary>
    /// Multiplies a scalar by a trivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector4 operator *(float a, Trivector4 b) => b * a;

    /// <summary>
    /// Multiplies a trivector by a vector, resulting in a bivector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector4 operator *(Trivector4 a, Vector4 b) =>
        new(
            a.XYZ * b.W - a.XYW * b.Z + a.XZW * b.Y,
            a.XYZ * b.W + a.XYW * b.Z - a.YZW * b.X,
            a.XYW * b.X - a.XZW * b.W + a.YZW * b.X,
            a.XZW * b.X - a.YZW * b.Y + a.YZW * b.X,
            a.XZW * b.W + a.YZW * b.X,
            a.YZW * b.W
        );

    /// <summary>
    /// Multiplies a vector by a trivector, resulting in a bivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector4 operator *(Vector4 a, Trivector4 b) =>
        new(
            a.W * b.XYZ - a.Z * b.XYW + a.Y * b.XZW,
            a.W * b.XYZ + a.Z * b.XYW - a.X * b.YZW,
            a.X * b.XYW - a.W * b.XZW + a.X * b.YZW,
            a.X * b.XZW - a.Y * b.YZW + a.X * b.YZW,
            a.W * b.XZW + a.X * b.YZW,
            a.W * b.YZW
        );

    /// <summary>
    /// Multiplies a bivector by a trivector, resulting in a vector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting vector.</returns>
    [Pure]
    public static Vector4 operator *(Bivector4 a, Trivector4 b) =>
        new(
            -a.YZ * b.XYZ - a.XZ * b.XYW - a.XW * b.XZW,
            -a.XY * b.XYZ - a.XZ * b.YZW - a.XW * b.XYW,
            -a.XY * b.XZW - a.YZ * b.YZW - a.YW * b.YZW,
            -a.XY * b.XYW - a.YZ * b.YZW - a.XW * b.XZW
        );

    /// <summary>
    /// Multiplies a trivector by a bivector, resulting in a vector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting vector.</returns>
    [Pure]
    public static Vector4 operator *(Trivector4 a, Bivector4 b) =>
        new(
            -a.XYZ * b.YZ - a.XYW * b.XZ - a.XZW * b.XW,
            -a.XYZ * b.XY - a.YZW * b.XZ - a.XYW * b.YW,
            -a.XZW * b.XY - a.YZW * b.YZ - a.XYW * b.YW,
            -a.XYW * b.XY - a.YZW * b.YZ - a.XZW * b.XW
        );

    /// <summary>
    /// Computes the scalar product of two trivectors.
    /// </summary>
    /// <param name="a">The first trivector.</param>
    /// <param name="b">The second trivector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float operator *(Trivector4 a, Trivector4 b) =>
        -a.XYZ * b.XYZ - a.XYW * b.XYW - a.XZW * b.XZW - a.YZW * b.YZW;
}
