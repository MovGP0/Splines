using System.Numerics;

namespace Splines.Numerics;

[Serializable]
public partial struct ComplexMatrix2x2
{
    public Complex M00 { [Pure] get; set; }
    public Complex M01 { [Pure] get; set; }
    public Complex M10 { [Pure] get; set; }
    public Complex M11 { [Pure] get; set; }

    public ComplexMatrix2x2(Complex m00, Complex m01, Complex m10, Complex m11)
        => (M00, M01, M10, M11) = (m00, m01, m10, m11);

    public Complex this[int row, int column]
    {
        [Pure]
        get
        {
            return (row, column) switch
            {
                (0, 0) => M00,
                (0, 1) => M01,
                (1, 0) => M10,
                (1, 1) => M11,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (row, column)
            {
                case (0, 0): M00 = value; break;
                case (0, 1): M01 = value; break;
                case (1, 0): M10 = value; break;
                case (1, 1): M11 = value; break;
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
                2 => M10,
                3 => M11,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: M00 = value; break;
                case 1: M01 = value; break;
                case 2: M10 = value; break;
                case 3: M11 = value; break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
