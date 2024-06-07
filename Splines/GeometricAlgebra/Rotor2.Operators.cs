using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Rotor2
{
    /// <summary>
    /// Multiplies two rotors.
    /// </summary>
    /// <param name="a">The first rotor.</param>
    /// <param name="b">The second rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator *(Rotor2 a, Rotor2 b)
    {
        return new Rotor2(
            a.R * b.R - a.XY * b.XY,
            a.R * b.XY + a.XY * b.R
       );
    }

    /// <summary>
    /// Multiplies a rotor by a multivector.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector2 operator *(Rotor2 a, Multivector2 b)
    {
        return new Multivector2(
            a.R * b.R - a.XY * b.XY,
            a.R * b.V.X + a.XY * b.V.Y,
            a.R * b.V.Y - a.XY * b.V.X,
            a.XY * b.R
        );
    }

    /// <summary>
    /// Multiplies a multivector by a rotor.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector2 operator *(Multivector2 a, Rotor2 b)
    {
        return new Multivector2(
            a.R * b.R - a.XY * b.XY,
            a.V.X * b.R + a.V.Y * b.XY,
            a.V.Y * b.R - a.V.X * b.XY,
            a.XY * b.R
        );
    }

    /// <summary>
    /// Multiplies a rotor by a vector, resulting in a multivector.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector2 operator *(Rotor2 a, Vector2 b)
    {
        return new Multivector2(
            0,
            a.R * b.X + a.XY * b.Y,
            a.R * b.Y - a.XY * b.X,
            0
       );
    }

    /// <summary>
    /// Multiplies a vector by a rotor, resulting in a multivector.
    /// </summary>
    /// <param name="b">The vector.</param>
    /// <param name="a">The rotor.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector2 operator *(Vector2 b, Rotor2 a)
    {
        return new Multivector2(
            0,
            a.R * b.X - a.XY * b.Y,
            a.R * b.Y + a.XY * b.X,
            0
       );
    }

    /// <summary>
    /// Divides a rotor by a scalar.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator /(Rotor2 a, float b)
    {
        return new Rotor2(a.R / b, a.XY / b);
    }

    /// <summary>
    /// Adds a scalar to a rotor.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator +(Rotor2 a, float b) => new(a.R + b, a.B);

    /// <summary>
    /// Adds a scalar to a rotor.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator +(float a, Rotor2 b) => b + a;

    /// <summary>
    /// Adds a bivector to a rotor.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator +(Rotor2 a, Bivector2 b) => new(a.R, a.B + b);

    /// <summary>
    /// Adds a bivector to a rotor.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator +(Bivector2 a, Rotor2 b) => b + a;

    /// <summary>
    /// Adds two rotors.
    /// </summary>
    /// <param name="a">The first rotor.</param>
    /// <param name="b">The second rotor.</param>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public static Rotor2 operator +(Rotor2 a, Rotor2 b) => new(a.R + b.R, a.B + b.B);
}
