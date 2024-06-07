namespace Splines.GeometricShapes;

public partial struct Box3D : IEquatable<Box3D>
{
    /// <summary>
    /// Indicates whether the current box is equal to another box.
    /// </summary>
    /// <param name="other">A box to compare with this box.</param>
    /// <returns>true if the current box is equal to the other box; otherwise, false.</returns>
    [Pure]
    public bool Equals(Box3D other) => Center.Equals(other.Center) && Extents.Equals(other.Extents);

    /// <summary>
    /// Determines whether the specified object is equal to the current box.
    /// </summary>
    /// <param name="obj">The object to compare with the current box.</param>
    /// <returns>true if the specified object is equal to the current box; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Box3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current box.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Center, Extents);

    /// <summary>
    /// Determines whether two specified instances of Box3D are equal.
    /// </summary>
    /// <param name="left">The first box to compare.</param>
    /// <param name="right">The second box to compare.</param>
    /// <returns>true if the two boxes are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Box3D left, Box3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Box3D are not equal.
    /// </summary>
    /// <param name="left">The first box to compare.</param>
    /// <param name="right">The second box to compare.</param>
    /// <returns>true if the two boxes are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Box3D left, Box3D right) => !left.Equals(right);
}
