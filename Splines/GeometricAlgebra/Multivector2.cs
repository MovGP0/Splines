using System.Numerics;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a multivector in 2D space, consisting of scalar, vector, and bivector components.
/// </summary>
/// <remarks>
/// A multivector is a generalization of scalars, vectors, and higher-order tensors, providing a unified framework
/// for geometric algebra.
/// </remarks>
/// <example>
/// <code>
/// Multivector2 mv = new Multivector2(1.0f, 2.0f, 3.0f, 4.0f);
/// Vector2 vectorPart = mv.V;
/// float scalarPart = mv.R;
/// </code>
/// </example>
public partial struct Multivector2
{
    /// <summary>
    /// The scalar part of the multivector.
    /// </summary>
    public float R { get; set; }

    /// <summary>
    /// The vector part of the multivector.
    /// </summary>
    public Vector2 V { get; set; }

    /// <summary>
    /// The bivector part of the multivector.
    /// </summary>
    public Bivector2 B { get; set; }

    /// <summary>
    /// Gets or sets the X component of the vector part.
    /// </summary>
    public float X
    {
        get => V.X;
        set => V = new Vector2(value, V.Y);
    }

    /// <summary>
    /// Gets or sets the Y component of the vector part.
    /// </summary>
    public float Y
    {
        get => V.Y;
        set => V = new Vector2(V.X, value);
    }

    /// <summary>
    /// Gets or sets the XY component of the bivector part.
    /// </summary>
    public float XY
    {
        get => B.XY;
        set => B = new Bivector2(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector2"/> struct with specified scalar, vector, and bivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="x">The X component of the vector part.</param>
    /// <param name="y">The Y component of the vector part.</param>
    /// <param name="xy">The XY component of the bivector part.</param>
    public Multivector2(float r, float x, float y, float xy)
        : this(r, new Vector2(x, y), new Bivector2(xy))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector2"/> struct with specified scalar and vector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="v">The vector part.</param>
    public Multivector2(float r, Vector2 v) : this(r, v, Bivector2.Zero)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Multivector2"/> struct with specified scalar, vector, and bivector components.
    /// </summary>
    /// <param name="r">The scalar part.</param>
    /// <param name="v">The vector part.</param>
    /// <param name="b">The bivector part.</param>
    public Multivector2(float r, Vector2 v, Bivector2 b)
    {
        R = r;
        V = v;
        B = b;
    }

    /// <summary>
    /// Adds two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(Multivector2 a, Multivector2 b) => new(a.R + b.R, a.V + b.V, a.B + b.B);

    /// <summary>
    /// Adds a scalar to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(Multivector2 a, float b) => new(a.R + b, a.V, a.B);

    /// <summary>
    /// Adds a scalar to a multivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(float a, Multivector2 b) => b + a;

    /// <summary>
    /// Adds a vector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The vector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(Multivector2 a, Vector2 b) => new(a.R, a.V + b, a.B);

    /// <summary>
    /// Adds a vector to a multivector.
    /// </summary>
    /// <param name="a">The vector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(Vector2 a, Multivector2 b) => b + a;

    /// <summary>
    /// Adds a bivector to a multivector.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(Multivector2 a, Bivector2 b) => new(a.R, a.V, a.B + b);

    /// <summary>
    /// Adds a bivector to a multivector.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator +(Bivector2 a, Multivector2 b) => b + a;

    /// <summary>
    /// Multiplies two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator *(Multivector2 a, Multivector2 b)
    {
        return new(
            a.R * b.R + a.V.X * b.V.X + a.V.Y * b.V.Y + a.B.XY * b.B.XY,
            a.R * b.V + b.R * a.V + a.B.XY * new Vector2(-b.V.Y, b.V.X),
            a.R * b.B + b.R * a.B + new Bivector2(Vector2.Dot(a.V, new Vector2(b.B.XY, -b.B.XY)))
        );
    }

    /// <summary>
    /// Multiplies a multivector by a scalar.
    /// </summary>
    /// <param name="a">The multivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator *(Multivector2 a, float b) => new(a.R * b, a.V * b, a.B * b);

    /// <summary>
    /// Multiplies a scalar by a multivector.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The multivector.</param>
    /// <returns>The resulting multivector.</returns>
    public static Multivector2 operator *(float a, Multivector2 b) => b * a;

    /// <summary>
    /// Computes the wedge product of two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting bivector.</returns>
    public static Bivector2 Wedge(Multivector2 a, Multivector2 b) =>
        new(a.R * b.B.XY + b.R * a.B.XY + a.V.X * b.V.Y - a.V.Y * b.V.X);

    /// <summary>
    /// Computes the dot product of two multivectors.
    /// </summary>
    /// <param name="a">The first multivector.</param>
    /// <param name="b">The second multivector.</param>
    /// <returns>The resulting scalar.</returns>
    public static float Dot(Multivector2 a, Multivector2 b) =>
        a.R * b.R + Vector2.Dot(a.V, b.V) + a.B.XY * b.B.XY;
}
