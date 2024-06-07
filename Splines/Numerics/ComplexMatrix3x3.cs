using System.Numerics;

namespace Splines.Numerics;

[Serializable]
public partial struct ComplexMatrix3x3
{
    public Complex M00 { [Pure] get; set; }
    public Complex M01 { [Pure] get; set; }
    public Complex M02 { [Pure] get; set; }
    public Complex M10 { [Pure] get; set; }
    public Complex M11 { [Pure] get; set; }
    public Complex M12 { [Pure] get; set; }
    public Complex M20 { [Pure] get; set; }
    public Complex M21 { [Pure] get; set; }
    public Complex M22 { [Pure] get; set; }

#pragma warning disable BDX0024
    public ComplexMatrix3x3(
        Complex m00, Complex m01, Complex m02,
        Complex m10, Complex m11, Complex m12,
        Complex m20, Complex m21, Complex m22)
    {
        (M00, M01, M02) = (m00, m01, m02);
        (M10, M11, M12) = (m10, m11, m12);
        (M20, M21, M22) = (m20, m21, m22);
    }
#pragma warning restore BDX0024

    public Complex this[int row, int column]
    {
        [Pure]
        get
        {
            return (row, column) switch
            {
                (0, 0) => M00,
                (0, 1) => M01,
                (0, 2) => M02,
                (1, 0) => M10,
                (1, 1) => M11,
                (1, 2) => M12,
                (2, 0) => M20,
                (2, 1) => M21,
                (2, 2) => M22,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (row, column)
            {
                case (0, 0): M00 = value; break;
                case (0, 1): M01 = value; break;
                case (0, 2): M02 = value; break;
                case (1, 0): M10 = value; break;
                case (1, 1): M11 = value; break;
                case (1, 2): M12 = value; break;
                case (2, 0): M20 = value; break;
                case (2, 1): M21 = value; break;
                case (2, 2): M22 = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Complex this[int index]
    {
        [Pure]
        get
        {
            return index switch
            {
                0 => M00,
                1 => M01,
                2 => M02,
                3 => M10,
                4 => M11,
                5 => M12,
                6 => M20,
                7 => M21,
                8 => M22,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: M00 = value; break;
                case 1: M01 = value; break;
                case 2: M02 = value; break;
                case 3: M10 = value; break;
                case 4: M11 = value; break;
                case 5: M12 = value; break;
                case 6: M20 = value; break;
                case 7: M21 = value; break;
                case 8: M22 = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Complex Determinant() =>
        M00 * (M11 * M22 - M12 * M21) -
        M01 * (M10 * M22 - M12 * M20) +
        M02 * (M10 * M21 - M11 * M20);
}
