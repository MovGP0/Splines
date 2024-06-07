namespace Splines.GeometricShapes;

public partial struct Ray1D : IEquatable<Ray1D>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Ray1D ray && Equals(ray);

    /// <summary>
    /// Determines whether the specified <see cref="Ray1D"/> is equal to the current <see cref="Ray1D"/>.
    /// </summary>
    /// <param name="other">The ray to compare with the current ray.</param>
    /// <returns><c>true</c> if the specified ray is equal to the current ray; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Ray1D other) => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, Direction);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Ray1D"/> are equal.
    /// </summary>
    /// <param name="left">The first ray to compare.</param>
    /// <param name="right">The second ray to compare.</param>
    /// <returns><c>true</c> if the specified rays are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Ray1D left, Ray1D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Ray1D"/> are not equal.
    /// </summary>
    /// <param name="left">The first ray to compare.</param>
    /// <param name="right">The second ray to compare.</param>
    /// <returns><c>true</c> if the specified rays are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Ray1D left, Ray1D right) => !(left == right);
}
