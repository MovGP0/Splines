namespace Splines.GeometricAlgebra;

public partial struct Bivector2 : IEquatable<Bivector2>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current bivector.
    /// </summary>
    /// <param name="obj">The object to compare with the current bivector.</param>
    /// <returns>True if the specified object is equal to the current bivector; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Bivector2 bv && Equals(bv);

    /// <summary>
    /// Determines whether the specified bivector is equal to the current bivector.
    /// </summary>
    /// <param name="other">The bivector to compare with the current bivector.</param>
    /// <returns>True if the specified bivector is equal to the current bivector; otherwise, false.</returns>
    [Pure]
    public bool Equals(Bivector2 other) => XY == other.XY;

    /// <summary>
    /// Returns a hash code for the current bivector.
    /// </summary>
    /// <returns>A hash code for the current bivector.</returns>
    [Pure]
    public override int GetHashCode() => XY.GetHashCode();

    /// <summary>
    /// Determines whether two <see cref="Bivector2"/> instances are equal.
    /// </summary>
    /// <param name="left">The first bivector to compare.</param>
    /// <param name="right">The second bivector to compare.</param>
    /// <returns>True if the two bivectors are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Bivector2 left, Bivector2 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Bivector2"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first bivector to compare.</param>
    /// <param name="right">The second bivector to compare.</param>
    /// <returns>True if the two bivectors are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Bivector2 left, Bivector2 right) => !(left == right);
}
