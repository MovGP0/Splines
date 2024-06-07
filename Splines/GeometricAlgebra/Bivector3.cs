using System.Numerics;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a bivector in 3D space, which is an antisymmetric second-order tensor.
/// </summary>
/// <remarks>
/// A bivector can be used to represent oriented areas in 3D space. This class provides various
/// operations for manipulating bivectors, such as addition, subtraction, multiplication, and
/// conversion to and from vectors.
/// </remarks>
/// <example>
/// <code>
/// Bivector3 bivector = new Bivector3(1.0f, 2.0f, 3.0f);
/// Vector3 normal = bivector.Normal;
/// float magnitude = bivector.Magnitude;
/// </code>
/// </example>
[Serializable]
public partial struct Bivector3
{
    /// <summary>
    /// Represents a bivector with all components set to zero.
    /// </summary>
    public static readonly Bivector3 Zero = new(0, 0, 0);

    /// <summary>
    /// Gets or sets the YZ component of the bivector.
    /// </summary>
    public float YZ
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the ZX component of the bivector.
    /// </summary>
    public float ZX
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the XY component of the bivector.
    /// </summary>
    public float XY
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets the component of the bivector at the specified index.
    /// </summary>
    /// <param name="i">The index of the component (0 for YZ, 1 for ZX, 2 for XY).</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is not 0, 1, or 2.</exception>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 => YZ,
                1 => ZX,
                2 => XY,
                _ => throw new IndexOutOfRangeException()
            };
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bivector3"/> struct with the specified components.
    /// </summary>
    /// <param name="yz">The YZ component.</param>
    /// <param name="zx">The ZX component.</param>
    /// <param name="xy">The XY component.</param>
    public Bivector3(float yz, float zx, float xy)
    {
        YZ = yz;
        ZX = zx;
        XY = xy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bivector3"/> struct from two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    public Bivector3(Vector3 a, Vector3 b)
    {
        Bivector3 bv = Mathfs.Wedge(a, b);
        YZ = bv.YZ;
        ZX = bv.ZX;
        XY = bv.XY;
    }

    /// <summary>
    /// Gets the magnitude of the bivector.
    /// </summary>
    [Pure]
    public float Magnitude => Mathf.Sqrt(SqrMagnitude);

    /// <summary>
    /// Gets a normalized version of the bivector.
    /// </summary>
    [Pure]
    public Bivector3 Normalized => new Bivector3(YZ, ZX, XY) / Magnitude;

    /// <summary>
    /// Gets the normal vector of the bivector.
    /// </summary>
    [Pure]
    public Vector3 Normal => Vector3.Normalize(HodgeDual);

    /// <summary>
    /// Gets the Hodge dual of the bivector.
    /// </summary>
    [Pure]
    public Vector3 HodgeDual => new(YZ, ZX, XY);

    /// <summary>
    /// Gets the square of the magnitude of the bivector.
    /// </summary>
    [Pure]
    public float SqrMagnitude => YZ * YZ + ZX * ZX + XY * XY;

    /// <inheritdoc cref="Dot(Bivector3,Bivector3)"/>
    [Pure]
    public float Dot(Bivector3 b) => Dot(this, b);

    /// <inheritdoc cref="Wedge(Bivector3,Bivector3)"/>
    [Pure]
    public Bivector3 Wedge(Bivector3 b) => Wedge(this, b);

    /// <summary>The real part when multiplying two bivectors</summary>
    [Pure]
    public static float Dot(Bivector3 a, Bivector3 b) => -a.YZ * b.YZ - a.ZX * b.ZX - a.XY * b.XY;

    /// <summary>The bivector part when multiplying two bivectors</summary>
    [Pure]
    public static Bivector3 Wedge(Bivector3 a, Bivector3 b) =>
        new(
            yz: a.XY * b.ZX - a.ZX * b.XY,
            zx: a.YZ * b.XY - a.XY * b.YZ,
            xy: a.ZX * b.YZ - a.YZ * b.ZX);

    /// <summary>Returns the normal of this bivector plane and its area</summary>
    [Pure]
    public (Vector3 normal, float area) GetNormalAndArea() => HodgeDual.GetDirectionAndMagnitude();

    /// <summary>
    /// Negates the bivector.
    /// </summary>
    /// <param name="b">The bivector to negate.</param>
    /// <returns>The negated bivector.</returns>
    [Pure]
    public static Bivector3 operator -(Bivector3 b) => new(-b.YZ, -b.ZX, -b.XY);

    /// <summary>
    /// Multiplies a bivector by a scalar.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The scaled bivector.</returns>
    [Pure]
    public static Bivector3 operator *(float a, Bivector3 b) => b * a;

    /// <summary>
    /// Multiplies a bivector by a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The scaled bivector.</returns>
    [Pure]
    public static Bivector3 operator *(Bivector3 a, float b) => new(a.YZ * b, a.ZX * b, a.XY * b);

    /// <summary>
    /// Multiplies two bivectors.
    /// </summary>
    /// <param name="a">The first bivector.</param>
    /// <param name="b">The second bivector.</param>
    /// <returns>The rotor resulting from the multiplication.</returns>
    [Pure]
    public static Rotor3 operator *(Bivector3 a, Bivector3 b) =>
        new(
            r: Dot(a, b),
            b: Wedge(a, b)
       );

    /// <summary>
    /// Squares the bivector.
    /// </summary>
    /// <returns>The resulting rotor.</returns>
    [Pure]
    public Rotor3 Square() =>
        new(
            -YZ * YZ - ZX * ZX - XY * XY,
            yz: XY * ZX - ZX * XY,
            zx: YZ * XY - XY * YZ,
            xy: ZX * YZ - YZ * ZX
       );

    /// <summary>
    /// Multiplies a bivector by a vector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator *(Bivector3 a, Vector3 b)
    {
        return new(
            0, // real
            a.XY * b.Y - a.ZX * b.Z, // vector
            a.YZ * b.Z - a.XY * b.X,
            a.ZX * b.X - a.YZ * b.Y,
            0, 0, 0, // bivector
            a.YZ * b.X + a.ZX * b.Y + a.XY * b.Z // trivector
       );
    }

    /// <summary>
    /// Multiplies a vector by a bivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator *(Vector3 a, Bivector3 b)
    {
        return new(
            0, // real
            a.Z * b.ZX - a.Y * b.XY, // vector
            a.X * b.XY - a.Z * b.YZ,
            a.Y * b.YZ - a.X * b.ZX,
            0, 0, 0, // bivector
            a.X * b.YZ + a.Y * b.ZX + a.Z * b.XY // trivector
       );
    }

    /// <summary>
    /// Divides a bivector by a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector3 operator /(Bivector3 a, float b) => new(a.YZ / b, a.ZX / b, a.XY / b);

    /// <summary>
    /// Adds two bivectors.
    /// </summary>
    /// <param name="a">The first bivector.</param>
    /// <param name="b">The second bivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector3 operator +(Bivector3 a, Bivector3 b) => new(a.YZ * b.YZ, a.ZX * b.ZX, a.XY * b.XY);

    /// <summary>
    /// Adds a bivector and a trivector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Bivector3 a, Trivector3 b) => new(0, Vector3.Zero, a, b);

    /// <summary>
    /// Adds a trivector and a bivector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Trivector3 a, Bivector3 b) => new(0, Vector3.Zero, b, a);

    /// <summary>
    /// Explicit conversion from <see cref="Bivector3"/> to <see cref="Vector3"/>.
    /// </summary>
    /// <param name="bv">The bivector.</param>
    [Pure]
    public static explicit operator Vector3(Bivector3 bv) => new(bv.YZ, bv.ZX, bv.XY);

    /// <summary>
    /// Explicit conversion from <see cref="Vector3"/> to <see cref="Bivector3"/>.
    /// </summary>
    /// <param name="v">The vector.</param>
    [Pure]
    public static explicit operator Bivector3(Vector3 v) => new(v.X, v.Y, v.Z);
}
