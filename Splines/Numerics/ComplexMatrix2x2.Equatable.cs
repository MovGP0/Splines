namespace Splines.Numerics;

public partial struct ComplexMatrix2x2 : IEquatable<ComplexMatrix2x2>
{
    public bool Equals(ComplexMatrix2x2 other)
    {
        return M00.Equals(other.M00)
            && M01.Equals(other.M01)
            && M10.Equals(other.M10)
            && M11.Equals(other.M11);
    }

    public override bool Equals(object? obj) => obj is ComplexMatrix2x2 other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(M00, M01, M10, M11);
    public static bool operator ==(ComplexMatrix2x2 left, ComplexMatrix2x2 right) => left.Equals(right);
    public static bool operator !=(ComplexMatrix2x2 left, ComplexMatrix2x2 right) => !left.Equals(right);
}
