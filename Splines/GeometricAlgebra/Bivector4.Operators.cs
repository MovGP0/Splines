using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Bivector4
{
    /// <summary>
    /// Negates the bivector.
    /// </summary>
    /// <param name="b">The bivector to negate.</param>
    /// <returns>The negated bivector.</returns>
    [Pure]
    public static Bivector4 operator -(Bivector4 b) => new(-b.XY, -b.XZ, -b.XW, -b.YZ, -b.YW, -b.ZW);

    /// <summary>
    /// Multiplies a bivector by a scalar.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The scaled bivector.</returns>
    [Pure]
    public static Bivector4 operator *(float a, Bivector4 b) => b * a;

    /// <summary>
    /// Multiplies a bivector by a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The scaled bivector.</returns>
    [Pure]
    public static Bivector4 operator *(Bivector4 a, float b) => new(a.XY * b, a.XZ * b, a.XW * b, a.YZ * b, a.YW * b, a.ZW * b);

    /// <summary>
    /// Multiplies a bivector by a vector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator *(Bivector4 a, Vector4 b)
    {
        // Compute the scalar part
        float r = -a.XY * b.Y - a.XZ * b.Z - a.XW * b.W - a.YZ * b.X - a.YW * b.X - a.ZW * b.X;

        // Compute the vector part
        Vector4 v = new Vector4(
            -a.YZ * b.Z - a.YW * b.W,
            a.XY * b.X - a.ZW * b.W,
            a.XZ * b.X + a.YZ * b.Y,
            a.XW * b.X + a.YW * b.Y
        );

        // Compute the bivector part
        Bivector4 bPart = new Bivector4(
            a.XY * b.W,
            -a.XZ * b.W,
            a.XW * b.Y,
            a.YZ * b.W,
            -a.YW * b.Z,
            a.ZW * b.Y
        );

        // The trivector and quadvector parts are zero in this case
        Trivector4 t = Trivector4.Zero;
        Quadvector4 q = Quadvector4.Zero;

        return new Multivector4(r, v, bPart, t, q);
    }

    /// <summary>
    /// Multiplies a vector by a bivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator *(Vector4 a, Bivector4 b)
    {
        // Compute the scalar part
        float r = -a.X * b.YZ - a.X * b.YW - a.X * b.ZW - a.Y * b.XY - a.Z * b.XZ - a.W * b.XW;

        // Compute the vector part
        Vector4 v = new Vector4(
            a.Y * b.XY - a.Z * b.XZ - a.W * b.XW,
            -a.X * b.XY + a.Z * b.YZ + a.W * b.YW,
            a.X * b.XZ - a.Y * b.YZ + a.W * b.ZW,
            a.X * b.XW - a.Y * b.YW - a.Z * b.ZW
        );

        // Compute the bivector part
        Bivector4 bPart = new Bivector4(
            -a.Z * b.XW,
            a.Y * b.XW,
            -a.X * b.YZ,
            a.X * b.YW,
            -a.X * b.ZW,
            a.X * b.YZ
        );

        // The trivector and quadvector parts are zero in this case
        Trivector4 t = Trivector4.Zero;
        Quadvector4 q = Quadvector4.Zero;

        return new Multivector4(r, v, bPart, t, q);
    }

    /// <summary>
    /// Divides a bivector by a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector4 operator /(Bivector4 a, float b) => new(a.XY / b, a.XZ / b, a.XW / b, a.YZ / b, a.YW / b, a.ZW / b);

    /// <summary>
    /// Adds two bivectors.
    /// </summary>
    /// <param name="a">The first bivector.</param>
    /// <param name="b">The second bivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector4 operator +(Bivector4 a, Bivector4 b) => new(a.XY + b.XY, a.XZ + b.XZ, a.XW + b.XW, a.YZ + b.YZ, a.YW + b.YW, a.ZW + b.ZW);

    /// <summary>
    /// Subtracts one bivector from another.
    /// </summary>
    /// <param name="a">The first bivector.</param>
    /// <param name="b">The second bivector.</param>
    /// <returns>The resulting bivector after subtraction.</returns>
    [Pure]
    public static Bivector4 operator -(Bivector4 a, Bivector4 b) => new(a.XY - b.XY, a.XZ - b.XZ, a.XW - b.XW, a.YZ - b.YZ, a.YW - b.YW, a.ZW - b.ZW);

    /// <summary>
    /// Adds a scalar to a bivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting bivector after addition.</returns>
    [Pure]
    public static Bivector4 operator +(float a, Bivector4 b) => new(b.XY + a, b.XZ + a, b.XW + a, b.YZ + a, b.YW + a, b.ZW + a);

    /// <summary>
    /// Adds a bivector to a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting bivector after addition.</returns>
    [Pure]
    public static Bivector4 operator +(Bivector4 a, float b) => new(a.XY + b, a.XZ + b, a.XW + b, a.YZ + b, a.YW + b, a.ZW + b);

    /// <summary>
    /// Subtracts a scalar from a bivector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting bivector after subtraction.</returns>
    [Pure]
    public static Bivector4 operator -(Bivector4 a, float b) => new(a.XY - b, a.XZ - b, a.XW - b, a.YZ - b, a.YW - b, a.ZW - b);

    /// <summary>
    /// Subtracts a bivector from a scalar.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting bivector after subtraction.</returns>
    [Pure]
    public static Bivector4 operator -(float a, Bivector4 b) => new(a - b.XY, a - b.XZ, a - b.XW, a - b.YZ, a - b.YW, a - b.ZW);

    /// <summary>
    /// Multiplies two bivectors to produce a trivector.
    /// </summary>
    /// <param name="a">The first bivector.</param>
    /// <param name="b">The second bivector.</param>
    /// <returns>The resulting trivector after multiplication.</returns>
    [Pure]
    public static Trivector4 operator *(Bivector4 a, Bivector4 b)
    {
        // Compute the trivector part as the wedge product of two bivectors
        return new Trivector4(
            a.XY * b.XZ - a.XZ * b.XY + a.XY * b.YZ - a.YZ * b.XY + a.XY * b.YW - a.YW * b.XY,
            a.XY * b.XW - a.XW * b.XY + a.XY * b.ZW - a.ZW * b.XY + a.XZ * b.XW - a.XW * b.XZ,
            a.XZ * b.YZ - a.YZ * b.XZ + a.XZ * b.ZW - a.ZW * b.XZ + a.XW * b.YW - a.YW * b.XW,
            a.XW * b.YW - a.YW * b.XW + a.YZ * b.ZW - a.ZW * b.YZ
        );
    }

    /// <summary>
    /// Explicit conversion from <see cref="Bivector4"/> to <see cref="Vector4"/>.
    /// </summary>
    /// <param name="bv">The bivector.</param>
    [Pure]
    public static explicit operator Vector4(Bivector4 bv) => new(bv.XY, bv.XZ, bv.XW, bv.YZ);

    /// <summary>
    /// Explicit conversion from <see cref="Vector4"/> to <see cref="Bivector4"/>.
    /// </summary>
    /// <param name="v">The vector.</param>
    [Pure]
    public static explicit operator Bivector4(Vector4 v) => new(v.X, v.Y, v.Z, v.W, v.W, v.W);
}
