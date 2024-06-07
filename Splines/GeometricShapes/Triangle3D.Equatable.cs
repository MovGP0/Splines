namespace Splines.GeometricShapes;

public partial struct Triangle3D : IEquatable<Triangle3D>
{
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
    [Pure]
    public bool Equals(Triangle3D other) => A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C);

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Triangle3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(A, B, C);

    /// <summary>
    /// Indicates whether two <see cref="Triangle3D"/> instances are equal.
    /// </summary>
    /// <param name="left">The first <see cref="Triangle3D"/> to compare.</param>
    /// <param name="right">The second <see cref="Triangle3D"/> to compare.</param>
    /// <returns>true if the two <see cref="Triangle3D"/> instances are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Triangle3D left, Triangle3D right) => left.Equals(right);

    /// <summary>
    /// Indicates whether two <see cref="Triangle3D"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first <see cref="Triangle3D"/> to compare.</param>
    /// <param name="right">The second <see cref="Triangle3D"/> to compare.</param>
    /// <returns>true if the two <see cref="Triangle3D"/> instances are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Triangle3D left, Triangle3D right) => !left.Equals(right);
}
