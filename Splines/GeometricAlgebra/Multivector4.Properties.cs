using System.Numerics;

namespace Splines.GeometricAlgebra;

public partial struct Multivector4
{
    /// <summary>
    /// The scalar part of the multivector.
    /// </summary>
    public float R { get; set; }

    /// <summary>
    /// The vector part of the multivector.
    /// </summary>
    public Vector4 V { get; set; }

    /// <summary>
    /// The bivector part of the multivector.
    /// </summary>
    public Bivector4 B { get; set; }

    /// <summary>
    /// The trivector part of the multivector.
    /// </summary>
    public Trivector4 T { get; set; }

    /// <summary>
    /// The quadvector part of the multivector.
    /// </summary>
    public Quadvector4 Q { get; set; }

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
    /// Gets or sets the W component of the vector part.
    /// </summary>
    public float W
    {
        [Pure]
        get => V.W;
        set => V = V with { W = value };
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
    /// Gets or sets the XZ component of the bivector part.
    /// </summary>
    public float XZ
    {
        [Pure]
        get => B.XZ;
        set => B = B with { XZ = value };
    }

    /// <summary>
    /// Gets or sets the XW component of the bivector part.
    /// </summary>
    public float XW
    {
        [Pure]
        get => B.XW;
        set => B = B with { XW = value };
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
    /// Gets or sets the YW component of the bivector part.
    /// </summary>
    public float YW
    {
        [Pure]
        get => B.YW;
        set => B = B with { YW = value };
    }

    /// <summary>
    /// Gets or sets the ZW component of the bivector part.
    /// </summary>
    public float ZW
    {
        [Pure]
        get => B.ZW;
        set => B = B with { ZW = value };
    }

    /// <summary>
    /// Gets or sets the XYZ component of the trivector part.
    /// </summary>
    public float XYZ
    {
        [Pure]
        get => T.XYZ;
        set => T = T with { XYZ = value };
    }

    /// <summary>
    /// Gets or sets the XYW component of the trivector part.
    /// </summary>
    public float XYW
    {
        [Pure]
        get => T.XYW;
        set => T = T with { XYW = value };
    }

    /// <summary>
    /// Gets or sets the XZW component of the trivector part.
    /// </summary>
    public float XZW
    {
        [Pure]
        get => T.XZW;
        set => T = T with { XZW = value };
    }

    /// <summary>
    /// Gets or sets the YZW component of the trivector part.
    /// </summary>
    public float YZW
    {
        [Pure]
        get => T.YZW;
        set => T = T with { YZW = value };
    }

    /// <summary>
    /// Gets or sets the XYZW component of the quadvector part.
    /// </summary>
    public float XYZW
    {
        [Pure]
        get => Q.XYZW;
        set => Q = new(value);
    }
}
