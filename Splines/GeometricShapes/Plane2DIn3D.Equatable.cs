namespace Splines.GeometricShapes;

public partial struct Plane2DIn3D : IEquatable<Plane2DIn3D>
{
    /// <summary>
    /// Indicates whether the current plane is equal to another plane.
    /// </summary>
    /// <param name="other">A plane to compare with this plane.</param>
    /// <returns>true if the current plane is equal to the other plane; otherwise, false.</returns>
    [Pure]
    public bool Equals(Plane2DIn3D other) => Origin.Equals(other.Origin) && AxisX.Equals(other.AxisX) && AxisY.Equals(other.AxisY);

    /// <summary>
    /// Determines whether the specified object is equal to the current plane.
    /// </summary>
    /// <param name="obj">The object to compare with the current plane.</param>
    /// <returns>true if the specified object is equal to the current plane; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Plane2DIn3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current plane.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, AxisX, AxisY);

    /// <summary>
    /// Determines whether two specified instances of Plane2DIn3D are equal.
    /// </summary>
    /// <param name="left">The first plane to compare.</param>
    /// <param name="right">The second plane to compare.</param>
    /// <returns>true if the two planes are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Plane2DIn3D left, Plane2DIn3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Plane2DIn3D are not equal.
    /// </summary>
    /// <param name="left">The first plane to compare.</param>
    /// <param name="right">The second plane to compare.</param>
    /// <returns>true if the two planes are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Plane2DIn3D left, Plane2DIn3D right) => !left.Equals(right);
}
