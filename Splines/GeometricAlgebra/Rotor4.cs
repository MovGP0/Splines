namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a rotor in 4D space, which is used for rotations.
/// The even subalgebra of 4D Vector-Grade Algebra (VGA), isomorphic to bi-quaternions.
/// </summary>
/// <remarks>
/// A rotor is a multivector that can be used to represent rotations in 4D space.
/// This class provides various operations for manipulating rotors, such as multiplication and rotation of vectors.
/// </remarks>
/// <example>
/// <code>
/// Rotor4 rotor = new Rotor4(1.0f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f);
/// Vector4 rotatedVector = rotor.Rotate(new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
/// </code>
/// </example>
public partial struct Rotor4
{
    /// <summary>
    /// The scalar part of the rotor.
    /// </summary>
    public float R { get; set; }

    /// <summary>
    /// The bivector part of the rotor.
    /// </summary>
    public Bivector4 B { get; set; }

    public Trivector4 T { get; set; }

    /// <summary>
    /// Gets or sets the XY component of the bivector part.
    /// </summary>
    public float XY
    {
        get => B.XY;
        set => B = B with { XY = value };
    }

    /// <summary>
    /// Gets or sets the XZ component of the bivector part.
    /// </summary>
    public float XZ
    {
        get => B.XZ;
        set => B = B with { XZ = value };
    }

    /// <summary>
    /// Gets or sets the XW component of the bivector part.
    /// </summary>
    public float XW
    {
        get => B.XW;
        set => B = B with { XW = value };
    }

    /// <summary>
    /// Gets or sets the YZ component of the bivector part.
    /// </summary>
    public float YZ
    {
        get => B.YZ;
        set => B = B with { YZ = value };
    }

    /// <summary>
    /// Gets or sets the YW component of the bivector part.
    /// </summary>
    public float YW
    {
        get => B.YW;
        set => B = B with { YW = value };
    }

    /// <summary>
    /// Gets or sets the ZW component of the bivector part.
    /// </summary>
    public float ZW
    {
        get => B.ZW;
        set => B = B with { ZW = value };
    }

    /// <summary>
    /// Gets or sets the XYZ component of the trivector part.
    /// </summary>
    public float XYZ
    {
        get => T.XYZ;
        set => T = T with { XYZ = value };
    }

    /// <summary>
    /// Gets or sets the XYW component of the trivector part.
    /// </summary>
    public float XYW
    {
        get => T.XYW;
        set => T = T with { XYW = value };
    }

    /// <summary>
    /// Gets or sets the XZW component of the trivector part.
    /// </summary>
    public float XZW
    {
        get => T.XZW;
        set => T = T with { XZW = value };
    }

    /// <summary>
    /// Gets or sets the YZW component of the trivector part.
    /// </summary>
    public float YZW
    {
        get => T.YZW;
        set => T = T with { YZW = value };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotor4"/> struct with the specified scalar and bivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="xy">The XY component of the bivector part.</param>
    /// <param name="xz">The XZ component of the bivector part.</param>
    /// <param name="xw">The XW component of the bivector part.</param>
    /// <param name="yz">The YZ component of the bivector part.</param>
    /// <param name="yw">The YW component of the bivector part.</param>
    /// <param name="zw">The ZW component of the bivector part.</param>
    /// <param name="xyz">The XYZ component of the trivector part.</param>
    /// <param name="xyw">The XYW component of the trivector part.</param>
    /// <param name="xzw">The XZW component of the trivector part.</param>
    /// <param name="yzw">The YZW component of the trivector part.</param>
    public Rotor4(
        float r,
        float xy,
        float xz,
        float xw,
        float yz,
        float yw,
        float zw,
        float xyz,
        float xyw,
        float xzw,
        float yzw)
        : this(r, new Bivector4(xy, xz, xw, yz, yw, zw), new Trivector4(xyz, xyw, xzw, yzw))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rotor4"/> struct with the specified scalar, bivector, and trivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="b">The bivector part.</param>
    /// <param name="t">The trivector part.</param>
    public Rotor4(float r, Bivector4 b, Trivector4 t)
    {
        R = r;
        B = b;
        T = t;
    }
}
