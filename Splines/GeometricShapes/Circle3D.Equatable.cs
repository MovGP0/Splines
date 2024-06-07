namespace Splines.GeometricShapes;

public partial struct Circle3D : IEquatable<Circle3D>
{
    /// <summary>
    /// Indicates whether the current circle is equal to another circle.
    /// </summary>
    /// <param name="other">A circle to compare with this circle.</param>
    /// <returns>true if the current circle is equal to the other circle; otherwise, false.</returns>
    [Pure]
    public bool Equals(Circle3D other) => Center.Equals(other.Center) && Radius.Equals(other.Radius) && Normal.Equals(other.Normal);

    /// <summary>
    /// Determines whether the specified object is equal to the current circle.
    /// </summary>
    /// <param name="obj">The object to compare with the current circle.</param>
    /// <returns>true if the specified object is equal to the current circle; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Circle3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current circle.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Center, Radius, Normal);

    /// <summary>
    /// Determines whether two specified instances of Circle3D are equal.
    /// </summary>
    /// <param name="left">The first circle to compare.</param>
    /// <param name="right">The second circle to compare.</param>
    /// <returns>true if the two circles are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Circle3D left, Circle3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Circle3D are not equal.
    /// </summary>
    /// <param name="left">The first circle to compare.</param>
    /// <param name="right">The second circle to compare.</param>
    /// <returns>true if the two circles are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Circle3D left, Circle3D right) => !left.Equals(right);
}
