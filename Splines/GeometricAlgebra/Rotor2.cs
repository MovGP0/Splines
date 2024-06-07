using System.Numerics;
using Splines.Unity;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a rotor in 2D space, which is used for rotations.
/// </summary>
/// <remarks>
/// A rotor is a multivector that can be used to represent rotations in 2D space.
/// This class provides various operations for manipulating rotors, such as multiplication and rotation of vectors.
/// </remarks>
/// <example>
/// <code>
/// Rotor2 rotor = new Rotor2(1.0f, 0.5f);
/// Vector2 rotatedVector = rotor.Rotate(new Vector2(1.0f, 0.0f));
/// </code>
/// </example>
public partial struct Rotor2
{
    /// <summary>
    /// The scalar part of the rotor.
    /// </summary>
    public float R { get; set; }

    /// <summary>
    /// The bivector part of the rotor.
    /// </summary>
    public Bivector2 B { get; set; }

    /// <summary>
    /// Gets or sets the XY component of the bivector part.
    /// </summary>
    public float XY
    {
        get => B.XY;
        set => B = new Bivector2(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotor2"/> struct with the specified scalar and bivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="xy">The XY component of the bivector part.</param>
    public Rotor2(float r, float xy) : this(r, new Bivector2(xy)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotor2"/> struct with the specified scalar and bivector.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="b">The bivector part.</param>
    public Rotor2(float r, Bivector2 b)
    {
        R = r;
        B = b;
    }

    /// <summary>
    /// Constructs a unit rotor representing a rotation.
    /// </summary>
    /// <param name="angle">The angle of rotation in radians.</param>
    public Rotor2(float angle)
    {
        float halfAngle = angle / 2;
        R = Mathf.Cos(halfAngle);
        B = new Bivector2(Mathf.Sin(halfAngle));
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
    public Rotor2 Normalized() => this / Magnitude;

    /// <summary>
    /// Negates the bivector, which is equivalent to reversing the rotation, if this is normalized and interpreted as a rotation.
    /// </summary>
    [Pure]
    public Rotor2 Conjugate => new Rotor2(R, -B);

    /// <summary>
    /// Sandwich product, used to rotate vectors with unit rotors.
    /// </summary>
    /// <param name="v">The vector to multiply (or rotate).</param>
    /// <returns>The rotated vector.</returns>
    [Pure]
    public Vector2 Rotate(Vector2 v)
    {
        // Rotor2 form: cos(theta/2) + sin(theta/2) * e1e2
        // Rotation: v' = R * v * R^(-1)
        Multivector2 vMultivector = new Multivector2(0, v);
        Multivector2 result = this * vMultivector * Conjugate;
        return new Vector2(result.V.X, result.V.Y);
    }
}
