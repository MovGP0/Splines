using System.Numerics;

namespace Splines.Numerics;

[Serializable]
public struct ComplexVector3 : IEquatable<ComplexVector3>
{
    public Complex X { [Pure] get; set; }
    public Complex Y { [Pure] get; set; }
    public Complex Z { [Pure] get; set; }

    public ComplexVector3(Complex x, Complex y, Complex z)
        => (X, Y, Z) = (x, y, z);

    public Complex this[int index]
    {
        [Pure]
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public override bool Equals(object? obj) => obj is ComplexVector3 other && Equals(other);
    public bool Equals(ComplexVector3 other) => X == other.X && Y == other.Y && Z == other.Z;
    public static bool operator ==(ComplexVector3 left, ComplexVector3 right) => left.Equals(right);
    public static bool operator !=(ComplexVector3 left, ComplexVector3 right) => !(left == right);
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static ComplexVector3 operator +(ComplexVector3 a, ComplexVector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static ComplexVector3 operator -(ComplexVector3 a, ComplexVector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static ComplexVector3 operator *(ComplexVector3 vector, Complex scalar) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
    public static ComplexVector3 operator /(ComplexVector3 vector, Complex scalar) => new(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);

    public Complex Dot(ComplexVector3 other) => X * other.X + Y * other.Y + Z * other.Z;

    public (Complex XY, Complex XZ, Complex YZ) Wedge(ComplexVector3 other) => (
        X * other.Y - Y * other.X,
        Z * other.X - X * other.Z,
        Y * other.Z - Z * other.Y
    );

    public ComplexVector3 Cross(ComplexVector3 other) => new(
        Y * other.Z - Z * other.Y,
        Z * other.X - X * other.Z,
        X * other.Y - Y * other.X
    );

    public static Complex Determinant(ComplexVector3 a, ComplexVector3 b, ComplexVector3 c) =>
        a.X * (b.Y * c.Z - b.Z * c.Y) -
        a.Y * (b.X * c.Z - b.Z * c.X) +
        a.Z * (b.X * c.Y - b.Y * c.X);
}
