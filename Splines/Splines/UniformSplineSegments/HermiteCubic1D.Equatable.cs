namespace Splines.Splines.UniformSplineSegments;

public partial struct HermiteCubic1D : IEquatable<HermiteCubic1D>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="HermiteCubic1D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="HermiteCubic1D"/> to compare.</param>
    /// <param name="b">The second <see cref="HermiteCubic1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(HermiteCubic1D a, HermiteCubic1D b) => a.pointMatrix == b.pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="HermiteCubic1D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="HermiteCubic1D"/> to compare.</param>
    /// <param name="b">The second <see cref="HermiteCubic1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(HermiteCubic1D a, HermiteCubic1D b) => !(a == b);

    /// <summary>
    /// Determines whether the specified <see cref="HermiteCubic1D"/> is equal to the current <see cref="HermiteCubic1D"/>.
    /// </summary>
    /// <param name="other">The <see cref="HermiteCubic1D"/> to compare with the current <see cref="HermiteCubic1D"/>.</param>
    /// <returns>true if the specified <see cref="HermiteCubic1D"/> is equal to the current <see cref="HermiteCubic1D"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(HermiteCubic1D other) => P0.Equals(other.P0) && V0.Equals(other.V0) && P1.Equals(other.P1) && V1.Equals(other.V1);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="HermiteCubic1D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="HermiteCubic1D"/>.</param>
    /// <returns>true if the specified object is a <see cref="HermiteCubic1D"/> and is equal to the current <see cref="HermiteCubic1D"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is HermiteCubic1D other && pointMatrix.Equals(other.pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="HermiteCubic1D"/>.</returns>
    [Pure]
    public override int GetHashCode() => pointMatrix.GetHashCode();
}
