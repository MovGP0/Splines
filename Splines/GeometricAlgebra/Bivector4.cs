using System.Numerics;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a bivector in 4D space, which is an antisymmetric second-order tensor.
/// </summary>
/// <remarks>
/// A bivector can be used to represent oriented areas in 4D space. This class provides various
/// operations for manipulating bivectors, such as addition, subtraction, multiplication, and
/// conversion to and from vectors.
/// </remarks>
/// <example>
/// <code>
/// Bivector4 bivector = new Bivector4(1.0f, 2.0f, 3.0f, 4.0f, 5.0f, 6.0f);
/// Vector4 normal = bivector.Normal;
/// float magnitude = bivector.Magnitude;
/// </code>
/// </example>
[Serializable]
public partial struct Bivector4
{
    /// <summary>
    /// Represents a bivector with all components set to zero.
    /// </summary>
    public static readonly Bivector4 Zero = new(0, 0, 0, 0, 0, 0);

    /// <summary>
    /// Gets or sets the XY component of the bivector.
    /// </summary>
    public float XY { get; set; }

    /// <summary>
    /// Gets or sets the XZ component of the bivector.
    /// </summary>
    public float XZ { get; set; }

    /// <summary>
    /// Gets or sets the XW component of the bivector.
    /// </summary>
    public float XW { get; set; }

    /// <summary>
    /// Gets or sets the YZ component of the bivector.
    /// </summary>
    public float YZ { get; set; }

    /// <summary>
    /// Gets or sets the YW component of the bivector.
    /// </summary>
    public float YW { get; set; }

    /// <summary>
    /// Gets or sets the ZW component of the bivector.
    /// </summary>
    public float ZW { get; set; }

    /// <summary>
    /// Gets the component of the bivector at the specified index.
    /// </summary>
    /// <param name="i">The index of the component (0 for XY, 1 for XZ, 2 for XW, 3 for YZ, 4 for YW, 5 for ZW).</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is not between 0 and 5.</exception>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 => XY,
                1 => XZ,
                2 => XW,
                3 => YZ,
                4 => YW,
                5 => ZW,
                _ => throw new IndexOutOfRangeException()
            };
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bivector4"/> struct with the specified components.
    /// </summary>
    /// <param name="xy">The XY component.</param>
    /// <param name="xz">The XZ component.</param>
    /// <param name="xw">The XW component.</param>
    /// <param name="yz">The YZ component.</param>
    /// <param name="yw">The YW component.</param>
    /// <param name="zw">The ZW component.</param>
    public Bivector4(float xy, float xz, float xw, float yz, float yw, float zw)
    {
        XY = xy;
        XZ = xz;
        XW = xw;
        YZ = yz;
        YW = yw;
        ZW = zw;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bivector4"/> struct from two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    public Bivector4(Vector4 a, Vector4 b)
    {
        Bivector4 bv = Mathfs.Wedge(a, b);
        XY = bv.XY;
        XZ = bv.XZ;
        XW = bv.XW;
        YZ = bv.YZ;
        YW = bv.YW;
        ZW = bv.ZW;
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
    public Bivector4 Normalized => new Bivector4(XY, XZ, XW, YZ, YW, ZW) / Magnitude;

    /// <summary>
    /// Gets the normal vector of the bivector.
    /// </summary>
    [Pure]
    public Vector4 Normal => Vector4.Normalize(HodgeDual);

    /// <summary>
    /// Gets the Hodge dual of the bivector.
    /// </summary>
    [Pure]
    public Vector4 HodgeDual => new(XY, XZ, XW, YZ); // 4D Hodge dual is more complex

    /// <summary>
    /// Gets the square of the magnitude of the bivector.
    /// </summary>
    [Pure]
    public float SqrMagnitude => XY * XY + XZ * XZ + XW * XW + YZ * YZ + YW * YW + ZW * ZW;

    /// <inheritdoc cref="Dot(Bivector4,Bivector4)"/>
    [Pure]
    public float Dot(Bivector4 b) => Dot(this, b);

    /// <inheritdoc cref="Wedge(Bivector4,Bivector4)"/>
    [Pure]
    public Bivector4 Wedge(Bivector4 b) => Wedge(this, b);

    /// <summary>The real part when multiplying two bivectors</summary>
    [Pure]
    public static float Dot(Bivector4 a, Bivector4 b) => -a.XY * b.XY - a.XZ * b.XZ - a.XW * b.XW - a.YZ * b.YZ - a.YW * b.YW - a.ZW * b.ZW;

    /// <summary>The bivector part when multiplying two bivectors</summary>
    [Pure]
    public static Bivector4 Wedge(Bivector4 a, Bivector4 b) =>
        new(
            xy: a.XY * b.XZ - a.XZ * b.XY,
            xz: a.XY * b.YZ - a.XY * b.YZ,
            xw: a.XY * b.YW - a.YW * b.XY,
            yz: a.XZ * b.YZ - a.YZ * b.XZ,
            yw: a.XZ * b.YW - a.YW * b.XZ,
            zw: a.XW * b.ZW - a.ZW * b.XW
        );

    /// <summary>Returns the normal of this bivector plane and its area</summary>
    [Pure]
    public (Vector4 normal, float area) GetNormalAndArea() => HodgeDual.GetDirectionAndMagnitude();
}
