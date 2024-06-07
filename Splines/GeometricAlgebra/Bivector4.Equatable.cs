namespace Splines.GeometricAlgebra;

public partial struct Bivector4 : IEquatable<Bivector4>
{
    public bool Equals(Bivector4 other)
    {
        return XY == other.XY
            && XZ == other.XZ
            && XW == other.XW
            && YZ == other.YZ
            && YW == other.YW
            && ZW == other.ZW;
    }

    public override bool Equals(object? obj) => obj is Bivector4 other && Equals(other);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            XY,
            XZ,
            XW,
            YZ,
            YW,
            ZW);
    }

    public static bool operator ==(Bivector4 left, Bivector4 right) => left.Equals(right);

    public static bool operator !=(Bivector4 left, Bivector4 right) => !left.Equals(right);
}
