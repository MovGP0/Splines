using System.Numerics;
using Splines.Unity;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a rotor in 3D space, which is used for rotations.
/// The even subalgebra of 3D Vector-Grade Algebra (VGA), isomorphic to Quaternions.
/// </summary>
/// <remarks>
/// A rotor is a multivector that can be used to represent rotations in 3D space.
/// This class provides various operations for manipulating rotors, such as multiplication and rotation of vectors.
/// </remarks>
/// <example>
/// <code>
/// Rotor3 rotor = new Rotor3(1.0f, 0.5f, 0.5f, 0.5f);
/// Vector3 rotatedVector = rotor.Rotate(new Vector3(1.0f, 0.0f, 0.0f));
/// </code>
/// </example>
public partial struct Rotor3
{
    /// <summary>
    /// The scalar part of the rotor.
    /// </summary>
    public float R
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The bivector part of the rotor.
    /// </summary>
    public Bivector3 B
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the YZ component of the bivector part.
    /// </summary>
    public float YZ
    {
        [Pure]
        get => B.YZ;
        set => B = B with { YZ = value };
    }

    /// <summary>
    /// Gets or sets the ZX component of the bivector part.
    /// </summary>
    public float ZX
    {
        [Pure]
        get => B.ZX;
        set => B = B with { ZX = value };
    }

    /// <summary>
    /// Gets or sets the XY component of the bivector part.
    /// </summary>
    public float XY
    {
        [Pure]
        get => B.XY;
        set => B = B with { XY = value };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotor3"/> struct with the specified scalar and bivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="yz">The YZ component of the bivector part.</param>
    /// <param name="zx">The ZX component of the bivector part.</param>
    /// <param name="xy">The XY component of the bivector part.</param>
    public Rotor3(float r, float yz, float zx, float xy) : this(r, new Bivector3(yz, zx, xy)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotor3"/> struct with the specified scalar and bivector.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="b">The bivector part.</param>
    public Rotor3(float r, Bivector3 b)
    {
        R = r;
        B = b;
    }

    /// <summary>
    /// Creates a rotation representing twice the angle from <paramref name="a"/> to <paramref name="b"/>.
    /// This is equivalent to multiplying the two vectors a*b.
    /// Note: Assumes both input vectors are normalized.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    [Pure]
    public static Rotor3 FromToRotationDouble(Vector3 a, Vector3 b) => new(Vector3.Dot(a, b), Mathfs.Wedge(a, b));

    /// <summary>
    /// Creates a rotation from <paramref name="a"/> to <paramref name="b"/>. Note: Assumes both input vectors are normalized.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    [Pure]
    public static Rotor3 FromToRotation(Vector3 a, Vector3 b) => new Rotor3(Vector3.Dot(a, b) + 1, Mathfs.Wedge(a, b)).Normalized();

    /// <summary>
    /// Constructs a unit rotor representing a rotation.
    /// </summary>
    /// <param name="angle">The angle of rotation in radians.</param>
    /// <param name="axis">The axis of rotation.</param>
    public Rotor3(float angle, Vector3 axis)
    {
        Bivector3 axisDual = new Bivector3(axis.X, axis.Y, axis.Z);
        float halfAngle = angle / 2;
        R = Mathf.Cos(halfAngle);
        B = axisDual * Mathf.Sin(halfAngle);
    }

    /// <summary>
    /// Gets the magnitude of the rotor.
    /// </summary>
    [Pure]
    public float Magnitude => Mathf.Sqrt(SqrMagnitude);

    /// <summary>
    /// Gets the squared magnitude of the rotor.
    /// </summary>
    [Pure]
    public float SqrMagnitude => R * R + B.SqrMagnitude;

    /// <summary>
    /// Returns a normalized version of the rotor.
    /// </summary>
    [Pure]
    public Rotor3 Normalized() => this / Magnitude;

    /// <summary>
    /// Converts the rotor to a quaternion.
    /// </summary>
    /// <returns>The equivalent quaternion.</returns>
    [Pure]
    public Quaternion ToQuaternion() => new(YZ, ZX, XY, R);

    /// <summary>
    /// Negates the bivector, which is equivalent to reversing the rotation, if this is normalized and interpreted as a rotation.
    /// </summary>
    [Pure]
    public Rotor3 Conjugate => new(R, -B);

    /// <summary>Sandwich product, equivalent to ⭐(R* ⭐v R) (where R* is the conjugate of R and ⭐v is the hodge dual of v).
    /// Commonly used to rotate vectors with unit rotors</summary>
    /// <param name="v">The vector to multiply (or rotate)</param>
    /// <returns>The rotated vector.</returns>
    public Vector3 Rotate(Vector3 v)
    {
        // hodge variant
        Bivector3 vHodge = new(v.X, v.Y, v.Z);
        return (Conjugate * vHodge * this).B.HodgeDual;
    }
}
