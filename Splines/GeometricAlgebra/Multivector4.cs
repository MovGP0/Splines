using System.Numerics;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a multivector in 4D space, consisting of scalar, vector, bivector, trivector, and quadvector components.
/// </summary>
/// <remarks>
/// A multivector is a generalization of scalars, vectors, and higher-order tensors, providing a unified framework
/// for geometric algebra.
/// </remarks>
/// <example>
/// <code>
/// Multivector4 mv = new Multivector4(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f, 7.0f, 8.0f, 9.0f, 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f);
/// Vector4 vectorPart = mv.V;
/// float scalarPart = mv.R;
/// </code>
/// </example>
public partial struct Multivector4
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector4"/> struct with specified scalar, vector, bivector, trivector, and quadvector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="x">The X component of the vector part.</param>
    /// <param name="y">The Y component of the vector part.</param>
    /// <param name="z">The Z component of the vector part.</param>
    /// <param name="w">The W component of the vector part.</param>
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
    /// <param name="xyzw">The XYZW component of the quadvector part.</param>
    public Multivector4(float r, float x, float y, float z, float w, float xy, float xz, float xw, float yz, float yw, float zw, float xyz, float xyw, float xzw, float yzw, float xyzw)
        : this(
            r,
            new Vector4(x, y, z, w),
            new Bivector4(xy, xz, xw, yz, yw, zw),
            new Trivector4(xyz, xyw, xzw, yzw),
            new Quadvector4(xyzw))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector4"/> struct with specified scalar and vector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="v">The vector part.</param>
    public Multivector4(float r, Vector4 v) : this(r, v, Bivector4.Zero, Trivector4.Zero, Quadvector4.Zero)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector4"/> struct with specified scalar, vector, bivector, trivector, and quadvector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="v">The vector part.</param>
    /// <param name="b">The bivector part.</param>
    /// <param name="t">The trivector part.</param>
    /// <param name="q">The quadvector part.</param>
    public Multivector4(float r, Vector4 v, Bivector4 b, Trivector4 t, Quadvector4 q)
    {
        R = r;
        V = v;
        B = b;
        T = t;
        Q = q;
    }
}
