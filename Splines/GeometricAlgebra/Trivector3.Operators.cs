using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Trivector3
{
    /// <summary>
    /// Adds two trivectors.
    /// </summary>
    /// <param name="a">The first trivector.</param>
    /// <param name="b">The second trivector.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector3 operator +(Trivector3 a, Trivector3 b) => new(a.XYZ + b.XYZ);

    /// <summary>
    /// Multiplies a trivector by a scalar.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector3 operator *(Trivector3 a, float b) => new(a.XYZ * b);

    /// <summary>
    /// Multiplies a scalar by a trivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting trivector.</returns>
    [Pure]
    public static Trivector3 operator *(float a, Trivector3 b) => b * a;

    /// <summary>
    /// Multiplies a trivector by a vector, resulting in a bivector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector3 operator *(Trivector3 a, Vector3 b) => new(a.XYZ * b.X, a.XYZ * b.Y, a.XYZ * b.Z);

    /// <summary>
    /// Multiplies a vector by a trivector, resulting in a bivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector3 operator *(Vector3 a, Trivector3 b) => new(a.X * b.XYZ, a.Y * b.XYZ, a.Z * b.XYZ);

    /// <summary>
    /// Multiplies a bivector by a trivector, resulting in a vector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting vector.</returns>
    [Pure]
    public static Vector3 operator *(Bivector3 a, Trivector3 b) => new(-a.YZ * b.XYZ, -a.ZX * b.XYZ, -a.XY * b.XYZ);

    /// <summary>
    /// Multiplies a trivector by a bivector, resulting in a vector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting vector.</returns>
    [Pure]
    public static Vector3 operator *(Trivector3 a, Bivector3 b) => new(-a.XYZ * b.YZ, -a.XYZ * b.ZX, -a.XYZ * b.XY);

    /// <summary>
    /// Computes the scalar product of two trivectors.
    /// </summary>
    /// <param name="a">The first trivector.</param>
    /// <param name="b">The second trivector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float operator *(Trivector3 a, Trivector3 b) => -a.XYZ * b.XYZ;
}
