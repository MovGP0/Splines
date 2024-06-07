using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Rotor3
{
    /// <summary>
    /// Multiplies two rotors.
    /// </summary>
    /// <param name="a">The first rotor.</param>
    /// <param name="b">The second rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator *(Rotor3 a, Rotor3 b)
    {
        return new Rotor3(
            a.R * b.R - a.YZ * b.YZ - a.ZX * b.ZX - a.XY * b.XY,
            a.R * b.YZ + a.YZ * b.R - a.ZX * b.XY + a.XY * b.ZX,
            a.R * b.ZX + a.YZ * b.XY + a.ZX * b.R - a.XY * b.YZ,
            a.R * b.XY - a.YZ * b.ZX + a.ZX * b.YZ + a.XY * b.R
       );
    }

    /// <summary>
    /// Multiplies a bivector by a rotor.
    /// </summary>
    /// <param name="b">The bivector.</param>
    /// <param name="r">The rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator *(Bivector3 b, Rotor3 r)
    {
        return new Rotor3(
            -b.YZ * r.YZ - b.ZX * r.ZX - b.XY * r.XY,
            +b.YZ * r.R - b.ZX * r.XY + b.XY * r.ZX,
            +b.YZ * r.XY + b.ZX * r.R - b.XY * r.YZ,
            -b.YZ * r.ZX + b.ZX * r.YZ + b.XY * r.R
       );
    }

    /// <summary>
    /// Multiplies a rotor by a bivector.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator *(Rotor3 a, Bivector3 b)
    {
        return new Rotor3(
            a.YZ * b.YZ + a.ZX * b.ZX + a.XY * b.XY,
            a.R * b.YZ - a.ZX * b.XY + a.XY * b.ZX,
            a.R * b.ZX + a.YZ * b.XY + -a.XY * b.YZ,
            a.R * b.XY - a.YZ * b.ZX + a.ZX * b.YZ
       );
    }

    /// <summary>
    /// Multiplies a rotor by a vector, resulting in a multivector.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator *(Rotor3 a, Vector3 b)
    {
        return new Multivector3(
            0,
            a.R * b.X - a.ZX * b.Z + a.XY * b.Y,
            a.R * b.Y + a.YZ * b.Z - a.XY * b.X,
            a.R * b.Z - a.YZ * b.Y + a.ZX * b.X,
            0,
            0,
            0,
            a.YZ * b.X + a.ZX * b.Y + a.XY * b.Z
       );
    }

    /// <summary>
    /// Multiplies a vector by a rotor, resulting in a multivector.
    /// </summary>
    /// <param name="b">The vector.</param>
    /// <param name="a">The rotor.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator *(Vector3 b, Rotor3 a)
    {
        return new Multivector3(
            0,
            a.R * b.X - a.ZX * b.Z + a.XY * b.Y,
            a.R * b.Y + a.YZ * b.Z - a.XY * b.X,
            a.R * b.Z - a.YZ * b.Y + a.ZX * b.X,
            0,
            0,
            0,
            a.YZ * b.X + a.ZX * b.Y + a.XY * b.Z
       );
    }

    /// <summary>
    /// Divides a rotor by a scalar.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator /(Rotor3 a, float b)
    {
        return new Rotor3(a.R / b, a.YZ / b, a.ZX / b, a.XY / b);
    }

    /// <summary>
    /// Adds a scalar to a rotor.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator +(Rotor3 a, float b) => new(a.R + b, a.B);

    /// <summary>
    /// Adds a scalar to a rotor.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator +(float a, Rotor3 b) => b + a;

    /// <summary>
    /// Adds a bivector to a rotor.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator +(Rotor3 a, Bivector3 b) => new(a.R, a.B + b);

    /// <summary>
    /// Adds a bivector to a rotor.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator +(Bivector3 a, Rotor3 b) => b + a;

    /// <summary>
    /// Adds two rotors.
    /// </summary>
    /// <param name="a">The first rotor.</param>
    /// <param name="b">The second rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor3 operator +(Rotor3 a, Rotor3 b) => new(a.R + b.R, a.B + b.B);
}
