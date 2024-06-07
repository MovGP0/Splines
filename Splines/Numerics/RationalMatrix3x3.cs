using Splines.Extensions;

namespace Splines.Numerics;

/// <summary>A 3x3 matrix using exact rational number representation</summary>
public readonly struct RationalMatrix3x3 : IEquatable<RationalMatrix3x3>
{
    /// <summary>
    /// The identity matrix.
    /// </summary>
    public static readonly RationalMatrix3x3 Identity = new(1, 0, 0, 0, 1, 0, 0, 0, 1);

    /// <summary>
    /// The zero matrix.
    /// </summary>
    public static readonly RationalMatrix3x3 Zero = new(0, 0, 0, 0, 0, 0, 0, 0, 0);

    public readonly Rational M00, M01, M02;
    public readonly Rational M10, M11, M12;
    public readonly Rational M20, M21, M22;

    /// <summary>
    /// Initializes a new instance of the <see cref="RationalMatrix3x3"/> struct.
    /// </summary>
    /// <param name="m00">The element at row 0, column 0.</param>
    /// <param name="m01">The element at row 0, column 1.</param>
    /// <param name="m02">The element at row 0, column 2.</param>
    /// <param name="m10">The element at row 1, column 0.</param>
    /// <param name="m11">The element at row 1, column 1.</param>
    /// <param name="m12">The element at row 1, column 2.</param>
    /// <param name="m20">The element at row 2, column 0.</param>
    /// <param name="m21">The element at row 2, column 1.</param>
    /// <param name="m22">The element at row 2, column 2.</param>
    public RationalMatrix3x3(
        Rational m00, Rational m01, Rational m02,
        Rational m10, Rational m11, Rational m12,
        Rational m20, Rational m21, Rational m22)
    {
        (M00, M01, M02) = (m00, m01, m02);
        (M10, M11, M12) = (m10, m11, m12);
        (M20, M21, M22) = (m20, m21, m22);
    }

    /// <summary>Gets the element at the specified row and column.</summary>
    /// <param name="row">The row index (0-2).</param>
    /// <param name="column">The column index (0-2).</param>
    /// <returns>The element at the specified row and column.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the row or column index is not between 0 and 2.</exception>
    public Rational this[int row, int column]
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
                _ => throw new IndexOutOfRangeException($"Matrix row/column indices have to be from 0 to 2, got: ({row},{column})")
            };
        }
    }

    /// <summary>Returns the inverse of this matrix. Throws a division by zero exception if it's not invertible.</summary>
    [Pure]
    public RationalMatrix3x3 Inverse
    {
        get
        {
            Rational A1212 = M11 * M22 - M12 * M21;
            Rational A0212 = M10 * M22 - M12 * M20;
            Rational A0112 = M10 * M21 - M11 * M20;
            Rational det = M00 * A1212 - M01 * A0212 + M02 * A0112;

            if (det == Rational.Zero)
                throw new DivideByZeroException("The matrix is not invertible - its determinant is 0");

            return new RationalMatrix3x3(
                A1212, M02 * M21 - M01 * M22, M01 * M12 - M02 * M11,
                -A0212, M00 * M22 - M02 * M20, M10 * M02 - M00 * M12,
                A0112, M20 * M01 - M00 * M21, M00 * M11 - M10 * M01
            ) / det;
        }
    }

    /// <summary>Returns the determinant of this matrix.</summary>
    [Pure]
    public Rational Determinant
    {
        get
        {
            Rational A1212 = M11 * M22 - M12 * M21;
            Rational A0212 = M10 * M22 - M12 * M20;
            Rational A0112 = M10 * M21 - M11 * M20;
            return M00 * A1212 - M01 * A0212 + M02 * A0112;
        }
    }

    /// <summary>Returns a string representation of the matrix.</summary>
    /// <returns>A string representation of the matrix.</returns>
    [Pure]
    public override string ToString() => ToStringMatrix().ToValueTableString();

    /// <summary>Returns a 2D string array representing the matrix elements.</summary>
    /// <returns>A 2D string array representing the matrix elements.</returns>
    [Pure]
    public string[,] ToStringMatrix()
    {
        return new[,]
        {
            { M00.ToString(), M01.ToString(), M02.ToString() },
            { M10.ToString(), M11.ToString(), M12.ToString() },
            { M20.ToString(), M21.ToString(), M22.ToString() }
        };
    }

    /// <summary>Multiplies the matrix by a scalar value.</summary>
    /// <param name="c">The matrix.</param>
    /// <param name="v">The scalar value.</param>
    /// <returns>The resulting matrix after multiplication.</returns>
    [Pure]
    public static RationalMatrix3x3 operator *(RationalMatrix3x3 c, Rational v) =>
        new(c.M00 * v, c.M01 * v, c.M02 * v,
            c.M10 * v, c.M11 * v, c.M12 * v,
            c.M20 * v, c.M21 * v, c.M22 * v);

    /// <summary>Divides the matrix by a scalar value.</summary>
    /// <param name="c">The matrix.</param>
    /// <param name="v">The scalar value.</param>
    /// <returns>The resulting matrix after division.</returns>
    [Pure]
    public static RationalMatrix3x3 operator /(RationalMatrix3x3 c, Rational v) => c * v.Reciprocal;

    /// <summary>Multiplies two matrices together.</summary>
    /// <param name="a">The first matrix.</param>
    /// <param name="b">The second matrix.</param>
    /// <returns>The resulting matrix after multiplication.</returns>
    [Pure]
    public static RationalMatrix3x3 operator *(RationalMatrix3x3 a, RationalMatrix3x3 b)
    {
        Rational GetEntry(int r, int c) => a[r, 0] * b[0, c] + a[r, 1] * b[1, c] + a[r, 2] * b[2, c];

        return new RationalMatrix3x3(
            GetEntry(0, 0), GetEntry(0, 1), GetEntry(0, 2),
            GetEntry(1, 0), GetEntry(1, 1), GetEntry(1, 2),
            GetEntry(2, 0), GetEntry(2, 1), GetEntry(2, 2)
        );
    }

    /// <summary>Multiplies the matrix by a 3x1 column matrix.</summary>
    /// <param name="c">The 3x3 matrix.</param>
    /// <param name="m">The 3x1 column matrix.</param>
    /// <returns>The resulting 3x1 column matrix after multiplication.</returns>
    [Pure]
    public static Matrix3x1 operator *(RationalMatrix3x3 c, Matrix3x1 m) =>
        new(m.M0 * c.M00 + m.M1 * c.M01 + m.M2 * c.M02,
            m.M0 * c.M10 + m.M1 * c.M11 + m.M2 * c.M12,
            m.M0 * c.M20 + m.M1 * c.M21 + m.M2 * c.M22);

    /// <summary>Multiplies the matrix by a 3x1 column matrix with Vector2 values.</summary>
    /// <param name="c">The 3x3 matrix.</param>
    /// <param name="m">The 3x1 column matrix with Vector2 values.</param>
    /// <returns>The resulting 3x1 column matrix with Vector2 values after multiplication.</returns>
    [Pure]
    public static Vector2Matrix3x1 operator *(RationalMatrix3x3 c, Vector2Matrix3x1 m) => new(c * m.X, c * m.Y);

    /// <summary>Multiplies the matrix by a 3x1 column matrix with Vector3 values.</summary>
    /// <param name="c">The 3x3 matrix.</param>
    /// <param name="m">The 3x1 column matrix with Vector3 values.</param>
    /// <returns>The resulting 3x1 column matrix with Vector3 values after multiplication.</returns>
    [Pure]
    public static Vector3Matrix3x1 operator *(RationalMatrix3x3 c, Vector3Matrix3x1 m) => new(c * m.X, c * m.Y, c * m.Z);

    /// <summary>Multiplies the matrix by a 3x1 column matrix with Vector4 values.</summary>
    /// <param name="c">The 3x3 matrix.</param>
    /// <param name="m">The 3x1 column matrix with Vector4 values.</param>
    /// <returns>The resulting 3x1 column matrix with Vector4 values after multiplication.</returns>
    [Pure]
    public static Vector4Matrix3x1 operator *(RationalMatrix3x3 c, Vector4Matrix3x1 m) => new(c * m.X, c * m.Y, c * m.Z, c * m.W);

    /// <summary>Determines whether the specified object is equal to the current matrix.</summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if the specified object is equal to the current matrix; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is RationalMatrix3x3 other && Equals(other);

    /// <summary>Determines whether the specified matrix is equal to the current matrix.</summary>
    /// <param name="other">The matrix to compare with.</param>
    /// <returns><c>true</c> if the specified matrix is equal to the current matrix; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(RationalMatrix3x3 other) =>
        M00 == other.M00 && M01 == other.M01 && M02 == other.M02 &&
        M10 == other.M10 && M11 == other.M11 && M12 == other.M12 &&
        M20 == other.M20 && M21 == other.M21 && M22 == other.M22;

    /// <summary>Returns the hash code for the current matrix.</summary>
    /// <returns>The hash code for the current matrix.</returns>
    [Pure]
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M00);
        hash.Add(M01);
        hash.Add(M02);
        hash.Add(M10);
        hash.Add(M11);
        hash.Add(M12);
        hash.Add(M20);
        hash.Add(M21);
        hash.Add(M22);
        return hash.ToHashCode();
    }

    /// <summary>Determines whether two matrices are equal.</summary>
    /// <param name="a">The first matrix to compare.</param>
    /// <param name="b">The second matrix to compare.</param>
    /// <returns><c>true</c> if the matrices are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(RationalMatrix3x3 a, RationalMatrix3x3 b) => a.Equals(b);

    /// <summary>Determines whether two matrices are not equal.</summary>
    /// <param name="a">The first matrix to compare.</param>
    /// <param name="b">The second matrix to compare.</param>
    /// <returns><c>true</c> if the matrices are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(RationalMatrix3x3 a, RationalMatrix3x3 b) => !a.Equals(b);
}
