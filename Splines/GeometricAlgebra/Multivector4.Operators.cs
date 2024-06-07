using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Multivector4
{
    /// <summary>
    /// Multiplies two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator *(Multivector4 a, Multivector4 b)
    {
        return new(
            a.R * b.R + a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W + a.XY * b.XY + a.XZ * b.XZ + a.XW * b.XW + a.YZ * b.YZ + a.YW * b.YW + a.ZW * b.ZW + a.XYZ * b.XYZ + a.XYW * b.XYW + a.XZW * b.XZW + a.YZW * b.YZW + a.XYZW * b.XYZW,
            a.R * b.X + a.X * b.R - a.Y * b.XY - a.Z * b.XZ - a.W * b.XW - a.XY * b.Y - a.XZ * b.Z - a.XW * b.W - a.YZ * b.XYZ - a.YW * b.XYW - a.ZW * b.XZW + a.XYZ * b.YZ + a.XYW * b.YW + a.XZW * b.ZW - a.YZW * b.XYZW - a.XYZW * b.YZW,
            a.R * b.Y + a.X * b.XY + a.Y * b.R - a.Z * b.YZ - a.W * b.YW + a.XY * b.X - a.XZ * b.XYZ - a.XW * b.XYW + a.YZ * b.Z - a.YW * b.W - a.ZW * b.YZW + a.XYZ * b.XZ + a.XYW * b.XW + a.XZW * b.ZW - a.YZW * b.XYZW - a.XYZW * b.XYZW,
            a.R * b.Z + a.X * b.XZ + a.Y * b.YZ + a.Z * b.R - a.W * b.ZW + a.XY * b.XYZ - a.XZ * b.X - a.XW * b.XZW + a.YZ * b.Y - a.YW * b.YZW + a.ZW * b.W - a.XYZ * b.XY + a.XYW * b.YW + a.XZW * b.XZW - a.YZW * b.XYZW - a.XYZW * b.XYZW,
            a.R * b.W + a.X * b.XW + a.Y * b.YW + a.Z * b.ZW + a.W * b.R + a.XY * b.XYW + a.XZ * b.XZW + a.XW * b.X - a.YZ * b.YZW + a.YW * b.Y - a.ZW * b.Z - a.XYZ * b.XYZW + a.XYW * b.XY - a.XZW * b.XZ + a.YZW * b.YZ - a.XYZW * b.XYZW,
            a.R * b.XY + a.X * b.Y - a.Y * b.X + a.Z * b.XYZ + a.W * b.XYW + a.XY * b.R - a.XZ * b.YZW + a.XW * b.ZW - a.YZ * b.XZ - a.YW * b.Z + a.ZW * b.Y + a.XYZ * b.YZ - a.XYW * b.W + a.XZW * b.X - a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.XZ + a.X * b.Z - a.Y * b.XYZ + a.Z * b.X + a.W * b.XZW + a.XY * b.YZW - a.XZ * b.R - a.XW * b.YZW - a.YZ * b.Y - a.YW * b.YZW - a.ZW * b.W + a.XYZ * b.XY + a.XYW * b.W - a.XZW * b.X - a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.XW + a.X * b.W - a.Y * b.XYW - a.Z * b.XZW + a.W * b.X + a.XY * b.ZW - a.XZ * b.YZW - a.XW * b.R + a.YZ * b.YZW - a.YW * b.Y - a.ZW * b.Z + a.XYZ * b.XYW - a.XYW * b.XY + a.XZW * b.X + a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.YZ + a.X * b.XYZ + a.Y * b.Z - a.Z * b.Y + a.W * b.YZW + a.XY * b.XZ + a.XZ * b.X + a.XW * b.XYW - a.YZ * b.R - a.YW * b.ZW + a.ZW * b.W + a.XYZ * b.X + a.XYW * b.YW - a.XZW * b.ZW + a.YZW * b.Y - a.XYZW * b.XYZW,
            a.R * b.YW + a.X * b.XYW + a.Y * b.ZW + a.Z * b.YZW - a.W * b.Y + a.XY * b.XW + a.XZ * b.YZW + a.XW * b.X - a.YZ * b.ZW - a.YW * b.R + a.ZW * b.Z + a.XYZ * b.XYW - a.XYW * b.X + a.XZW * b.YZW - a.YZW * b.Y + a.XYZW * b.XYZW,
            a.R * b.ZW + a.X * b.XZW + a.Y * b.YZW + a.Z * b.Z + a.W * b.R + a.XY * b.YZW + a.XZ * b.X + a.XW * b.XW - a.YZ * b.XYW - a.YW * b.Y - a.ZW * b.R + a.XYZ * b.ZW - a.XYW * b.XYW + a.XZW * b.X - a.YZW * b.YZW + a.XYZW * b.XYZW,
            a.R * b.XYZ + a.X * b.YZ - a.Y * b.XZ + a.Z * b.XY - a.W * b.XYZW + a.XY * b.Z + a.XZ * b.Y - a.XW * b.XYW - a.YZ * b.X + a.YW * b.YZW - a.ZW * b.XYW + a.XYZ * b.R + a.XYW * b.YZW - a.XZW * b.XZW - a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.XYW + a.X * b.YW - a.Y * b.XW - a.Z * b.XYW + a.W * b.XY + a.XY * b.W + a.XZ * b.ZW - a.XW * b.XZ - a.YZ * b.XYW + a.YW * b.R - a.ZW * b.YZW + a.XYZ * b.YZW + a.XYW * b.R - a.XZW * b.XZW + a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.XZW + a.X * b.ZW + a.Y * b.XYW - a.Z * b.XW - a.W * b.XZ + a.XY * b.YZW - a.XZ * b.W - a.XW * b.YZW + a.YZ * b.XYW + a.YW * b.Y - a.ZW * b.R + a.XYZ * b.YZW - a.XYW * b.XY + a.XZW * b.R - a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.YZW + a.X * b.XYZW + a.Y * b.ZW + a.Z * b.YW + a.W * b.YZ + a.XY * b.XYW - a.XZ * b.XZW + a.XW * b.X - a.YZ * b.R - a.YW * b.Y - a.ZW * b.Z + a.XYZ * b.XYW + a.XYW * b.XZW - a.XZW * b.YZW + a.YZW * b.R - a.XYZW * b.XYZW,
            a.R * b.XYZW + a.X * b.YZW + a.Y * b.XZW + a.Z * b.XYW + a.W * b.XYZ + a.XY * b.XYZ - a.XZ * b.XYW + a.XW * b.XYW - a.YZ * b.YZW - a.YW * b.XZW + a.ZW * b.XY + a.XYZ * b.XYW - a.XYW * b.XYW + a.XZW * b.XZW - a.YZW * b.YZW + a.XYZW * b.R
       );
    }

    // /// <summary>
    // /// Multiplies a multivector by a rotor.
    // /// </summary>
    // /// <param name="m">The multivector.</param>
    // /// <param name="r">The rotor.</param>
    // /// <returns>The resulting multivector.</returns>
    // [Pure]
    // public static Multivector4 operator *(Multivector4 m, Rotor4 r)
    // {
    //     // Multiply the scalar part by the rotor
    //     float scalarPart = m.R * r.R;
    // 
    //     // Multiply the vector part by the rotor
    //     Vector4 vectorPart = r * m.V;
    // 
    //     // Multiply the bivector part by the rotor
    //     Bivector4 bivectorPart = r * m.B;
    // 
    //     // Multiply the trivector part by the rotor
    //     Trivector4 trivectorPart = r * m.T;
    // 
    //     // Multiply the quadvector part by the rotor
    //     Quadvector4 quadvectorPart = r * m.Q;
    // 
    //     return new Multivector4(scalarPart, vectorPart, bivectorPart, trivectorPart, quadvectorPart);
    // }

    /// <summary>
    /// Computes the wedge product of two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector4 Wedge(Multivector4 a, Multivector4 b) =>
        new(
            a.R * b.XY + a.X * b.Y - a.Y * b.X + a.Z * b.XYZ - a.W * b.XYW + a.XY * b.R - a.XZ * b.YZW + a.XW * b.ZW - a.YZ * b.XZ - a.YW * b.Z + a.ZW * b.Y + a.XYZ * b.YZ - a.XYW * b.W + a.XZW * b.X - a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.XZ + a.X * b.Z - a.Y * b.XYZ + a.Z * b.X + a.W * b.XZW + a.XY * b.YZW - a.XZ * b.R - a.XW * b.YZW - a.YZ * b.Y - a.YW * b.YZW - a.ZW * b.W + a.XYZ * b.XY + a.XYW * b.W - a.XZW * b.X - a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.XW + a.X * b.W - a.Y * b.XYW - a.Z * b.XZW + a.W * b.X + a.XY * b.XZW - a.XZ * b.XYW - a.XW * b.R + a.YZ * b.YZW - a.YW * b.Y - a.ZW * b.Z + a.XYZ * b.XYW - a.XYW * b.XY + a.XZW * b.X + a.YZW * b.YZW - a.XYZW * b.XYZW,
            a.R * b.YZ + a.X * b.XYZ + a.Y * b.Z - a.Z * b.Y + a.W * b.YZW + a.XY * b.XZ + a.XZ * b.X + a.XW * b.XYW - a.YZ * b.R - a.YW * b.ZW + a.ZW * b.W + a.XYZ * b.X + a.XYW * b.YW - a.XZW * b.ZW + a.YZW * b.Y - a.XYZW * b.XYZW,
            a.R * b.YW + a.X * b.XYW + a.Y * b.W + a.Z * b.YZW - a.W * b.Y + a.XY * b.XW + a.XZ * b.XZW + a.XW * b.X - a.YZ * b.ZW - a.YW * b.R + a.ZW * b.Z + a.XYZ * b.XYW - a.XYW * b.X + a.XZW * b.YZW - a.YZW * b.Y + a.XYZW * b.XYZW,
            a.R * b.ZW + a.X * b.XZW + a.Y * b.YZW + a.Z * b.Z + a.W * b.R + a.XY * b.YZW + a.XZ * b.X + a.XW * b.XW - a.YZ * b.XYW - a.YW * b.Y - a.ZW * b.R + a.XYZ * b.ZW - a.XYW * b.XYW + a.XZW * b.X - a.YZW * b.YZW + a.XYZW * b.XYZW
        );

    /// <summary>
    /// Computes the dot product of two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float Dot(Multivector4 a, Multivector4 b) => a.R * b.R + a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W + a.XY * b.XY + a.XZ * b.XZ + a.XW * b.XW + a.YZ * b.YZ + a.YW * b.YW + a.ZW * b.ZW + a.XYZ * b.XYZ + a.XYW * b.XYW + a.XZW * b.XZW + a.YZW * b.YZW + a.XYZW * b.XYZW;

    /// <summary>
    /// Adds a scalar to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, float b) => new(a.R + b, a.V, a.B, a.T, a.Q);

    /// <summary>
    /// Adds a scalar to a multivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(float a, Multivector4 b) => b + a;

    /// <summary>
    /// Adds a vector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, Vector4 b) => new(a.R, a.V + b, a.B, a.T, a.Q);

    /// <summary>
    /// Adds a vector to a multivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Vector4 a, Multivector4 b) => b + a;

    /// <summary>
    /// Adds a bivector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, Bivector4 b) => new(a.R, a.V, a.B + b, a.T, a.Q);

    /// <summary>
    /// Adds a bivector to a multivector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Bivector4 a, Multivector4 b) => b + a;

    /// <summary>
    /// Adds a trivector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, Trivector4 b) => new(a.R, a.V, a.B, a.T + b, a.Q);

    /// <summary>
    /// Adds a trivector to a multivector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Trivector4 a, Multivector4 b) => b + a;

    /// <summary>
    /// Adds a quadvector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The quadvector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, Quadvector4 b) => new(a.R, a.V, a.B, a.T, a.Q + b);

    /// <summary>
    /// Adds a quadvector to a multivector.
    /// </summary>
    /// <param name="a">The quadvector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Quadvector4 a, Multivector4 b) => b + a;

    /// <summary>
    /// Adds two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, Multivector4 b) => new(a.R + b.R, a.V + b.V, a.B + b.B, a.T + b.T, a.Q + b.Q);

    /// <summary>
    /// Adds a rotor to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Multivector4 a, Rotor4 b) => new(a.R + b.R, a.V, a.B + b.B, a.T, a.Q);

    /// <summary>
    /// Adds a rotor to a multivector.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector4 operator +(Rotor4 a, Multivector4 b) => b + a;
}
