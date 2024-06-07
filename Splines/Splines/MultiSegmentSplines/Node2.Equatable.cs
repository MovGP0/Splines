namespace Splines.Splines.MultiSegmentSplines;

public partial struct Node2 : IEquatable<Node2>
{
    /// <summary>
    /// Determines whether the specified <see cref="Node2"/> is equal to the current <see cref="Node2"/>.
    /// </summary>
    /// <param name="other">The <see cref="Node2"/> to compare with the current <see cref="Node2"/>.</param>
    /// <returns>true if the specified <see cref="Node2"/> is equal to the current <see cref="Node2"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(Node2 other)
    {
        return Position.Equals(other.Position)
               && Knot == other.Knot
               && Curve.Equals(other.Curve);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Node2"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Node2"/>.</param>
    /// <returns>true if the specified object is equal to the current <see cref="Node2"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Node2 node && Equals(node);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Node2"/>.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Position, Knot, Curve);

    /// <summary>
    /// Determines whether two instances of <see cref="Node2"/> are equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="Node2"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="Node2"/> to compare.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Node2? left, Node2? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        if (right is null) return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two instances of <see cref="Node2"/> are not equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="Node2"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="Node2"/> to compare.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Node2? left, Node2? right) => !(left == right);
}
