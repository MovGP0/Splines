namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a trivector in 4D space, which is a third-order tensor.
/// </summary>
/// <remarks>
/// A trivector can be used to represent oriented volumes in 4D space. This class provides various
/// operations for manipulating trivectors, such as addition, scalar multiplication, and products with vectors and bivectors.
/// </remarks>
/// <example>
/// <code>
/// Trivector4 trivector = new Trivector4(1.0f, 2.0f, 3.0f, 4.0f);
/// float value = trivector.XYZ;
/// </code>
/// </example>
public partial struct Trivector4
{
    /// <summary>
    /// Represents a trivector with all components set to zero.
    /// </summary>
    public static readonly Trivector4 Zero = new(0, 0, 0, 0);

    /// <summary>
    /// The XYZ component of the trivector.
    /// </summary>
    public float XYZ { get; set; }

    /// <summary>
    /// The XYW component of the trivector.
    /// </summary>
    public float XYW { get; set; }

    /// <summary>
    /// The XZW component of the trivector.
    /// </summary>
    public float XZW { get; set; }

    /// <summary>
    /// The YZW component of the trivector.
    /// </summary>
    public float YZW { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Trivector4"/> struct with the specified components.
    /// </summary>
    /// <param name="xyz">The XYZ component.</param>
    /// <param name="xyw">The XYW component.</param>
    /// <param name="xzw">The XZW component.</param>
    /// <param name="yzw">The YZW component.</param>
    public Trivector4(float xyz, float xyw, float xzw, float yzw)
    {
        XYZ = xyz;
        XYW = xyw;
        XZW = xzw;
        YZW = yzw;
    }
}
