using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Quadvector4
{
    /// <summary>
    /// Adds two quadvectors.
    /// </summary>
    /// <param name="a">The first quadvector.</param>
    /// <param name="b">The second quadvector.</param>
    /// <returns>The resulting quadvector.</returns>
    [Pure]
    public static Quadvector4 operator +(Quadvector4 a, Quadvector4 b) => new(a.XYZW + b.XYZW);

    /// <summary>
    /// Multiplies a quadvector by a scalar.
    /// </summary>
    /// <param name="a">The quadvector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting quadvector.</returns>
    [Pure]
    public static Quadvector4 operator *(Quadvector4 a, float b) => new(a.XYZW * b);

    /// <summary>
    /// Multiplies a scalar by a quadvector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The quadvector.</param>
    /// <returns>The resulting quadvector.</returns>
    [Pure]
    public static Quadvector4 operator *(float a, Quadvector4 b) => b * a;

    /// <summary>
    /// Multiplies a quadvector by a vector, resulting in a trivector.
    /// </summary>
    /// <param name="a">The quadvector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector4 operator *(Quadvector4 a, Vector4 b) =>
        new(a.XYZW * b.X, a.XYZW * b.Y, a.XYZW * b.Z, a.XYZW * b.W);

    /// <summary>
    /// Multiplies a vector by a quadvector, resulting in a trivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The quadvector.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector4 operator *(Vector4 a, Quadvector4 b) => new(a.X * b.XYZW, a.Y * b.XYZW, a.Z * b.XYZW, a.W * b.XYZW);

    /// <summary>
    /// Multiplies a quadvector by a bivector, resulting in a vector.
    /// </summary>
    /// <param name="a">The quadvector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting vector.</returns>
    [Pure]
    public static Vector4 operator *(Quadvector4 a, Bivector4 b)
    {
        return new Vector4(
            -a.XYZW * b.YZ,
            -a.XYZW * b.XZ,
            -a.XYZW * b.XY,
            -a.XYZW * b.XW
        );
    }

    /// <summary>
    /// Multiplies a bivector by a quadvector, resulting in a vector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The quadvector.</param>
    /// <returns>The resulting vector.</returns>
    [Pure]
    public static Vector4 operator *(Bivector4 a, Quadvector4 b) => b * a;

    /// <summary>
    /// Multiplies a quadvector by a trivector, resulting in a scalar.
    /// </summary>
    /// <param name="a">The quadvector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float operator *(Quadvector4 a, Trivector4 b) =>
        -a.XYZW * b.XYZ - a.XYZW * b.XYW - a.XYZW * b.XZW - a.XYZW * b.YZW;

    /// <summary>
    /// Multiplies a trivector by a quadvector, resulting in a scalar.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The quadvector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float operator *(Trivector4 a, Quadvector4 b) => b * a;

    /// <summary>
    /// Computes the scalar product of two quadvectors.
    /// </summary>
    /// <param name="a">The first quadvector.</param>
    /// <param name="b">The second quadvector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float operator *(Quadvector4 a, Quadvector4 b) => -a.XYZW * b.XYZW;
}
