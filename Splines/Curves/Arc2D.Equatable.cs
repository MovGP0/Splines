namespace Splines.Curves;

public partial struct Arc2D : IEquatable<Arc2D>
{
    /// <summary>
    /// Determines whether the specified <see cref="Arc2D"/> is equal to the current <see cref="Arc2D"/>.
    /// </summary>
    /// <param name="other">The <see cref="Arc2D"/> to compare with the current <see cref="Arc2D"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Arc2D"/> is equal to the current <see cref="Arc2D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Arc2D other)
    {
        return Placement.Equals(other.Placement) &&
               Curvature.Equals(other.Curvature) &&
               Length.Equals(other.Length);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Arc2D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Arc2D"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Arc2D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Arc2D other && Equals(other);

    /// <summary>
    /// Serves as a hash function for the <see cref="Arc2D"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Arc2D"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Placement, Curvature, Length);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Arc2D"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Arc2D"/> to compare.</param>
    /// <param name="right">The second <see cref="Arc2D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Arc2D"/> instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Arc2D left, Arc2D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Arc2D"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Arc2D"/> to compare.</param>
    /// <param name="right">The second <see cref="Arc2D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Arc2D"/> instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Arc2D left, Arc2D right) => !left.Equals(right);
}
