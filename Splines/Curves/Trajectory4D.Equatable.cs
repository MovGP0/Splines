namespace Splines.Curves;

public partial struct Trajectory4D
{
    [Pure]
    public bool Equals(Trajectory4D other)
    {
        return Position.Equals(other.Position) &&
               Velocity.Equals(other.Velocity) &&
               Acceleration.Equals(other.Acceleration);
    }

    [Pure]
    public override bool Equals(object? obj) => obj is Trajectory4D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(Position, Velocity, Acceleration);

    [Pure]
    public static bool operator ==(Trajectory4D left, Trajectory4D right) => left.Equals(right);

    [Pure]
    public static bool operator !=(Trajectory4D left, Trajectory4D right) => !(left == right);
}
