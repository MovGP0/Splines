using System.Numerics;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a multivector in 3D space, consisting of scalar, vector, bivector, and trivector components.
/// </summary>
/// <remarks>
/// A multivector is a generalization of scalars, vectors, and higher-order tensors, providing a unified framework
/// for geometric algebra.
/// </remarks>
/// <example>
/// <code>
/// Multivector3 mv = new Multivector3(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f);
/// Vector3 vectorPart = mv.V;
/// float scalarPart = mv.R;
/// </code>
/// </example>
public partial struct Multivector3
{
    /// <summary>
    /// The scalar part of the multivector.
    /// </summary>
    public float R
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The vector part of the multivector.
    /// </summary>
    public Vector3 V
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The bivector part of the multivector.
    /// </summary>
    public Bivector3 B
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The trivector part of the multivector.
    /// </summary>
    public Trivector3 T
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the X component of the vector part.
    /// </summary>
    public float X
    {
        [Pure]
        get => V.X;
        set => V = V with { X = value };
    }

    /// <summary>
    /// Gets or sets the Y component of the vector part.
    /// </summary>
    public float Y
    {
        [Pure]
        get => V.Y;
        set => V = V with { Y = value };
    }

    /// <summary>
    /// Gets or sets the Z component of the vector part.
    /// </summary>
    public float Z
    {
        [Pure]
        get => V.Z;
        set => V = V with { Z = value };
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
    /// Gets or sets the XYZ component of the trivector part.
    /// </summary>
    public float XYZ
    {
        [Pure]
        get => T.XYZ;
        set => T = new(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector3"/> struct with specified scalar, vector, bivector, and trivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="x">The X component of the vector part.</param>
    /// <param name="y">The Y component of the vector part.</param>
    /// <param name="z">The Z component of the vector part.</param>
    /// <param name="yz">The YZ component of the bivector part.</param>
    /// <param name="zx">The ZX component of the bivector part.</param>
    /// <param name="xy">The XY component of the bivector part.</param>
    /// <param name="xyz">The XYZ component of the trivector part.</param>
    public Multivector3(float r, float x, float y, float z, float yz, float zx, float xy, float xyz)
        : this(
            r,
            new Vector3(x, y, z),
            new Bivector3(yz, zx, xy),
            new Trivector3(xyz))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector3"/> struct with specified scalar and vector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="v">The vector part.</param>
    public Multivector3(float r, Vector3 v) : this(r, v, Bivector3.Zero, Trivector3.Zero)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector3"/> struct with specified scalar, vector, bivector, and trivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="v">The vector part.</param>
    /// <param name="b">The bivector part.</param>
    /// <param name="t">The trivector part.</param>
    public Multivector3(float r, Vector3 v, Bivector3 b, Trivector3 t)
    {
        R = r;
        V = v;
        B = b;
        T = t;
    }

    /// <summary>
    /// Multiplies two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator *(Multivector3 a, Multivector3 b)
    {
        // 64 multiplications, 56 add/sub
        // R:    a_r*b_r        +a_x*b_x    +a_y*b_y    +a_z*b_z    +a_yz*b_yz    +a_zx*b_zx    +a_xy*b_xy    +a_xyz*b_xyz
        // X:    a_r*b_x        +a_x*b_r    -a_y*b_xy    +a_z*b_zx    -a_yz*b_xyz    -a_zx*b_z    +a_xy*b_y    -a_xyz*b_yz
        // Y:    a_r*b_y        +a_x*b_xy    +a_y*b_r    -a_z*b_yz    +a_yz*b_z    -a_zx*b_xyz    -a_xy*b_x    -a_xyz*b_zx
        // Z:    a_r*b_z        -a_x*b_zx    +a_y*b_yz    +a_z*b_r    -a_yz*b_y    +a_zx*b_x    -a_xy*b_xyz    -a_xyz*b_xy
        // YZ:    a_r*b_yz    +a_x*b_xyz    +a_y*b_z    -a_z*b_y    +a_yz*b_r    -a_zx*b_xy    +a_xy*b_zx    +a_xyz*b_x
        // ZX:    a_r*b_zx    -a_x*b_z    +a_y*b_xyz    +a_z*b_x    +a_yz*b_xy    +a_zx*b_r    -a_xy*b_yz    +a_xyz*b_y
        // XY:    a_r*b_xy    +a_x*b_y    -a_y*b_x    +a_z*b_xyz    -a_yz*b_zx    +a_zx*b_yz    +a_xy*b_r    +a_xyz*b_z
        // XYZ:    a_r*b_xyz    +a_x*byz    +a_y*b_zx    +a_z*xy        +a_yz*b_x    +a_zx*b_y    +a_xy*b_z    +a_xyz*b_r
        return new(
            a.R * b.R + a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.YZ * b.YZ + a.ZX * b.ZX + a.XY * b.XY + a.XYZ * b.XYZ,
            a.R * b.X + a.X * b.R - a.Y * b.XY + a.Z * b.ZX - a.YZ * b.XYZ - a.ZX * b.Z + a.XY * b.Y - a.XYZ * b.YZ,
            a.R * b.Y + a.X * b.XY + a.Y * b.R - a.Z * b.YZ + a.YZ * b.Z - a.ZX * b.XYZ - a.XY * b.X - a.XYZ * b.ZX,
            a.R * b.Z - a.X * b.ZX + a.Y * b.YZ + a.Z * b.R - a.YZ * b.Y + a.ZX * b.X - a.XY * b.XYZ - a.XYZ * b.XY,
            a.R * b.YZ + a.X * b.XYZ + a.Y * b.Z - a.Z * b.Y + a.YZ * b.R - a.ZX * b.XY + a.XY * b.ZX + a.XYZ * b.X,
            a.R * b.ZX - a.X * b.Z + a.Y * b.XYZ + a.Z * b.X + a.YZ * b.XY + a.ZX * b.R - a.XY * b.YZ + a.XYZ * b.Y,
            a.R * b.XY + a.X * b.Y - a.Y * b.X + a.Z * b.XYZ - a.YZ * b.ZX + a.ZX * b.YZ + a.XY * b.R + a.XYZ * b.Z,
            a.R * b.XYZ + a.X * b.YZ + a.Y * b.ZX + a.Z * b.XY + a.YZ * b.X + a.ZX * b.Y + a.XY * b.Z + a.XYZ * b.R
       );
    }

    /// <summary>
    /// Multiplies a multivector by a rotor.
    /// </summary>
    /// <param name="m">The multivector.</param>
    /// <param name="r">The rotor.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator *(Multivector3 m, Rotor3 r)
    {
        return new(
            m.R * r.R + m.YZ * r.YZ + m.ZX * r.ZX + m.XY * r.XY,
            m.X * r.R - m.Y * r.XY + m.Z * r.ZX - m.XYZ * r.YZ,
            +m.X * r.XY + m.Y * r.R - m.Z * r.YZ - m.XYZ * r.ZX,
            -m.X * r.ZX + m.Y * r.YZ + m.Z * r.R - m.XYZ * r.XY,
            m.R * r.YZ + m.YZ * r.R - m.ZX * r.XY + m.XY * r.ZX,
            m.R * r.ZX + m.YZ * r.XY + m.ZX * r.R - m.XY * r.YZ,
            m.R * r.XY - m.YZ * r.ZX + m.ZX * r.YZ + m.XY * r.R,
            m.X * r.YZ + m.Y * r.ZX + m.Z * r.XY + m.XYZ * r.R
       );
    }

    /// <summary>
    /// Computes the wedge product of two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector3 Wedge(Multivector3 a, Multivector3 b) =>
        new(
            a.R * b.YZ + a.X * b.XYZ + a.Y * b.Z - a.Z * b.Y + a.YZ * b.R - a.ZX * b.XY + a.XY * b.ZX + a.XYZ * b.X,
            a.R * b.ZX - a.X * b.Z + a.Y * b.XYZ + a.Z * b.X + a.YZ * b.XY + a.ZX * b.R - a.XY * b.YZ + a.XYZ * b.Y,
            a.R * b.XY + a.X * b.Y - a.Y * b.X + a.Z * b.XYZ - a.YZ * b.ZX + a.ZX * b.YZ + a.XY * b.R + a.XYZ * b.Z);

    /// <summary>
    /// Computes the dot product of two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting scalar.</returns>
    [Pure]
    public static float Dot(Multivector3 a, Multivector3 b) => a.R * b.R + a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.YZ * b.YZ + a.ZX * b.ZX + a.XY * b.XY + a.XYZ * b.XYZ;

    /// <summary>
    /// Adds a scalar to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Multivector3 a, float b) => new(a.R + b, a.V, a.B, a.T);

    /// <summary>
    /// Adds a scalar to a multivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(float a, Multivector3 b) => b + a;

    /// <summary>
    /// Adds a vector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Multivector3 a, Vector3 b) => new(a.R, a.V + b, a.B, a.T);

    /// <summary>
    /// Adds a vector to a multivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Vector3 a, Multivector3 b) => b + a;

    /// <summary>
    /// Adds a bivector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Multivector3 a, Bivector3 b) => new(a.R, a.V, a.B + b, a.T);

    /// <summary>
    /// Adds a bivector to a multivector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Bivector3 a, Multivector3 b) => b + a;

    /// <summary>
    /// Adds a trivector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The trivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Multivector3 a, Trivector3 b) => new(a.R, a.V, a.B, a.T + b);

    /// <summary>
    /// Adds a trivector to a multivector.
    /// </summary>
    /// <param name="a">The trivector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Trivector3 a, Multivector3 b) => b + a;

    /// <summary>
    /// Adds two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Multivector3 a, Multivector3 b) => new(a.R + b.R, a.V + b.V, a.B + b.B, a.T + b.T);

    /// <summary>
    /// Adds a rotor to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The rotor.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Multivector3 a, Rotor3 b) => new(a.R + b.R, a.V, a.B + b.B, a.T);

    /// <summary>
    /// Adds a rotor to a multivector.
    /// </summary>
    /// <param name="a">The rotor.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    [Pure]
    public static Multivector3 operator +(Rotor3 a, Multivector3 b) => b + a;
}
