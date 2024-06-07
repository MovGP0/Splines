namespace Splines.Splines.UniformSplineSegments;

public partial struct CatRomCubic3D : IEquatable<CatRomCubic3D>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="CatRomCubic3D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="CatRomCubic3D"/> to compare.</param>
    /// <param name="b">The second <see cref="CatRomCubic3D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(CatRomCubic3D a, CatRomCubic3D b) => a._pointMatrix == b._pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="CatRomCubic3D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="CatRomCubic3D"/> to compare.</param>
    /// <param name="b">The second <see cref="CatRomCubic3D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(CatRomCubic3D a, CatRomCubic3D b) => !(a == b);

    /// <summary>
    /// Determines whether the specified <see cref="CatRomCubic3D"/> is equal to the current <see cref="CatRomCubic3D"/>.
    /// </summary>
    /// <param name="other">The <see cref="CatRomCubic3D"/> to compare with the current <see cref="CatRomCubic3D"/>.</param>
    /// <returns>true if the specified <see cref="CatRomCubic3D"/> is equal to the current <see cref="CatRomCubic3D"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(CatRomCubic3D other) => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="CatRomCubic3D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="CatRomCubic3D"/>.</param>
    /// <returns>true if the specified object is a <see cref="CatRomCubic3D"/> and is equal to the current <see cref="CatRomCubic3D"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is CatRomCubic3D other && _pointMatrix.Equals(other._pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="CatRomCubic3D"/>.</returns>
    [Pure]
    public override int GetHashCode() => _pointMatrix.GetHashCode();
}
