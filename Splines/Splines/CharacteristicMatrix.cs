using System.Numerics;
using Splines.Curves;
using Splines.Numerics;

namespace Splines.Splines;

/// <summary>
/// Provides characteristic matrices for different types of spline curves,
/// as well as methods to manipulate and convert these matrices.
/// </summary>
public static class CharacteristicMatrix
{
    /// <summary>The characteristic matrix of a quadratic bézier curve</summary>
    public static readonly RationalMatrix3x3 QuadraticBezier = new(
        1, 0, 0,
        -2, 2, 0,
        1, -2, 1
    );

    /// <summary>The characteristic matrix of a cubic bézier curve</summary>
    public static readonly RationalMatrix4x4 CubicBezier = new(
        1, 0, 0, 0,
        -3, 3, 0, 0,
        3, -6, 3, 0,
        -1, 3, -3, 1
    );

    /// <summary>The characteristic matrix of a uniform cubic hermite curve</summary>
    public static readonly RationalMatrix4x4 CubicHermite = new(
        1, 0, 0, 0,
        0, 1, 0, 0,
        -3, -2, 3, -1,
        2, 1, -2, 1
    );

    /// <summary>The basis functions for position of a uniform cubic Hermite curve.</summary>
    public static readonly Polynomial1D[] CubicHermitePositionBasisFunctions = {
        GetBasisFunction(CubicHermite, 0),
        GetBasisFunction(CubicHermite, 2)
    };

    /// <summary>The characteristic matrix of a uniform cubic Catmull-Rom curve.</summary>
    public static readonly Polynomial1D[] CubicHermiteVelocityBasisFunctions = {
        GetBasisFunction(CubicHermite, 1),
        GetBasisFunction(CubicHermite, 3)
    };

    /// <summary>The characteristic matrix of a uniform cubic catmull-rom curve</summary>
    public static readonly RationalMatrix4x4 CubicCatmullRom = new RationalMatrix4x4(
        0, 2, 0, 0,
        -1, 0, 1, 0,
        2, -5, 4, -1,
        -1, 3, -3, 1
    ) / 2;

    /// <summary>The basis functions of a uniform cubic Catmull-Rom curve.</summary>
    public static readonly Polynomial1D[] CubicCatmullRomBasisFunctions = {
        GetBasisFunction(CubicCatmullRom, 0),
        GetBasisFunction(CubicCatmullRom, 1),
        GetBasisFunction(CubicCatmullRom, 2),
        GetBasisFunction(CubicCatmullRom, 3)
    };

    /// <summary>The characteristic matrix of a uniform cubic B-spline curve</summary>
    public static readonly RationalMatrix4x4 CubicUniformBSpline = new RationalMatrix4x4(
        1, 4, 1, 0,
        -3, 0, 3, 0,
        3, -6, 3, 0,
        -1, 3, -3, 1
   ) / 6;

    /// <summary>The inverse characteristic matrix of a quadratic Bézier curve.</summary>
    public static readonly RationalMatrix3x3 QuadraticBezierInverse = QuadraticBezier.Inverse;

    /// <summary>The inverse characteristic matrix of a cubic Bézier curve.</summary>
    public static readonly RationalMatrix4x4 CubicBezierInverse = CubicBezier.Inverse;

    /// <summary>The inverse characteristic matrix of a uniform cubic Hermite curve.</summary>
    public static readonly RationalMatrix4x4 CubicHermiteInverse = CubicHermite.Inverse;

    /// <summary>The inverse characteristic matrix of a uniform cubic Catmull-Rom curve.</summary>
    public static readonly RationalMatrix4x4 CubicCatmullRomInverse = CubicCatmullRom.Inverse;

    /// <summary>The inverse characteristic matrix of a uniform cubic B-spline curve.</summary>
    public static readonly RationalMatrix4x4 CubicUniformBSplineInverse = CubicUniformBSpline.Inverse;

    /// <summary>
    /// Returns the matrix to convert control points from one cubic spline to another, keeping the same curve intact.
    /// </summary>
    /// <param name="from">The characteristic matrix of the spline to convert from.</param>
    /// <param name="to">The characteristic matrix of the spline to convert to.</param>
    /// <returns>The conversion matrix.</returns>
    public static RationalMatrix4x4 GetConversionMatrix(RationalMatrix4x4 from, RationalMatrix4x4 to) => to.Inverse * from;

    /// <summary>
    /// Creates a 4x4 matrix from the specified elements.
    /// </summary>
    /// <param name="m00">Element at row 0, column 0.</param>
    /// <param name="m01">Element at row 0, column 1.</param>
    /// <param name="m02">Element at row 0, column 2.</param>
    /// <param name="m03">Element at row 0, column 3.</param>
    /// <param name="m10">Element at row 1, column 0.</param>
    /// <param name="m11">Element at row 1, column 1.</param>
    /// <param name="m12">Element at row 1, column 2.</param>
    /// <param name="m13">Element at row 1, column 3.</param>
    /// <param name="m20">Element at row 2, column 0.</param>
    /// <param name="m21">Element at row 2, column 1.</param>
    /// <param name="m22">Element at row 2, column 2.</param>
    /// <param name="m23">Element at row 2, column 3.</param>
    /// <param name="m30">Element at row 3, column 0.</param>
    /// <param name="m31">Element at row 3, column 1.</param>
    /// <param name="m32">Element at row 3, column 2.</param>
    /// <param name="m33">Element at row 3, column 3.</param>
    /// <returns>A 4x4 matrix.</returns>
    public static Matrix4x4 Create(
        float m00,
        float m01,
        float m02,
        float m03,
        float m10,
        float m11,
        float m12,
        float m13,
        float m20,
        float m21,
        float m22,
        float m23,
        float m30,
        float m31,
        float m32,
        float m33)
    {
        Matrix4x4 m;
        m.M11 = m00;
        m.M21 = m10;
        m.M31 = m20;
        m.M41 = m30;
        m.M12 = m01;
        m.M22 = m11;
        m.M32 = m21;
        m.M42 = m31;
        m.M13 = m02;
        m.M23 = m12;
        m.M33 = m22;
        m.M43 = m32;
        m.M14 = m03;
        m.M24 = m13;
        m.M34 = m23;
        m.M44 = m33;
        return m;
    }

    /// <summary>
    /// Returns the basis function (weight) for the given spline points by index <paramref name="i"/>,
    /// equal to the t-matrix multiplied by the characteristic matrix.
    /// </summary>
    /// <param name="c">The characteristic matrix to get the basis functions of.</param>
    /// <param name="i">The point index to get the basis function of.</param>
    /// <returns>The basis function as a polynomial.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is not between 0 and 3.</exception>
    public static Polynomial1D GetBasisFunction(RationalMatrix4x4 c, int i)
    {
        return i switch
        {
            0 => new Polynomial1D((float)c.M00, (float)c.M10, (float)c.M20, (float)c.M30),
            1 => new Polynomial1D((float)c.M01, (float)c.M11, (float)c.M21, (float)c.M31),
            2 => new Polynomial1D((float)c.M02, (float)c.M12, (float)c.M22, (float)c.M32),
            3 => new Polynomial1D((float)c.M03, (float)c.M13, (float)c.M23, (float)c.M33),
            _ => throw new IndexOutOfRangeException("Basis index needs to be between 0 and 3")
        };
    }

    /// <inheritdoc cref="GetBasisFunction(RationalMatrix4x4,int)"/>
    public static Polynomial1D GetBasisFunction(Matrix4x4 c, int i)
    {
        return i switch
        {
            0 => new Polynomial1D(c.M11, c.M21, c.M31, c.M41),
            1 => new Polynomial1D(c.M12, c.M22, c.M32, c.M42),
            2 => new Polynomial1D(c.M13, c.M23, c.M33, c.M43),
            3 => new Polynomial1D(c.M14, c.M24, c.M34, c.M44),
            _ => throw new IndexOutOfRangeException("Basis index needs to be between 0 and 3")
        };
    }
}
