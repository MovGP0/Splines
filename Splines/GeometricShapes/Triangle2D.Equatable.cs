namespace Splines.GeometricShapes;

public partial struct Triangle2D : IEquatable<Triangle2D>
{
    [Pure]
    public static bool operator ==(Triangle2D a, Triangle2D b) => a.Equals(b);

    [Pure]
    public static bool operator !=(Triangle2D a, Triangle2D b) => !a.Equals(b);

    [Pure]
    public bool Equals(Triangle2D other) => A.Equals(other.A) && B.Equals(other.B) && C.Equals(other.C);

    [Pure]
    public override bool Equals(object? obj) => obj is Triangle2D other && Equals(other);

    [Pure]
    public override int GetHashCode() => HashCode.Combine(A, B, C);
}
