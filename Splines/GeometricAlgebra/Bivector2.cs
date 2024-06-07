using System.Numerics;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a bivector in 2D space, which is an antisymmetric second-order tensor.
/// </summary>
/// <remarks>
/// A bivector can be used to represent oriented areas in 2D space. This class provides various
/// operations for manipulating bivectors, such as addition, subtraction, multiplication, and
/// conversion to and from vectors.
/// </remarks>
/// <example>
/// <code>
/// Bivector2 bivector = new Bivector2(3.0f);
/// float magnitude = bivector.Magnitude;
/// </code>
/// </example>
[Serializable]
public partial struct Bivector2
{
    /// <summary>
    /// Represents a bivector with all components set to zero.
    /// </summary>
    public static readonly Bivector2 Zero = new(0);

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
    /// <param name="i">The index of the component (0 for XY).</param>
    /// <returns>The value of the component at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is not 0.</exception>
    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 => XY,
                _ => throw new IndexOutOfRangeException()
            };
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bivector2"/> struct with the specified component.
    /// </summary>
    /// <param name="xy">The XY component.</param>
    public Bivector2(float xy)
    {
        XY = xy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bivector2"/> struct from two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    public Bivector2(Vector2 a, Vector2 b)
    {
        XY = a.X * b.Y - a.Y * b.X;
    }

    /// <summary>
    /// Gets the magnitude of the bivector.
    /// </summary>
    [Pure]
    public float Magnitude => Mathf.Abs(XY);

    /// <summary>
    /// Gets a normalized version of the bivector.
    /// </summary>
    [Pure]
    public Bivector2 Normalized => new Bivector2(XY / Magnitude);

    /// <summary>
    /// Gets the normal vector of the bivector.
    /// </summary>
    [Pure]
    public Vector2 Normal => new Vector2(-XY, XY).Normalized();

    /// <summary>
    /// Gets the Hodge dual of the bivector.
    /// </summary>
    [Pure]
    public float HodgeDual => XY;

    /// <summary>
    /// Gets the square of the magnitude of the bivector.
    /// </summary>
    [Pure]
    public float SqrMagnitude => XY * XY;

    /// <inheritdoc cref="Dot(Bivector2, Bivector2)"/>
    [Pure]
    public float Dot(Bivector2 b) => Dot(this, b);

    /// <inheritdoc cref="Wedge(Bivector2, Bivector2)"/>
    [Pure]
    public Bivector2 Wedge(Bivector2 b) => Wedge(this, b);

    /// <summary>The real part when multiplying two bivectors</summary>
    [Pure]
    public static float Dot(Bivector2 a, Bivector2 b) => a.XY * b.XY;

    /// <summary>The bivector part when multiplying two bivectors</summary>
    [Pure]
    public static Bivector2 Wedge(Bivector2 a, Bivector2 b) => new Bivector2(a.XY * b.XY);

    /// <summary>
    /// Negates the bivector.
    /// </summary>
    /// <param name="b">The bivector to negate.</param>
    /// <returns>The negated bivector.</returns>
    [Pure]
    public static Bivector2 operator -(Bivector2 b) => new(-b.XY);

    /// <summary>
    /// Multiplies a bivector by a scalar.
    /// </summary>
    /// <param name="a">The scalar.</param>
    /// <param name="b">The bivector.</param>
    /// <returns>The scaled bivector.</returns>
    [Pure]
    public static Bivector2 operator *(float a, Bivector2 b) => b * a;

    /// <summary>
    /// Multiplies a bivector by a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The scaled bivector.</returns>
    [Pure]
    public static Bivector2 operator *(Bivector2 a, float b) => new(a.XY * b);

    /// <summary>
    /// Divides a bivector by a scalar.
    /// </summary>
    /// <param name="a">The bivector.</param>
    /// <param name="b">The scalar.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector2 operator /(Bivector2 a, float b) => new(a.XY / b);

    /// <summary>
    /// Adds two bivectors.
    /// </summary>
    /// <param name="a">The first bivector.</param>
    /// <param name="b">The second bivector.</param>
    /// <returns>The resulting bivector.</returns>
    [Pure]
    public static Bivector2 operator +(Bivector2 a, Bivector2 b) => new(a.XY + b.XY);
}
