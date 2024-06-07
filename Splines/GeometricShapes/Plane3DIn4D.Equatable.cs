namespace Splines.GeometricShapes;

public partial struct Plane3DIn4D : IEquatable<Plane3DIn4D>
{
    /// <summary>
    /// Indicates whether the current plane is equal to another plane.
    /// </summary>
    /// <param name="other">A plane to compare with this plane.</param>
    /// <returns>true if the current plane is equal to the other plane; otherwise, false.</returns>
    [Pure]
    public bool Equals(Plane3DIn4D other) => Origin.Equals(other.Origin) && AxisX.Equals(other.AxisX) && AxisY.Equals(other.AxisY) && AxisZ.Equals(other.AxisZ);

    /// <summary>
    /// Determines whether the specified object is equal to the current plane.
    /// </summary>
    /// <param name="obj">The object to compare with the current plane.</param>
    /// <returns>true if the specified object is equal to the current plane; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Plane3DIn4D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current plane.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, AxisX, AxisY, AxisZ);

    /// <summary>
    /// Determines whether two specified instances of Plane3DIn4D are equal.
    /// </summary>
    /// <param name="left">The first plane to compare.</param>
    /// <param name="right">The second plane to compare.</param>
    /// <returns>true if the two planes are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Plane3DIn4D left, Plane3DIn4D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Plane3DIn4D are not equal.
    /// </summary>
    /// <param name="left">The first plane to compare.</param>
    /// <param name="right">The second plane to compare.</param>
    /// <returns>true if the two planes are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Plane3DIn4D left, Plane3DIn4D right) => !left.Equals(right);
}
