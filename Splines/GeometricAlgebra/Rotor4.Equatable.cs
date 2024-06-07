namespace Splines.GeometricAlgebra;

public partial struct Rotor4 : IEquatable<Rotor4>
{
    public override bool Equals(object? obj) => obj is Rotor4 rotor && Equals(rotor);

    public bool Equals(Rotor4 other)
    {
        return R == other.R
               && B == other.B
               && T == other.T;
    }

    public override int GetHashCode() => HashCode.Combine(R, B, T);
    public static bool operator ==(Rotor4 a, Rotor4 b) => a.Equals(b);
    public static bool operator !=(Rotor4 a, Rotor4 b) => !(a == b);
}
