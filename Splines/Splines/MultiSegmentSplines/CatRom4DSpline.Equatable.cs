using System.Linq;

namespace Splines.Splines.MultiSegmentSplines;

public sealed partial class CatRom4DSpline : IEquatable<CatRom4DSpline>
{
    /// <summary>
    /// Determines whether the specified <see cref="CatRom4DSpline"/> is equal to the current <see cref="CatRom4DSpline"/>.
    /// </summary>
    /// <param name="other">The <see cref="CatRom4DSpline"/> to compare with the current <see cref="CatRom4DSpline"/>.</param>
    /// <returns>true if the specified <see cref="CatRom4DSpline"/> is equal to the current <see cref="CatRom4DSpline"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(CatRom4DSpline? other)
    {
        if (other is null)
        {
            return false;
        }

        return _alpha == other._alpha
               && _autoCalculateKnots == other._autoCalculateKnots
               && EndpointMode == other.EndpointMode
               && Nodes.SequenceEqual(other.Nodes);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="CatRom4DSpline"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="CatRom4DSpline"/>.</param>
    /// <returns>true if the specified object is equal to the current <see cref="CatRom4DSpline"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is CatRom4DSpline spline && Equals(spline);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="CatRom4DSpline"/>.</returns>
    [Pure]
    public override int GetHashCode()
    {
        return HashCode.Combine(
            _alpha,
            _autoCalculateKnots,
            EndpointMode,
            HashCode.Combine(Nodes));
    }

    /// <summary>
    /// Determines whether two instances of <see cref="CatRom4DSpline"/> are equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="CatRom4DSpline"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="CatRom4DSpline"/> to compare.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(CatRom4DSpline? left, CatRom4DSpline? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        if (right is null) return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two instances of <see cref="CatRom4DSpline"/> are not equal.
    /// </summary>
    /// <param name="left">The first instance of <see cref="CatRom4DSpline"/> to compare.</param>
    /// <param name="right">The second instance of <see cref="CatRom4DSpline"/> to compare.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(CatRom4DSpline? left, CatRom4DSpline? right) => !(left == right);
}
