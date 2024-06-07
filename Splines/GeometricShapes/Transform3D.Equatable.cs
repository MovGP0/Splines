namespace Splines.GeometricShapes;

public partial struct Transform3D
{
    [Pure]
    public static bool operator ==(Transform3D a, Transform3D b) => a.Equals(b);

    [Pure]
    public static bool operator !=(Transform3D a, Transform3D b) => !a.Equals(b);

    [Pure]
    public bool Equals(Transform3D other)
    {
        return origin_x == other.origin_x
               && origin_y == other.origin_y
               && origin_z == other.origin_z
               && axisX_x == other.axisX_x
               && axisX_y == other.axisX_y
               && axisX_z == other.axisX_z
               && axisY_x == other.axisY_x
               && axisY_y == other.axisY_y
               && axisY_z == other.axisY_z;
    }

    [Pure]
    public override bool Equals(object? obj) => obj is Transform3D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(origin_x, origin_y, axisX_x, axisX_y);
}
