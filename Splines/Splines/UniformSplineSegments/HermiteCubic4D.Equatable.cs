namespace Splines.Splines.UniformSplineSegments;

public partial struct HermiteCubic4D : IEquatable<HermiteCubic4D>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="HermiteCubic4D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="HermiteCubic4D"/> to compare.</param>
    /// <param name="b">The second <see cref="HermiteCubic4D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    public static bool operator ==(HermiteCubic4D a, HermiteCubic4D b) => a.pointMatrix == b.pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="HermiteCubic4D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="HermiteCubic4D"/> to compare.</param>
    /// <param name="b">The second <see cref="HermiteCubic4D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    public static bool operator !=(HermiteCubic4D a, HermiteCubic4D b) => !(a == b);

    /// <summary>
    /// Determines whether the specified <see cref="HermiteCubic4D"/> is equal to the current <see cref="HermiteCubic4D"/>.
    /// </summary>
    /// <param name="other">The <see cref="HermiteCubic4D"/> to compare with the current <see cref="HermiteCubic4D"/>.</param>
    /// <returns>true if the specified <see cref="HermiteCubic4D"/> is equal to the current <see cref="HermiteCubic4D"/>; otherwise, false.</returns>
    public bool Equals(HermiteCubic4D other) => P0.Equals(other.P0) && V0.Equals(other.V0) && P1.Equals(other.P1) && V1.Equals(other.V1);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="HermiteCubic4D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="HermiteCubic4D"/>.</param>
    /// <returns>true if the specified object is a <see cref="HermiteCubic4D"/> and is equal to the current <see cref="HermiteCubic4D"/>; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is HermiteCubic4D other && pointMatrix.Equals(other.pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="HermiteCubic4D"/>.</returns>
    public override int GetHashCode() => pointMatrix.GetHashCode();
}
