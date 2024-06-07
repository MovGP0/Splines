namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a trivector in 3D space, which is a third-order tensor.
/// </summary>
/// <remarks>
/// A trivector can be used to represent oriented volumes in 3D space. This class provides various
/// operations for manipulating trivectors, such as addition, scalar multiplication, and products with vectors and bivectors.
/// </remarks>
/// <example>
/// <code>
/// Trivector3 trivector = new Trivector3(1.0f);
/// float value = trivector.XYZ;
/// </code>
/// </example>
public partial struct Trivector3
{
    /// <summary>
    /// Represents a trivector with all components set to zero.
    /// </summary>
    public static readonly Trivector3 Zero = new(0);

    /// <summary>
    /// The XYZ component of the trivector.
    /// </summary>
    public float XYZ
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Trivector3"/> struct with the specified component.
    /// </summary>
    /// <param name="xyz">The XYZ component.</param>
    public Trivector3(float xyz) => XYZ = xyz;
}
