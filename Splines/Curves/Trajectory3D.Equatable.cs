namespace Splines.Curves;

public partial struct Trajectory3D : IEquatable<Trajectory3D>
{
    [Pure]
    public bool Equals(Trajectory3D other)
    {
        return Position.Equals(other.Position) &&
               Velocity.Equals(other.Velocity) &&
               Acceleration.Equals(other.Acceleration);
    }

    [Pure]
    public override bool Equals(object? obj) => obj is Trajectory3D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(Position, Velocity, Acceleration);

    [Pure]
    public static bool operator ==(Trajectory3D left, Trajectory3D right) => left.Equals(right);

    [Pure]
    public static bool operator !=(Trajectory3D left, Trajectory3D right) => !(left == right);
}
