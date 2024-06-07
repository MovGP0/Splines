namespace Splines.GeometricAlgebra;

public partial struct Multivector2
{
    /// <summary>
    /// Determines whether the specified object is equal to the current multivector.
    /// </summary>
    /// <param name="obj">The object to compare with the current multivector.</param>
    /// <returns>True if the specified object is equal to the current multivector; otherwise, false.</returns>
    public override bool Equals(object? obj) => obj is Multivector2 mv && Equals(mv);

    /// <summary>
    /// Determines whether the specified multivector is equal to the current multivector.
    /// </summary>
    /// <param name="other">The multivector to compare with the current multivector.</param>
    /// <returns>True if the specified multivector is equal to the current multivector; otherwise, false.</returns>
    public bool Equals(Multivector2 other) => R == other.R && V.Equals(other.V) && B.Equals(other.B);

    /// <summary>
    /// Returns a hash code for the current multivector.
    /// </summary>
    /// <returns>A hash code for the current multivector.</returns>
    public override int GetHashCode() => HashCode.Combine(R, V, B);

    /// <summary>
    /// Determines whether two <see cref="Multivector2"/> instances are equal.
    /// </summary>
    /// <param name="left">The first multivector to compare.</param>
    /// <param name="right">The second multivector to compare.</param>
    /// <returns>True if the two multivectors are equal; otherwise, false.</returns>
    public static bool operator ==(Multivector2 left, Multivector2 right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="Multivector2"/> instances are not equal.
    /// </summary>
    /// <param name="left">The first multivector to compare.</param>
    /// <param name="right">The second multivector to compare.</param>
    /// <returns>True if the two multivectors are not equal; otherwise, false.</returns>
    public static bool operator !=(Multivector2 left, Multivector2 right) => !(left == right);
}
