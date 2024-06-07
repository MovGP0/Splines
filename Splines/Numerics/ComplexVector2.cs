using System.Numerics;

namespace Splines.Numerics;

[Serializable]
public struct ComplexVector2 : IEquatable<ComplexVector2>
{
    public Complex X { [Pure] get; set; }
    public Complex Y { [Pure] get; set; }

    public ComplexVector2(Complex x, Complex y)
        => (X, Y) = (x, y);

    public Complex this[int index]
    {
        [Pure]
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public override bool Equals(object? obj) => obj is ComplexVector2 other && Equals(other);
    public bool Equals(ComplexVector2 other) => X == other.X && Y == other.Y;
    public static bool operator ==(ComplexVector2 left, ComplexVector2 right) => left.Equals(right);
    public static bool operator !=(ComplexVector2 left, ComplexVector2 right) => !(left == right);
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static ComplexVector2 operator +(ComplexVector2 a, ComplexVector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static ComplexVector2 operator -(ComplexVector2 a, ComplexVector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static ComplexVector2 operator *(ComplexVector2 vector, Complex scalar) => new(vector.X * scalar, vector.Y * scalar);
    public static ComplexVector2 operator /(ComplexVector2 vector, Complex scalar) => new(vector.X / scalar, vector.Y / scalar);

    public Complex Dot(ComplexVector2 other) => X * other.X + Y * other.Y;
    public Complex Wedge(ComplexVector2 other) => X * other.Y - Y * other.X;
    public static Complex Determinant(ComplexVector2 a, ComplexVector2 b) => a.X * b.Y - a.Y * b.X;
}
