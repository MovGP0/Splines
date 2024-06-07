namespace Splines.GeometricAlgebra;

/// <summary>
/// Represents a quadvector in 4D space, which is a fourth-order tensor.
/// </summary>
/// <remarks>
/// A quadvector can be used to represent oriented hypervolumes in 4D space. This class provides various
/// operations for manipulating quadvectors, such as addition, scalar multiplication, and products with vectors, bivectors, and trivectors.
/// </remarks>
/// <example>
/// <code>
/// Quadvector4 quadvector = new Quadvector4(1.0f);
/// float value = quadvector.XYZW;
/// </code>
/// </example>
public partial struct Quadvector4
{
    /// <summary>
    /// Represents a quadvector with all components set to zero.
    /// </summary>
    public static readonly Quadvector4 Zero = new(0);

    /// <summary>
    /// The XYZW component of the quadvector.
    /// </summary>
    public float XYZW
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Quadvector4"/> struct with the specified component.
    /// </summary>
    /// <param name="xyzw">The XYZW component.</param>
    public Quadvector4(float xyzw) => XYZW = xyzw;
}
