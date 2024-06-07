namespace Splines.GeometricAlgebra;

public partial struct Trivector4 : IEquatable<Trivector4>
{
    public bool Equals(Trivector4 other)
    {
        return XYZ == other.XYZ
            && XYW == other.XYW
            && XZW == other.XZW
            && YZW == other.YZW;
    }

    public override bool Equals(object? obj) => obj is Trivector4 other && Equals(other);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            XYZ,
            XYW,
            XZW,
            YZW);
    }

    public static bool operator ==(Trivector4 left, Trivector4 right) => left.Equals(right);

    public static bool operator !=(Trivector4 left, Trivector4 right) => !left.Equals(right);
}
