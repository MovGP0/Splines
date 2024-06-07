using System.Numerics;

namespace Splines.Numerics;

[Serializable]
public struct ComplexVector4 : IEquatable<ComplexVector4>
{
    public Complex X { [Pure] get; set; }
    public Complex Y { [Pure] get; set; }
    public Complex Z { [Pure] get; set; }
    public Complex W { [Pure] get; set; }

    public ComplexVector4(Complex x, Complex y, Complex z, Complex w)
        => (X, Y, Z, W) = (x, y, z, w);

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
                3 => W,
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
                case 3: W = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public override bool Equals(object? obj) => obj is ComplexVector4 other && Equals(other);
    public bool Equals(ComplexVector4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;
    public static bool operator ==(ComplexVector4 left, ComplexVector4 right) => left.Equals(right);
    public static bool operator !=(ComplexVector4 left, ComplexVector4 right) => !(left == right);
    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

    public static ComplexVector4 operator +(ComplexVector4 a, ComplexVector4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    public static ComplexVector4 operator -(ComplexVector4 a, ComplexVector4 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
    public static ComplexVector4 operator *(ComplexVector4 vector, Complex scalar) => new(vector.X * scalar, vector.Y * scalar, vector.Z * scalar, vector.W * scalar);
    public static ComplexVector4 operator /(ComplexVector4 vector, Complex scalar) => new(vector.X / scalar, vector.Y / scalar, vector.Z / scalar, vector.W / scalar);

    public Complex Dot(ComplexVector4 other) => X * other.X + Y * other.Y + Z * other.Z + W * other.W;

    public (Complex XY, Complex XZ, Complex XW, Complex YZ, Complex YW, Complex ZW) Wedge(ComplexVector4 other)
    {
        return (
            X * other.Y - Y * other.X,
            X * other.Z - Z * other.X,
            X * other.W - W * other.X,
            Y * other.Z - Z * other.Y,
            Y * other.W - W * other.Y,
            Z * other.W - W * other.Z
        );
    }

#pragma warning disable BDX0023
    public static Complex Determinant(ComplexVector4 a, ComplexVector4 b, ComplexVector4 c, ComplexVector4 d) =>
        a.X * new ComplexMatrix3x3(b.Y, b.Z, b.W, c.Y, c.Z, c.W, d.Y, d.Z, d.W).Determinant() -
        a.Y * new ComplexMatrix3x3(b.X, b.Z, b.W, c.X, c.Z, c.W, d.X, d.Z, d.W).Determinant() +
        a.Z * new ComplexMatrix3x3(b.X, b.Y, b.W, c.X, c.Y, c.W, d.X, d.Y, d.W).Determinant() -
        a.W * new ComplexMatrix3x3(b.X, b.Y, b.Z, c.X, c.Y, c.Z, d.X, d.Y, d.Z).Determinant();
#pragma warning restore BDX0023
}
