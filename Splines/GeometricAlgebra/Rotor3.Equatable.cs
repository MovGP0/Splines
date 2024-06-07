namespace Splines.GeometricAlgebra;

public partial struct Rotor3 : IEquatable<Rotor3>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current rotor.
    /// </summary>
    /// <param name="obj">The object to compare with the current rotor.</param>
    /// <returns>True if the specified object is equal to the current rotor; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Rotor3 r && Equals(r);

    /// <summary>
    /// Determines whether the specified rotor is equal to the current rotor.
    /// </summary>
    /// <param name="other">The rotor to compare with the current rotor.</param>
    /// <returns>True if the specified rotor is equal to the current rotor; otherwise, false.</returns>
    [Pure]
    public bool Equals(Rotor3 other) => R == other.R && B.Equals(other.B);

    /// <summary>
    /// Returns a hash code for the current rotor.
    /// </summary>
    /// <returns>A hash code for the current rotor.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(R, B);

    /// <summary>
    /// Determines whether two <see cref="Rotor3"/> instances are equal.
    /// </summary>
    /// <param name="left">The first rotor to compare.</param>
    /// <param name="right">The second rotor to compare.</param>
    /// <returns>True if the two rotors are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Rotor3 left, Rotor3 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Rotor3"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first rotor to compare.</param>
    /// <param name="right">The second rotor to compare.</param>
    /// <returns>True if the two rotors are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Rotor3 left, Rotor3 right) => !(left == right);
}
