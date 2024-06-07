namespace Splines.GeometricShapes;

public partial struct Transform2D : IEquatable<Transform2D>
{
    [Pure]
    public static bool operator ==(Transform2D a, Transform2D b) => a.Equals(b);

    [Pure]
    public static bool operator !=(Transform2D a, Transform2D b) => !a.Equals(b);

    [Pure]
    public bool Equals(Transform2D other)
    {
        return origin_x == other.origin_x
               && origin_y == other.origin_y
               && axisX_x == other.axisX_x
               && axisX_y == other.axisX_y;
    }

    [Pure]
    public override bool Equals(object? obj) => obj is Transform2D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(origin_x, origin_y, axisX_x, axisX_y);
}
