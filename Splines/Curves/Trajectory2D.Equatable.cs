namespace Splines.Curves;

public partial struct Trajectory2D
{
    [Pure]
    public bool Equals(Trajectory2D other)
    {
        return Position.Equals(other.Position) &&
               Velocity.Equals(other.Velocity) &&
               Acceleration.Equals(other.Acceleration);
    }

    [Pure]
    public override bool Equals(object? obj) => obj is Trajectory2D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(Position, Velocity, Acceleration);

    [Pure]
    public static bool operator ==(Trajectory2D left, Trajectory2D right) => left.Equals(right);

    [Pure]
    public static bool operator !=(Trajectory2D left, Trajectory2D right) => !(left == right);
}
