namespace Splines.Splines.UniformSplineSegments;

public partial struct UBSCubic1D : IEquatable<UBSCubic1D>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="UBSCubic1D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="UBSCubic1D"/> to compare.</param>
    /// <param name="b">The second <see cref="UBSCubic1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(UBSCubic1D a, UBSCubic1D b) => a.pointMatrix == b.pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="UBSCubic1D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="UBSCubic1D"/> to compare.</param>
    /// <param name="b">The second <see cref="UBSCubic1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(UBSCubic1D a, UBSCubic1D b) => !(a == b);

    /// <summary>
    /// Determines whether the specified <see cref="UBSCubic1D"/> is equal to the current <see cref="UBSCubic1D"/>.
    /// </summary>
    /// <param name="other">The <see cref="UBSCubic1D"/> to compare with the current <see cref="UBSCubic1D"/>.</param>
    /// <returns>true if the specified <see cref="UBSCubic1D"/> is equal to the current <see cref="UBSCubic1D"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(UBSCubic1D other) => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="UBSCubic1D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="UBSCubic1D"/>.</param>
    /// <returns>true if the specified object is a <see cref="UBSCubic1D"/> and is equal to the current <see cref="UBSCubic1D"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is UBSCubic1D other && pointMatrix.Equals(other.pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="UBSCubic1D"/>.</returns>
    [Pure]
    public override int GetHashCode() => pointMatrix.GetHashCode();
}
