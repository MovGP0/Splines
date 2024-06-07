namespace Splines.Curves;

public partial struct Catenary4D : IEquatable<Catenary4D>
{
    /// <summary>
    /// Determines whether the specified <see cref="Catenary4D"/> is equal to the current <see cref="Catenary4D"/>.
    /// </summary>
    /// <param name="other">The <see cref="Catenary4D"/> to compare with the current <see cref="Catenary4D"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Catenary4D"/> is equal to the current <see cref="Catenary4D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Catenary4D other)
    {
        return _p1.Equals(other._p1) &&
               _cat3D.Equals(other._cat3D) &&
               _space.Equals(other._space) &&
               _evaluability == other._evaluability;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Catenary4D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Catenary4D"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Catenary4D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Catenary4D other && Equals(other);

    /// <summary>
    /// Serves as a hash function for the <see cref="Catenary4D"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Catenary4D"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_p1, _cat3D, _space, _evaluability);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Catenary4D"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Catenary4D"/> to compare.</param>
    /// <param name="right">The second <see cref="Catenary4D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Catenary4D"/> instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Catenary4D left, Catenary4D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Catenary4D"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Catenary4D"/> to compare.</param>
    /// <param name="right">The second <see cref="Catenary4D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Catenary4D"/> instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Catenary4D left, Catenary4D right) => !left.Equals(right);
}
