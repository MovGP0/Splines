using Splines.Extensions;

namespace Splines.Numerics;

/// <summary>A struct representing exact rational numbers</summary>
[Serializable]
public struct Rational : IComparable<Rational>, IEquatable<Rational>
{
    public static readonly Rational Zero = new(0, 1);
    public static readonly Rational One = new(1, 1);
    public static readonly Rational MaxValue = new(int.MaxValue, 1);
    public static readonly Rational MinValue = new(int.MinValue, 1);

    /// <summary>The numerator of this number</summary>
    public int Numerator
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The denominator of this number</summary>
    public int Denominator
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates an exact representation of a rational number</summary>
    /// <param name="num">The numerator of this number</param>
    /// <param name="den">The denominator of this number</param>
    public Rational(int num, int den)
    {
        switch (den)
        {
            case -1:
                (Numerator, Denominator) = (-num, -den);
                break;
            case 0: throw new DivideByZeroException("The denominator can't be 0");
            case 1:
                (Numerator, Denominator) = (num, den);
                break;
            default:
                if (num == 0)
                {
                    (Numerator, Denominator) = (0, 1);
                    break;
                }

                // ensure only the numerator carries the sign
                int sign = Mathfs.Sign(den);
                Numerator = sign * num;
                Denominator = sign * den;

                if (Numerator is -1 or 1)
                    break; // no reduction needed

                // in this case, we have to try simplifying the expression
                int gcd = Mathfs.Gcd(num, den);
                Numerator /= gcd;
                Denominator /= gcd;
                break;
        }
    }

    /// <summary>Returns the reciprocal of this number</summary>
    [Pure]
    public Rational Reciprocal => new(Denominator, Numerator);

    /// <summary>Gets a value indicating whether this rational number is an integer.</summary>
    [Pure]
    public bool IsInteger => Denominator == 1;

    /// <summary>Returns the absolute value of this number</summary>
    [Pure]
    public Rational Abs() => new(Numerator.Abs(), Denominator);

    /// <summary>Returns this number to the power of another integer <c>pow</c></summary>
    /// <param name="pow">The power to raise this number by</param>
    /// <returns>The resulting rational number</returns>
    [Pure]
    public Rational Pow(int pow)
    {
        return pow switch
        {
            <= -2 => Reciprocal.Pow(-pow),
            -1 => Reciprocal,
            0 => 1,
            1 => this,
            >= 2 => new Rational((int)Math.Pow(Numerator, pow), (int)Math.Pow(Denominator, pow))
        };
    }

    /// <summary>Returns a string representation of the rational number.</summary>
    /// <returns>A string representing the rational number.</returns>
    [Pure]
    public override string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";

    /// <summary>Returns the smaller of two rational numbers.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns>The smaller of the two rational numbers.</returns>
    [Pure]
    public static Rational Min(Rational a, Rational b) => a < b ? a : b;

    /// <summary>Returns the larger of two rational numbers.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns>The larger of the two rational numbers.</returns>
    [Pure]
    public static Rational Max(Rational a, Rational b) => a > b ? a : b;

    /// <summary>Linearly interpolates between two rational numbers, based on a value <c>t</c></summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <param name="t">The interpolation value.</param>
    /// <returns>The interpolated rational number.</returns>
    [Pure]
    public static Rational Lerp(Rational a, Rational b, Rational t) => a + t * (b - a);

    /// <summary>Calculates the normalized position of a value within a range defined by two rational numbers.</summary>
    /// <param name="a">The start of the range.</param>
    /// <param name="b">The end of the range.</param>
    /// <param name="v">The value to normalize.</param>
    /// <returns>The normalized position of the value.</returns>
    [Pure]
    public static Rational InverseLerp(Rational a, Rational b, Rational v) => (v - a) / (b - a);

    /// <summary>Returns the largest integer less than or equal to the specified rational number.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The largest integer less than or equal to the specified rational number.</returns>
    [Pure]
    public static Rational Floor(Rational r)
    {
        if (r.Numerator < 0)
            return (r.Numerator - r.Denominator + 1) / r.Denominator;
        return r.Numerator / r.Denominator;
    }

    /// <summary>Returns the smallest integer greater than or equal to the specified rational number.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The smallest integer greater than or equal to the specified rational number.</returns>
    [Pure]
    public static Rational Ceil(Rational r)
    {
        if (r.Numerator > 0)
            return (r.Numerator + r.Denominator - 1) / r.Denominator;
        return r.Numerator / r.Denominator;
    }

    /// <summary>Rounds a rational number to the nearest integer.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The rounded rational number.</returns>
    [Pure]
    public static Rational Round(Rational r)
    {
        return r.Numerator < 0 == r.Denominator < 0 ? (r.Numerator + r.Denominator / 2) / r.Denominator : (r.Numerator - r.Denominator / 2) / r.Denominator;
    }

    /// <summary>Implicitly converts an integer to a rational number.</summary>
    /// <param name="n">The integer.</param>
    /// <returns>A rational number representing the integer.</returns>
    [Pure]
    public static implicit operator Rational(int n) => new(n, 1);

    /// <summary>Explicitly converts a rational number to an integer.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The integer value of the rational number.</returns>
    /// <exception cref="ArithmeticException">Thrown when the rational number cannot be exactly represented as an integer.</exception>
    [Pure]
    public static explicit operator int(Rational r) => r.IsInteger ? r.Numerator : throw new ArithmeticException($"Rational value {r} can't be cast to an integer");

    /// <summary>Explicitly converts a rational number to a float.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The float value of the rational number.</returns>
    [Pure]
    public static explicit operator float(Rational r) => (float)r.Numerator / r.Denominator;

    /// <summary>Explicitly converts a rational number to a double.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The double value of the rational number.</returns>
    [Pure]
    public static explicit operator double(Rational r) => (double)r.Numerator / r.Denominator;

    /// <summary>Negates a rational number.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The negated rational number.</returns>
    [Pure]
    public static Rational operator -(Rational r) => checked(new(-r.Numerator, r.Denominator));

    /// <summary>Returns the same rational number.</summary>
    /// <param name="r">The rational number.</param>
    /// <returns>The same rational number.</returns>
    [Pure]
    public static Rational operator +(Rational r) => r;

    /// <summary>Adds two rational numbers.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns>The sum of the two rational numbers.</returns>
    [Pure]
    public static Rational operator +(Rational a, Rational b) => checked(new(a.Numerator * b.Denominator + a.Denominator * b.Numerator, a.Denominator * b.Denominator));

    /// <summary>Adds a rational number and an integer.</summary>
    /// <param name="a">The rational number.</param>
    /// <param name="b">The integer.</param>
    /// <returns>The sum of the rational number and the integer.</returns>
    [Pure]
    public static Rational operator +(Rational a, int b) => checked(new(a.Numerator + a.Denominator * b, a.Denominator));

    /// <summary>Adds an integer and a rational number.</summary>
    /// <param name="a">The integer.</param>
    /// <param name="b">The rational number.</param>
    /// <returns>The sum of the integer and the rational number.</returns>
    [Pure]
    public static Rational operator +(int a, Rational b) => checked(new(a * b.Denominator + b.Numerator, b.Denominator));

    /// <summary>Subtracts one rational number from another.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns>The difference between the two rational numbers.</returns>
    [Pure]
    public static Rational operator -(Rational a, Rational b) => checked(new(a.Numerator * b.Denominator - a.Denominator * b.Numerator, a.Denominator * b.Denominator));

    /// <summary>Subtracts an integer from a rational number.</summary>
    /// <param name="a">The rational number.</param>
    /// <param name="b">The integer.</param>
    /// <returns>The difference between the rational number and the integer.</returns>
    [Pure]
    public static Rational operator -(Rational a, int b) => checked(new(a.Numerator - a.Denominator * b, a.Denominator));

    /// <summary>Subtracts a rational number from an integer.</summary>
    /// <param name="a">The integer.</param>
    /// <param name="b">The rational number.</param>
    /// <returns>The difference between the integer and the rational number.</returns>
    [Pure]
    public static Rational operator -(int a, Rational b) => checked(new(a * b.Denominator - b.Numerator, b.Denominator));

    /// <summary>Multiplies two rational numbers.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns>The product of the two rational numbers.</returns>
    [Pure]
    public static Rational operator *(Rational a, Rational b) => checked(new(a.Numerator * b.Numerator, a.Denominator * b.Denominator));

    /// <summary>Multiplies a rational number by an integer.</summary>
    /// <param name="a">The rational number.</param>
    /// <param name="b">The integer.</param>
    /// <returns>The product of the rational number and the integer.</returns>
    [Pure]
    public static Rational operator *(Rational a, int b) => checked(new(a.Numerator * b, a.Denominator));

    /// <summary>Multiplies an integer by a rational number.</summary>
    /// <param name="a">The integer.</param>
    /// <param name="b">The rational number.</param>
    /// <returns>The product of the integer and the rational number.</returns>
    [Pure]
    public static Rational operator *(int a, Rational b) => checked(new(b.Numerator * a, b.Denominator));

    /// <summary>Multiplies a rational number by a float.</summary>
    /// <param name="a">The rational number.</param>
    /// <param name="b">The float.</param>
    /// <returns>The product of the rational number and the float.</returns>
    [Pure]
    public static float operator *(Rational a, float b) => (a.Numerator * b) / a.Denominator;

    /// <summary>Multiplies a float by a rational number.</summary>
    /// <param name="a">The float.</param>
    /// <param name="b">The rational number.</param>
    /// <returns>The product of the float and the rational number.</returns>
    [Pure]
    public static float operator *(float a, Rational b) => (b.Numerator * a) / b.Denominator;

    /// <summary>Divides one rational number by another.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns>The quotient of the two rational numbers.</returns>
    [Pure]
    public static Rational operator /(Rational a, Rational b) => checked(new(a.Numerator * b.Denominator, a.Denominator * b.Numerator));

    /// <summary>Divides a rational number by an integer.</summary>
    /// <param name="a">The rational number.</param>
    /// <param name="b">The integer.</param>
    /// <returns>The quotient of the rational number and the integer.</returns>
    [Pure]
    public static Rational operator /(Rational a, int b) => checked(new(a.Numerator, a.Denominator * b));

    /// <summary>Divides an integer by a rational number.</summary>
    /// <param name="a">The integer.</param>
    /// <param name="b">The rational number.</param>
    /// <returns>The quotient of the integer and the rational number.</returns>
    [Pure]
    public static Rational operator /(int a, Rational b) => checked(new(a * b.Denominator, b.Numerator));

    /// <summary>Divides a rational number by a float.</summary>
    /// <param name="a">The rational number.</param>
    /// <param name="b">The float.</param>
    /// <returns>The quotient of the rational number and the float.</returns>
    [Pure]
    public static float operator /(Rational a, float b) => a.Numerator / (a.Denominator * b);

    /// <summary>Divides a float by a rational number.</summary>
    /// <param name="a">The float.</param>
    /// <param name="b">The rational number.</param>
    /// <returns>The quotient of the float and the rational number.</returns>
    [Pure]
    public static float operator /(float a, Rational b) => (a * b.Denominator) / b.Numerator;

    /// <summary>Determines whether two rational numbers are equal.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns><c>true</c> if the rational numbers are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Rational a, Rational b) => a.CompareTo(b) == 0;

    /// <summary>Determines whether two rational numbers are not equal.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns><c>true</c> if the rational numbers are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Rational a, Rational b) => a.CompareTo(b) != 0;

    /// <summary>Determines whether the first rational number is less than the second rational number.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns><c>true</c> if the first rational number is less than the second rational number; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator <(Rational a, Rational b) => a.CompareTo(b) < 0;

    /// <summary>Determines whether the first rational number is greater than the second rational number.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns><c>true</c> if the first rational number is greater than the second rational number; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator >(Rational a, Rational b) => a.CompareTo(b) > 0;

    /// <summary>Determines whether the first rational number is less than or equal to the second rational number.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns><c>true</c> if the first rational number is less than or equal to the second rational number; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator <=(Rational a, Rational b) => a.CompareTo(b) <= 0;

    /// <summary>Determines whether the first rational number is greater than or equal to the second rational number.</summary>
    /// <param name="a">The first rational number.</param>
    /// <param name="b">The second rational number.</param>
    /// <returns><c>true</c> if the first rational number is greater than or equal to the second rational number; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator >=(Rational a, Rational b) => a.CompareTo(b) >= 0;

    /// <summary>Compares the current rational number with another rational number.</summary>
    /// <param name="other">The rational number to compare with.</param>
    /// <returns>A value indicating the relative order of the rational numbers.</returns>
    [Pure]
    public int CompareTo(Rational other) => checked((Numerator * other.Denominator).CompareTo(Denominator * other.Numerator));

    /// <summary>Determines whether the specified rational number is equal to the current rational number.</summary>
    /// <param name="other">The rational number to compare with.</param>
    /// <returns><c>true</c> if the specified rational number is equal to the current rational number; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Rational other) => Numerator == other.Numerator && Denominator == other.Denominator;

    /// <summary>Determines whether the specified object is equal to the current rational number.</summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if the specified object is equal to the current rational number; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Rational other && Equals(other);

    /// <summary>Returns the hash code for the current rational number.</summary>
    /// <returns>The hash code for the current rational number.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);
}
