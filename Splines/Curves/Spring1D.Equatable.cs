namespace Splines.Curves;

public partial struct Spring1D : IEquatable<Spring1D>
{
    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="Spring1D"/> instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns>true if the specified object is equal to the current instance; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Spring1D other && Equals(other);

    /// <summary>
    /// Determines whether the specified <see cref="Spring1D"/> is equal to the current <see cref="Spring1D"/> instance.
    /// </summary>
    /// <param name="other">The <see cref="Spring1D"/> to compare with the current instance.</param>
    /// <returns>true if the specified <see cref="Spring1D"/> is equal to the current instance; otherwise, false.</returns>
    [Pure]
    public bool Equals(Spring1D other)
    {
        return InitialPosition == other.InitialPosition &&
               InitialVelocity == other.InitialVelocity &&
               Damping == other.Damping &&
               Stiffness == other.Stiffness &&
               Mass == other.Mass &&
               TargetPosition == other.TargetPosition;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Spring1D"/> instance.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(InitialPosition, InitialVelocity, Damping, Stiffness, Mass, TargetPosition);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Spring1D"/> are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>true if the instances are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Spring1D left, Spring1D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Spring1D"/> are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>true if the instances are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Spring1D left, Spring1D right) => !(left == right);
}
