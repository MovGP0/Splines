namespace Splines.Splines.UniformSplineSegments;

public partial struct BezierCubic2D : IEquatable<BezierCubic2D>
{
    /// <summary>
    /// Determines whether the specified <see cref="BezierCubic2D"/> is equal to the current <see cref="BezierCubic2D"/>.
    /// </summary>
    /// <param name="other">The <see cref="BezierCubic2D"/> to compare with the current <see cref="BezierCubic2D"/>.</param>
    /// <returns>
    /// true if the specified <see cref="BezierCubic2D"/> is equal to the current <see cref="BezierCubic2D"/>; otherwise, false.
    /// </returns>
    [Pure]
    public bool Equals(BezierCubic2D other) => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="BezierCubic2D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="BezierCubic2D"/>.</param>
    /// <returns>
    /// true if the specified object is a <see cref="BezierCubic2D"/> and is equal to the current <see cref="BezierCubic2D"/>; otherwise, false.
    /// </returns>
    [Pure]
    public override bool Equals(object? obj) => obj is BezierCubic2D other && _pointMatrix.Equals(other._pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="BezierCubic2D"/>.</returns>
    [Pure]
    public override int GetHashCode() => _pointMatrix.GetHashCode();

    /// <summary>
    /// Determines whether two specified instances of <see cref="BezierCubic2D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="BezierCubic2D"/> to compare.</param>
    /// <param name="b">The second <see cref="BezierCubic2D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(BezierCubic2D a, BezierCubic2D b) => a._pointMatrix == b._pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="BezierCubic2D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="BezierCubic2D"/> to compare.</param>
    /// <param name="b">The second <see cref="BezierCubic2D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(BezierCubic2D a, BezierCubic2D b) => !(a == b);
}
