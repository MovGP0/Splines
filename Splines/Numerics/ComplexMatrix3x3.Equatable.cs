namespace Splines.Numerics;

public partial struct ComplexMatrix3x3 : IEquatable<ComplexMatrix3x3>
{
    [Pure]
    public bool Equals(ComplexMatrix3x3 other)
    {
        return M00.Equals(other.M00) && M01.Equals(other.M01) && M02.Equals(other.M02)
            && M10.Equals(other.M10) && M11.Equals(other.M11) && M12.Equals(other.M12)
            && M20.Equals(other.M20) && M21.Equals(other.M21) && M22.Equals(other.M22);
    }

    [Pure]
    public override bool Equals(object? obj) => obj is ComplexMatrix3x3 other && Equals(other);

    [Pure]
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M00);
        hash.Add(M01);
        hash.Add(M02);
        hash.Add(M10);
        hash.Add(M11);
        hash.Add(M12);
        hash.Add(M20);
        hash.Add(M21);
        hash.Add(M22);
        return hash.ToHashCode();
    }

    [Pure]
    public static bool operator ==(ComplexMatrix3x3 left, ComplexMatrix3x3 right) => left.Equals(right);

    [Pure]
    public static bool operator !=(ComplexMatrix3x3 left, ComplexMatrix3x3 right) => !left.Equals(right);
}
