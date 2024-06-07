using System.Numerics;
using Splines.Extensions;
using Splines.Splines;

namespace Splines.Numerics;

/// <summary>A 4x4 matrix using exact rational number representation</summary>
public readonly struct RationalMatrix4x4 : IEquatable<RationalMatrix4x4>
{
    /// <summary>
    /// The identity matrix.
    /// </summary>
    public static readonly RationalMatrix4x4 Identity = new(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

    /// <summary>
    /// The zero matrix.
    /// </summary>
    public static readonly RationalMatrix4x4 Zero = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

    public readonly Rational M00, M01, M02, M03;
    public readonly Rational M10, M11, M12, M13;
    public readonly Rational M20, M21, M22, M23;
    public readonly Rational M30, M31, M32, M33;

    /// <summary>
    /// Initializes a new instance of the <see cref="RationalMatrix4x4"/> struct.
    /// </summary>
    /// <param name="m00">The element at row 0, column 0.</param>
    /// <param name="m01">The element at row 0, column 1.</param>
    /// <param name="m02">The element at row 0, column 2.</param>
    /// <param name="m03">The element at row 0, column 3.</param>
    /// <param name="m10">The element at row 1, column 0.</param>
    /// <param name="m11">The element at row 1, column 1.</param>
    /// <param name="m12">The element at row 1, column 2.</param>
    /// <param name="m13">The element at row 1, column 3.</param>
    /// <param name="m20">The element at row 2, column 0.</param>
    /// <param name="m21">The element at row 2, column 1.</param>
    /// <param name="m22">The element at row 2, column 2.</param>
    /// <param name="m23">The element at row 2, column 3.</param>
    /// <param name="m30">The element at row 3, column 0.</param>
    /// <param name="m31">The element at row 3, column 1.</param>
    /// <param name="m32">The element at row 3, column 2.</param>
    /// <param name="m33">The element at row 3, column 3.</param>
    public RationalMatrix4x4(
        Rational m00, Rational m01, Rational m02, Rational m03,
        Rational m10, Rational m11, Rational m12, Rational m13,
        Rational m20, Rational m21, Rational m22, Rational m23,
        Rational m30, Rational m31, Rational m32, Rational m33)
    {
        (M00, M01, M02, M03) = (m00, m01, m02, m03);
        (M10, M11, M12, M13) = (m10, m11, m12, m13);
        (M20, M21, M22, M23) = (m20, m21, m22, m23);
        (M30, M31, M32, M33) = (m30, m31, m32, m33);
    }

    /// <summary>Gets the element at the specified row and column.</summary>
    /// <param name="row">The row index (0-3).</param>
    /// <param name="column">The column index (0-3).</param>
    /// <returns>The element at the specified row and column.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the row or column index is not between 0 and 3.</exception>
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
                (0, 3) => M03,
                (1, 0) => M10,
                (1, 1) => M11,
                (1, 2) => M12,
                (1, 3) => M13,
                (2, 0) => M20,
                (2, 1) => M21,
                (2, 2) => M22,
                (2, 3) => M23,
                (3, 0) => M30,
                (3, 1) => M31,
                (3, 2) => M32,
                (3, 3) => M33,
                _      => throw new IndexOutOfRangeException($"Matrix row/column indices have to be from 0 to 3, got: ({row},{column})")
            };
        }
    }

    /// <summary>Returns the inverse of this matrix. Throws a division by zero exception if it's not invertible</summary>
    /// <remarks>Source: https://stackoverflow.com/questions/1148309/inverting-a-4x4-matrix</remarks>
    [Pure]
    public RationalMatrix4x4 Inverse
    {
        get
        {
            Rational A2323 = M22 * M33 - M23 * M32;
            Rational A1323 = M21 * M33 - M23 * M31;
            Rational A1223 = M21 * M32 - M22 * M31;
            Rational A0323 = M20 * M33 - M23 * M30;
            Rational A0223 = M20 * M32 - M22 * M30;
            Rational A0123 = M20 * M31 - M21 * M30;
            Rational det = M00 * (M11 * A2323 - M12 * A1323 + M13 * A1223)
                           - M01 * (M10 * A2323 - M12 * A0323 + M13 * A0223)
                           + M02 * (M10 * A1323 - M11 * A0323 + M13 * A0123)
                           - M03 * (M10 * A1223 - M11 * A0223 + M12 * A0123);

            if (det == Rational.Zero)
                throw new DivideByZeroException("The matrix is not invertible - its determinant is 0");

            Rational A2313 = M12 * M33 - M13 * M32;
            Rational A1313 = M11 * M33 - M13 * M31;
            Rational A1213 = M11 * M32 - M12 * M31;
            Rational A2312 = M12 * M23 - M13 * M22;
            Rational A1312 = M11 * M23 - M13 * M21;
            Rational A1212 = M11 * M22 - M12 * M21;
            Rational A0313 = M10 * M33 - M13 * M30;
            Rational A0213 = M10 * M32 - M12 * M30;
            Rational A0312 = M10 * M23 - M13 * M20;
            Rational A0212 = M10 * M22 - M12 * M20;
            Rational A0113 = M10 * M31 - M11 * M30;
            Rational A0112 = M10 * M21 - M11 * M20;

            return new RationalMatrix4x4(
                (M11 * A2323 - M12 * A1323 + M13 * A1223), -(M01 * A2323 - M02 * A1323 + M03 * A1223), (M01 * A2313 - M02 * A1313 + M03 * A1213), -(M01 * A2312 - M02 * A1312 + M03 * A1212),
                -(M10 * A2323 - M12 * A0323 + M13 * A0223), (M00 * A2323 - M02 * A0323 + M03 * A0223), -(M00 * A2313 - M02 * A0313 + M03 * A0213), (M00 * A2312 - M02 * A0312 + M03 * A0212),
                (M10 * A1323 - M11 * A0323 + M13 * A0123), -(M00 * A1323 - M01 * A0323 + M03 * A0123), (M00 * A1313 - M01 * A0313 + M03 * A0113), -(M00 * A1312 - M01 * A0312 + M03 * A0112),
                -(M10 * A1223 - M11 * A0223 + M12 * A0123), (M00 * A1223 - M01 * A0223 + M02 * A0123), -(M00 * A1213 - M01 * A0213 + M02 * A0113), (M00 * A1212 - M01 * A0212 + M02 * A0112)
           ) / det;
        }
    }

    /// <summary>Returns the determinant of this matrix</summary>
    /// <remarks>Source: https://stackoverflow.com/questions/1148309/inverting-a-4x4-matrix</remarks>
    [Pure]
    public Rational Determinant
    {
        get
        {
            Rational A2323 = M22 * M33 - M23 * M32;
            Rational A1323 = M21 * M33 - M23 * M31;
            Rational A1223 = M21 * M32 - M22 * M31;
            Rational A0323 = M20 * M33 - M23 * M30;
            Rational A0223 = M20 * M32 - M22 * M30;
            Rational A0123 = M20 * M31 - M21 * M30;
            return M00 * (M11 * A2323 - M12 * A1323 + M13 * A1223)
                   - M01 * (M10 * A2323 - M12 * A0323 + M13 * A0223)
                   + M02 * (M10 * A1323 - M11 * A0323 + M13 * A0123)
                   - M03 * (M10 * A1223 - M11 * A0223 + M12 * A0123);
        }
    }

    /// <summary>Returns a string representation of the matrix.</summary>
    /// <returns>A string representation of the matrix.</returns>
    public override string ToString() => ToStringMatrix().ToValueTableString();

    /// <summary>Returns a 2D string array representing the matrix elements.</summary>
    /// <returns>A 2D string array representing the matrix elements.</returns>
    [Pure]
    public string[,] ToStringMatrix()
    {
        return new[,]
        {
            { M00.ToString(), M01.ToString(), M02.ToString(), M03.ToString() },
            { M10.ToString(), M11.ToString(), M12.ToString(), M13.ToString() },
            { M20.ToString(), M21.ToString(), M22.ToString(), M23.ToString() },
            { M30.ToString(), M31.ToString(), M32.ToString(), M33.ToString() }
        };
    }

    /// <summary>Converts the <see cref="RationalMatrix4x4"/> to a <see cref="Matrix4x4"/>.</summary>
    /// <param name="c">The rational matrix.</param>
    /// <returns>A <see cref="Matrix4x4"/> representation of the rational matrix.</returns>
    [Pure]
    public static explicit operator Matrix4x4(RationalMatrix4x4 c)
    {
#pragma warning disable BDX0023
        return CharacteristicMatrix.Create(
            (float)c.M00, (float)c.M01, (float)c.M02, (float)c.M03,
            (float)c.M10, (float)c.M11, (float)c.M12, (float)c.M13,
            (float)c.M20, (float)c.M21, (float)c.M22, (float)c.M23,
            (float)c.M30, (float)c.M31, (float)c.M32, (float)c.M33
       );
#pragma warning restore BDX0023
    }

    /// <summary>Multiplies the matrix by a scalar value.</summary>
    /// <param name="c">The matrix.</param>
    /// <param name="v">The scalar value.</param>
    /// <returns>The resulting matrix after multiplication.</returns>
    [Pure]
    public static RationalMatrix4x4 operator *(RationalMatrix4x4 c, Rational v) =>
        new(c.M00 * v, c.M01 * v, c.M02 * v, c.M03 * v,
            c.M10 * v, c.M11 * v, c.M12 * v, c.M13 * v,
            c.M20 * v, c.M21 * v, c.M22 * v, c.M23 * v,
            c.M30 * v, c.M31 * v, c.M32 * v, c.M33 * v);

    /// <summary>Converts the <see cref="RationalMatrix3x3"/> to a <see cref="RationalMatrix4x4"/>.</summary>
    /// <param name="c">The 3x3 rational matrix.</param>
    /// <returns>A 4x4 rational matrix with the 3x3 matrix in the upper left.</returns>
    [Pure]
    public static explicit operator RationalMatrix4x4(RationalMatrix3x3 c) =>
        new(c.M00, c.M01, c.M02, 0,
            c.M10, c.M11, c.M12, 0,
            c.M20, c.M21, c.M22, 0,
            0, 0, 0, 1);

    /// <summary>Divides the matrix by a scalar value.</summary>
    /// <param name="c">The matrix.</param>
    /// <param name="v">The scalar value.</param>
    /// <returns>The resulting matrix after division.</returns>
    [Pure]
    public static RationalMatrix4x4 operator /(RationalMatrix4x4 c, Rational v) => c * v.Reciprocal;

    /// <summary>Multiplies two matrices together.</summary>
    /// <param name="a">The first matrix.</param>
    /// <param name="b">The second matrix.</param>
    /// <returns>The resulting matrix after multiplication.</returns>
    [Pure]
    public static RationalMatrix4x4 operator *(RationalMatrix4x4 a, RationalMatrix4x4 b) {
        Rational GetEntry(int r, int c) => a[r, 0] * b[0, c] + a[r, 1] * b[1, c] + a[r, 2] * b[2, c] + a[r, 3] * b[3, c];

        return new RationalMatrix4x4(
            GetEntry(0, 0), GetEntry(0, 1), GetEntry(0, 2), GetEntry(0, 3),
            GetEntry(1, 0), GetEntry(1, 1), GetEntry(1, 2), GetEntry(1, 3),
            GetEntry(2, 0), GetEntry(2, 1), GetEntry(2, 2), GetEntry(2, 3),
            GetEntry(3, 0), GetEntry(3, 1), GetEntry(3, 2), GetEntry(3, 3)
       );
    }

    /// <summary>Multiplies this matrix C by a column matrix M.</summary>
    /// <param name="c">The left-hand side 4x4 matrix.</param>
    /// <param name="m">The right-hand side 4x1 matrix.</param>
    /// <returns>The resulting 4x1 column matrix after multiplication.</returns>
    [Pure]
    public static Matrix4x1 operator *(RationalMatrix4x4 c, Matrix4x1 m) =>
        new(m.M0 * c.M00 + m.M1 * c.M01 + m.M2 * c.M02 + m.M3 * c.M03,
            m.M0 * c.M10 + m.M1 * c.M11 + m.M2 * c.M12 + m.M3 * c.M13,
            m.M0 * c.M20 + m.M1 * c.M21 + m.M2 * c.M22 + m.M3 * c.M23,
            m.M0 * c.M30 + m.M1 * c.M31 + m.M2 * c.M32 + m.M3 * c.M33);

    /// <summary>Multiplies this matrix C by a 4x1 column matrix with Vector2 values.</summary>
    /// <param name="c">The 4x4 matrix.</param>
    /// <param name="m">The 4x1 column matrix with Vector2 values.</param>
    /// <returns>The resulting 4x1 column matrix with Vector2 values after multiplication.</returns>
    [Pure]
    public static Vector2Matrix4x1 operator *(RationalMatrix4x4 c, Vector2Matrix4x1 m) => new(c * m.X, c * m.Y);

    /// <summary>Multiplies this matrix C by a 4x1 column matrix with Vector3 values.</summary>
    /// <param name="c">The 4x4 matrix.</param>
    /// <param name="m">The 4x1 column matrix with Vector3 values.</param>
    /// <returns>The resulting 4x1 column matrix with Vector3 values after multiplication.</returns>
    [Pure]
    public static Vector3Matrix4x1 operator *(RationalMatrix4x4 c, Vector3Matrix4x1 m) => new(c * m.X, c * m.Y, c * m.Z);

    /// <summary>Multiplies this matrix C by a 4x1 column matrix with Vector4 values.</summary>
    /// <param name="c">The 4x4 matrix.</param>
    /// <param name="m">The 4x1 column matrix with Vector4 values.</param>
    /// <returns>The resulting 4x1 column matrix with Vector4 values after multiplication.</returns>
    [Pure]
    public static Vector4Matrix4x1 operator *(RationalMatrix4x4 c, Vector4Matrix4x1 m) => new(c * m.X, c * m.Y, c * m.Z, c * m.W);

    /// <summary>Determines whether the specified object is equal to the current matrix.</summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if the specified object is equal to the current matrix; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is RationalMatrix4x4 other && Equals(other);

    /// <summary>Determines whether the specified matrix is equal to the current matrix.</summary>
    /// <param name="other">The matrix to compare with.</param>
    /// <returns><c>true</c> if the specified matrix is equal to the current matrix; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(RationalMatrix4x4 other) =>
        M00 == other.M00 && M01 == other.M01 && M02 == other.M02 && M03 == other.M03 &&
        M10 == other.M10 && M11 == other.M11 && M12 == other.M12 && M13 == other.M13 &&
        M20 == other.M20 && M21 == other.M21 && M22 == other.M22 && M23 == other.M23 &&
        M30 == other.M30 && M31 == other.M31 && M32 == other.M32 && M33 == other.M33;

    /// <summary>Returns the hash code for the current matrix.</summary>
    /// <returns>The hash code for the current matrix.</returns>
    [Pure]
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(M00);
        hash.Add(M01);
        hash.Add(M02);
        hash.Add(M03);
        hash.Add(M10);
        hash.Add(M11);
        hash.Add(M12);
        hash.Add(M13);
        hash.Add(M20);
        hash.Add(M21);
        hash.Add(M22);
        hash.Add(M23);
        hash.Add(M30);
        hash.Add(M31);
        hash.Add(M32);
        hash.Add(M33);
        return hash.ToHashCode();
    }

    /// <summary>Determines whether two matrices are equal.</summary>
    /// <param name="a">The first matrix to compare.</param>
    /// <param name="b">The second matrix to compare.</param>
    /// <returns><c>true</c> if the matrices are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(RationalMatrix4x4 a, RationalMatrix4x4 b) => a.Equals(b);

    /// <summary>Determines whether two matrices are not equal.</summary>
    /// <param name="a">The first matrix to compare.</param>
    /// <param name="b">The second matrix to compare.</param>
    /// <returns><c>true</c> if the matrices are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(RationalMatrix4x4 a, RationalMatrix4x4 b) => !a.Equals(b);
}
