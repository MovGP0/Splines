namespace Splines.GeometricShapes;

public partial struct Circle2D : IEquatable<Circle2D>
{
    /// <summary>
    /// Indicates whether the current circle is equal to another circle.
    /// </summary>
    /// <param name="other">A circle to compare with this circle.</param>
    /// <returns>true if the current circle is equal to the other circle; otherwise, false.</returns>
    [Pure]
    public bool Equals(Circle2D other) => Center.Equals(other.Center) && Radius.Equals(other.Radius);

    /// <summary>
    /// Determines whether the specified object is equal to the current circle.
    /// </summary>
    /// <param name="obj">The object to compare with the current circle.</param>
    /// <returns>true if the specified object is equal to the current circle; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Circle2D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current circle.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Center, Radius);

    /// <summary>
    /// Determines whether two specified instances of Circle2D are equal.
    /// </summary>
    /// <param name="left">The first circle to compare.</param>
    /// <param name="right">The second circle to compare.</param>
    /// <returns>true if the two circles are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Circle2D left, Circle2D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Circle2D are not equal.
    /// </summary>
    /// <param name="left">The first circle to compare.</param>
    /// <param name="right">The second circle to compare.</param>
    /// <returns>true if the two circles are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Circle2D left, Circle2D right) => !left.Equals(right);
}
