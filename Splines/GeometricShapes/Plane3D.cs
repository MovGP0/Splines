using System.Numerics;

namespace Splines.GeometricShapes;

/// <summary>A mathematical plane in 3D space</summary>
[Serializable]
public struct Plane3D : IEquatable<Plane3D>
{
    /// <summary>The normal of the plane. Note that this type lets you assign non-normalized vectors</summary>
    public Vector3 Normal
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The signed distance from the world origin</summary>
    public float Distance
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Returns the point on the plane closest to the world origin</summary>
    [Pure]
    public Vector3 PointClosestToOrigin => Normal * Distance;

    /// <summary>Creates a plane with a normal vector and a signed distance from the world origin</summary>
    /// <param name="normal">The normal of the plane</param>
    /// <param name="distance">The signed distance from the world origin</param>
    public Plane3D(Vector3 normal, float distance)
    {
        Normal = normal;
        Distance = distance;
    }

    /// <summary>Creates a plane with a normal vector and a point on the plane</summary>
    /// <param name="normal">The normal of the plane</param>
    /// <param name="point">A point on the plane</param>
    public Plane3D(Vector3 normal, Vector3 point)
    {
        Normal = normal;
        Distance = Vector3.Dot(normal, point);
    }

    /// <summary>
    /// Indicates whether the current plane is equal to another plane.
    /// </summary>
    /// <param name="other">A plane to compare with this plane.</param>
    /// <returns>true if the current plane is equal to the other plane; otherwise, false.</returns>
    [Pure]
    public bool Equals(Plane3D other) => Normal.Equals(other.Normal) && Distance.Equals(other.Distance);

    /// <summary>
    /// Determines whether the specified object is equal to the current plane.
    /// </summary>
    /// <param name="obj">The object to compare with the current plane.</param>
    /// <returns>true if the specified object is equal to the current plane; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Plane3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current plane.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Normal, Distance);

    /// <summary>
    /// Determines whether two specified instances of Plane3D are equal.
    /// </summary>
    /// <param name="left">The first plane to compare.</param>
    /// <param name="right">The second plane to compare.</param>
    /// <returns>true if the two planes are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Plane3D left, Plane3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Plane3D are not equal.
    /// </summary>
    /// <param name="left">The first plane to compare.</param>
    /// <param name="right">The second plane to compare.</param>
    /// <returns>true if the two planes are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Plane3D left, Plane3D right) => !left.Equals(right);
}
