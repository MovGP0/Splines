namespace Splines.GeometricAlgebra;

public partial struct Trivector3 : IEquatable<Trivector3>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current trivector.
    /// </summary>
    /// <param name="obj">The object to compare with the current trivector.</param>
    /// <returns>True if the specified object is equal to the current trivector; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Trivector3 tv && Equals(tv);

    /// <summary>
    /// Determines whether the specified trivector is equal to the current trivector.
    /// </summary>
    /// <param name="other">The trivector to compare with the current trivector.</param>
    /// <returns>True if the specified trivector is equal to the current trivector; otherwise, false.</returns>
    [Pure]
    public bool Equals(Trivector3 other) => XYZ == other.XYZ;

    /// <summary>
    /// Returns a hash code for the current trivector.
    /// </summary>
    /// <returns>A hash code for the current trivector.</returns>
    [Pure]
    public override int GetHashCode() => XYZ.GetHashCode();

    /// <summary>
    /// Determines whether two <see cref="Trivector3"/> instances are equal.
    /// </summary>
    /// <param name="left">The first trivector to compare.</param>
    /// <param name="right">The second trivector to compare.</param>
    /// <returns>True if the two trivectors are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Trivector3 left, Trivector3 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Trivector3"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first trivector to compare.</param>
    /// <param name="right">The second trivector to compare.</param>
    /// <returns>True if the two trivectors are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Trivector3 left, Trivector3 right) => !(left == right);
}
