namespace Splines.Numerics;

public partial struct ComplexMatrix2x2
{
    public static ComplexMatrix2x2 operator +(ComplexMatrix2x2 a, ComplexMatrix2x2 b)
    {
        return new ComplexMatrix2x2(
            a.M00 + b.M00,
            a.M01 + b.M01,
            a.M10 + b.M10,
            a.M11 + b.M11
        );
    }

    public static ComplexMatrix2x2 operator -(ComplexMatrix2x2 a, ComplexMatrix2x2 b)
    {
        return new ComplexMatrix2x2(
            a.M00 - b.M00,
            a.M01 - b.M01,
            a.M10 - b.M10,
            a.M11 - b.M11
        );
    }

    public static ComplexMatrix2x2 operator *(ComplexMatrix2x2 a, ComplexMatrix2x2 b)
    {
        return new ComplexMatrix2x2(
            a.M00 * b.M00 + a.M01 * b.M10,
            a.M00 * b.M01 + a.M01 * b.M11,
            a.M10 * b.M00 + a.M11 * b.M10,
            a.M10 * b.M01 + a.M11 * b.M11
        );
    }

    public static ComplexVector2 operator *(ComplexMatrix2x2 matrix, ComplexVector2 vector)
    {
        return new ComplexVector2(
            matrix.M00 * vector.X + matrix.M01 * vector.Y,
            matrix.M10 * vector.X + matrix.M11 * vector.Y
        );
    }
}
