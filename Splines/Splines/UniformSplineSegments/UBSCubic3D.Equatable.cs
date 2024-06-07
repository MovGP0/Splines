﻿namespace Splines.Splines.UniformSplineSegments;

public partial struct UBSCubic3D : IEquatable<UBSCubic3D>
{
    /// <summary>
    /// Determines whether two specified instances of <see cref="UBSCubic3D"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="UBSCubic3D"/> to compare.</param>
    /// <param name="b">The second <see cref="UBSCubic3D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(UBSCubic3D a, UBSCubic3D b) => a.pointMatrix == b.pointMatrix;

    /// <summary>
    /// Determines whether two specified instances of <see cref="UBSCubic3D"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="UBSCubic3D"/> to compare.</param>
    /// <param name="b">The second <see cref="UBSCubic3D"/> to compare.</param>
    /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(UBSCubic3D a, UBSCubic3D b) => !(a == b);

    /// <summary>
    /// Determines whether the specified <see cref="UBSCubic3D"/> is equal to the current <see cref="UBSCubic3D"/>.
    /// </summary>
    /// <param name="other">The <see cref="UBSCubic3D"/> to compare with the current <see cref="UBSCubic3D"/>.</param>
    /// <returns>true if the specified <see cref="UBSCubic3D"/> is equal to the current <see cref="UBSCubic3D"/>; otherwise, false.</returns>
    [Pure]
    public bool Equals(UBSCubic3D other) => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="UBSCubic3D"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="UBSCubic3D"/>.</param>
    /// <returns>true if the specified object is a <see cref="UBSCubic3D"/> and is equal to the current <see cref="UBSCubic3D"/>; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is UBSCubic3D other && pointMatrix.Equals(other.pointMatrix);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current <see cref="UBSCubic3D"/>.</returns>
    [Pure]
    public override int GetHashCode() => pointMatrix.GetHashCode();
}
