using System.Linq;

namespace Splines.Splines.MultiSegmentSplines;

public sealed partial class NURBS1D : IEquatable<NURBS1D>
{
    /// <summary>
    /// Determines whether the specified <see cref="NURBS1D"/> is equal to the current <see cref="NURBS1D"/>.
    /// </summary>
    /// <param name="other">The <see cref="NURBS1D"/> to compare with the current <see cref="NURBS1D"/>.</param>
    /// <returns>true if the specified <see cref="NURBS1D"/> is equal to the current <see cref="NURBS1D"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(NURBS1D? other)
    {
        if (other == null)
            return false;

        return Degree == other.Degree
               && Points.SequenceEqual(other.Points)
               && Knots.SequenceEqual(other.Knots)
               && NullableSequenceEqual(Weights, other.Weights);
    }

    /// <summary>
    /// Determines whether two nullable arrays of floats are equal.
    /// </summary>
    /// <param name="left">The first array of floats to compare.</param>
    /// <param name="right">The second array of floats to compare.</param>
    /// <returns>true if the arrays are equal; otherwise, false.</returns>
    [Pure]
    private static bool NullableSequenceEqual(float[]? left, float[]? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        if (right is null) return false;
        return left.SequenceEqual(right);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="NURBS1D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="NURBS1D"/>.</param>
    /// <returns>true if the specified object is equal to the current <see cref="NURBS1D"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is NURBS1D nurbs && Equals(nurbs);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="NURBS1D"/>.</returns>
    [Pure]
    public override int GetHashCode()
    {
        return HashCode.Combine(
            Degree,
            HashCode.Combine(Points),
            HashCode.Combine(Knots),
            HashCode.Combine(Weights));
    }

    /// <summary>
    /// Determines whether two instances of <see cref="NURBS1D"/> are equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="NURBS1D"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="NURBS1D"/> to compare.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(NURBS1D? left, NURBS1D? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        if (right is null) return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two instances of <see cref="NURBS1D"/> are not equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="NURBS1D"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="NURBS1D"/> to compare.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(NURBS1D? left, NURBS1D? right) => !(left == right);
}
