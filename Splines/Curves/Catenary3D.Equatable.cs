namespace Splines.Curves;

public partial struct Catenary3D : IEquatable<Catenary3D>
{
    /// <summary>
    /// Determines whether the specified <see cref="Catenary3D"/> is equal to the current <see cref="Catenary3D"/>.
    /// </summary>
    /// <param name="other">The <see cref="Catenary3D"/> to compare with the current <see cref="Catenary3D"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Catenary3D"/> is equal to the current <see cref="Catenary3D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Catenary3D other)
    {
        return _p1.Equals(other._p1) &&
               _cat2D.Equals(other._cat2D) &&
               _space.Equals(other._space) &&
               _evaluability == other._evaluability;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Catenary3D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Catenary3D"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Catenary3D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Catenary3D other && Equals(other);

    /// <summary>
    /// Serves as a hash function for the <see cref="Catenary3D"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Catenary3D"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(_p1, _cat2D, _space, _evaluability);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Catenary3D"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Catenary3D"/> to compare.</param>
    /// <param name="right">The second <see cref="Catenary3D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Catenary3D"/> instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Catenary3D left, Catenary3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Catenary3D"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Catenary3D"/> to compare.</param>
    /// <param name="right">The second <see cref="Catenary3D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Catenary3D"/> instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Catenary3D left, Catenary3D right) => !left.Equals(right);
}
