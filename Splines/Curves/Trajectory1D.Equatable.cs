namespace Splines.Curves;

public partial struct Trajectory1D : IEquatable<Trajectory1D>
{
    [Pure]
    public bool Equals(Trajectory1D other)
    {
        return Position == other.Position &&
               Velocity == other.Velocity &&
               Acceleration == other.Acceleration;
    }

    [Pure]
    public override bool Equals(object? obj) => obj is Trajectory1D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(Position, Velocity, Acceleration);

    [Pure]
    public static bool operator ==(Trajectory1D left, Trajectory1D right) => left.Equals(right);

    [Pure]
    public static bool operator !=(Trajectory1D left, Trajectory1D right) => !(left == right);
}
