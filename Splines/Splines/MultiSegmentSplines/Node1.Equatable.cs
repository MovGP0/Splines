namespace Splines.Splines.MultiSegmentSplines;

public partial struct Node1 : IEquatable<Node1>
{
    /// <summary>
    /// Determines whether the specified <see cref="Node1"/> is equal to the current <see cref="Node1"/>.
    /// </summary>
    /// <param name="other">The <see cref="Node1"/> to compare with the current <see cref="Node1"/>.</param>
    /// <returns>true if the specified <see cref="Node1"/> is equal to the current <see cref="Node1"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(Node1 other)
    {
        return Position.Equals(other.Position)
               && Knot == other.Knot
               && Curve.Equals(other.Curve);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Node1"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Node1"/>.</param>
    /// <returns>true if the specified object is equal to the current <see cref="Node1"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Node1 node && Equals(node);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Node1"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Position, Knot, Curve);

    /// <summary>
    /// Determines whether two instances of <see cref="Node1"/> are equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="Node1"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="Node1"/> to compare.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Node1 left, Node1 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two instances of <see cref="Node1"/> are not equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="Node1"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="Node1"/> to compare.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Node1 left, Node1 right) => !(left == right);
}
