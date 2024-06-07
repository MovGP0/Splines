namespace Splines.GeometricAlgebra;

public partial struct Multivector3 : IEquatable<Multivector3>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current multivector.
    /// </summary>
    /// <param name="obj">The object to compare with the current multivector.</param>
    /// <returns>True if the specified object is equal to the current multivector; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Multivector3 mv && Equals(mv);

    /// <summary>
    /// Determines whether the specified multivector is equal to the current multivector.
    /// </summary>
    /// <param name="other">The multivector to compare with the current multivector.</param>
    /// <returns>True if the specified multivector is equal to the current multivector; otherwise, false.</returns>
    [Pure]
    public bool Equals(Multivector3 other) => R == other.R && V.Equals(other.V) && B.Equals(other.B) && T.Equals(other.T);

    /// <summary>
    /// Returns a hash code for the current multivector.
    /// </summary>
    /// <returns>A hash code for the current multivector.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(R, V, B, T);

    /// <summary>
    /// Determines whether two <see cref="Multivector3"/> instances are equal.
    /// </summary>
    /// <param name="left">The first multivector to compare.</param>
    /// <param name="right">The second multivector to compare.</param>
    /// <returns>True if the two multivectors are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Multivector3 left, Multivector3 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Multivector3"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first multivector to compare.</param>
    /// <param name="right">The second multivector to compare.</param>
    /// <returns>True if the two multivectors are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Multivector3 left, Multivector3 right) => !(left == right);
}
