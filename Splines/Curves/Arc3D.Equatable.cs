namespace Splines.Curves;

public partial struct Arc3D : IEquatable<Arc3D>
{
    /// <summary>
    /// Determines whether the specified <see cref="Arc3D"/> is equal to the current <see cref="Arc3D"/>.
    /// </summary>
    /// <param name="other">The <see cref="Arc3D"/> to compare with the current <see cref="Arc3D"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="Arc3D"/> is equal to the current <see cref="Arc3D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Arc3D other)
    {
        return Placement.Equals(other.Placement) &&
               CurvatureXY.Equals(other.CurvatureXY) &&
               CurvatureZ.Equals(other.CurvatureZ) &&
               Length.Equals(other.Length);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Arc3D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Arc3D"/>.</param>
    /// <returns><c>true</c> if the specified object is equal to the current <see cref="Arc3D"/>; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Arc3D other && Equals(other);

    /// <summary>
    /// Serves as a hash function for the <see cref="Arc3D"/>.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Arc3D"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Placement, CurvatureXY, CurvatureZ, Length);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Arc3D"/> are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Arc3D"/> to compare.</param>
    /// <param name="right">The second <see cref="Arc3D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Arc3D"/> instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Arc3D left, Arc3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Arc3D"/> are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Arc3D"/> to compare.</param>
    /// <param name="right">The second <see cref="Arc3D"/> to compare.</param>
    /// <returns><c>true</c> if the two <see cref="Arc3D"/> instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Arc3D left, Arc3D right) => !left.Equals(right);
}
