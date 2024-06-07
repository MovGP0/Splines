namespace Splines.Curves;

public partial struct Catenary2D : IEquatable<Catenary2D>
{
    /// <summary>
    /// Determines whether the specified <see cref="Catenary2D"/> is equal to the current <see cref="Catenary2D"/>.
    /// </summary>
    /// <param name="other">The <see cref="Catenary2D"/> to compare with the current <see cref="Catenary2D"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Catenary2D"/> is equal to the current <see cref="Catenary2D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Catenary2D other)
    {
        return _p1.Equals(other._p1) &&
               _catenary.Equals(other._catenary) &&
               _space.Equals(other._space) &&
               _evaluability == other._evaluability;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Catenary2D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Catenary2D"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Catenary2D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Catenary2D other && Equals(other);

    /// <summary>
    /// Serves as a hash function for the <see cref="Catenary2D"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Catenary2D"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_p1, _catenary, _space, _evaluability);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Catenary2D"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Catenary2D"/> to compare.</param>
    /// <param name="right">The second <see cref="Catenary2D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Catenary2D"/> instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Catenary2D left, Catenary2D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Catenary2D"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Catenary2D"/> to compare.</param>
    /// <param name="right">The second <see cref="Catenary2D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Catenary2D"/> instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Catenary2D left, Catenary2D right) => !left.Equals(right);
}
