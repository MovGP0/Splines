using System.Linq;

namespace Splines.Splines.UniformSplineSegments;

public sealed partial class Bezier1D
{
    /// <summary>
    /// Determines whether the specified <see cref="Bezier1D"/> is equal to the current <see cref="Bezier1D"/>.
    /// </summary>
    /// <param name="other">The <see cref="Bezier1D"/> to compare with the current <see cref="Bezier1D"/>.</param>
    /// <returns>true if the specified <see cref="Bezier1D"/> is equal to the current <see cref="Bezier1D"/>; otherwise, false.</returns>
    public bool Equals(Bezier1D other) => Points.SequenceEqual(other.Points);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Bezier1D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="Bezier1D"/>.</param>
    /// <returns>true if the specified object is a <see cref="Bezier1D"/> and is equal to the current <see cref="Bezier1D"/>; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Bezier1D otherBezier)
        {
            return Equals(otherBezier);
        }

        return false;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Bezier1D"/>.</returns>
    public override int GetHashCode() => HashCode.Combine(Points);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Bezier1D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="Bezier1D"/> to compare.</param>
    /// <param name="b">The second <see cref="Bezier1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Bezier1D a, Bezier1D b) => a.Equals(b);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Bezier1D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="Bezier1D"/> to compare.</param>
    /// <param name="b">The second <see cref="Bezier1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Bezier1D a, Bezier1D b) => !(a == b);
}
