namespace Splines.Splines.UniformSplineSegments;

public partial struct BezierCubic1D : IEquatable<BezierCubic1D>
{
    /// <summary>
    /// Determines whether the specified <see cref="BezierCubic1D"/> is equal to the current <see cref="BezierCubic1D"/>.
    /// </summary>
    /// <param name="other">The <see cref="BezierCubic1D"/> to compare with the current <see cref="BezierCubic1D"/>.</param>
    /// <returns>
    /// true if the specified <see cref="BezierCubic1D"/> is equal to the current <see cref="BezierCubic1D"/>; otherwise, false.
    /// </returns>
    [Pure]
    public bool Equals(BezierCubic1D other) => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="BezierCubic1D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="BezierCubic1D"/>.</param>
    /// <returns>
    /// true if the specified object is a <see cref="BezierCubic1D"/> and is equal to the current <see cref="BezierCubic1D"/>; otherwise, false.
    /// </returns>
    [Pure]
    public override bool Equals(object? obj) => obj is BezierCubic1D other && _pointMatrix.Equals(other._pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="BezierCubic1D"/>.</returns>
    [Pure]
    public override int GetHashCode() => _pointMatrix.GetHashCode();

    /// <summary>
    /// Determines whether two specified instances of <see cref="BezierCubic1D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="BezierCubic1D"/> to compare.</param>
    /// <param name="b">The second <see cref="BezierCubic1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(BezierCubic1D a, BezierCubic1D b) => a._pointMatrix == b._pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="BezierCubic1D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="BezierCubic1D"/> to compare.</param>
    /// <param name="b">The second <see cref="BezierCubic1D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(BezierCubic1D a, BezierCubic1D b) => !(a == b);
}
