namespace Splines.GeometricAlgebra;

public partial struct Bivector3 : IEquatable<Bivector3>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current bivector.
    /// </summary>
    /// <param name="obj">The object to compare with the current bivector.</param>
    /// <returns>True if the specified object is equal to the current bivector; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Bivector3 bv && Equals(bv);

    /// <summary>
    /// Determines whether the specified bivector is equal to the current bivector.
    /// </summary>
    /// <param name="other">The bivector to compare with the current bivector.</param>
    /// <returns>True if the specified bivector is equal to the current bivector; otherwise, false.</returns>
    [Pure]
    public bool Equals(Bivector3 other) => YZ == other.YZ && ZX == other.ZX && XY == other.XY;

    /// <summary>
    /// Returns a hash code for the current bivector.
    /// </summary>
    /// <returns>A hash code for the current bivector.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(YZ, ZX, XY);

    /// <summary>
    /// Determines whether two <see cref="Bivector3"/> instances are equal.
    /// </summary>
    /// <param name="left">The first bivector to compare.</param>
    /// <param name="right">The second bivector to compare.</param>
    /// <returns>True if the two bivectors are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Bivector3 left, Bivector3 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Bivector3"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first bivector to compare.</param>
    /// <param name="right">The second bivector to compare.</param>
    /// <returns>True if the two bivectors are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Bivector3 left, Bivector3 right) => !(left == right);
}
