using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 1D Cubic bézier segment, with 4 control points</summary>
[Serializable]
public partial struct BezierCubic1D : IParamSplineSegment<Polynomial1D,Matrix4x1>
{
    private Matrix4x1 _pointMatrix;

    [NonSerialized]
    private Polynomial1D _curve;

    [NonSerialized]
    private bool _validCoefficients;

    /// <summary>Creates a uniform 1D Cubic bézier segment, from 4 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
    /// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
    /// <param name="p3">The end point of the curve</param>
    public BezierCubic1D(float p0, float p1, float p2, float p3) : this(new Matrix4x1(p0, p1, p2, p3))
    {
    }

    /// <summary>Creates a uniform 1D Cubic bézier segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public BezierCubic1D(Matrix4x1 pointMatrix) => (this._pointMatrix, _curve, _validCoefficients) = (pointMatrix, default, false);

    public Polynomial1D Curve
    {
        get
        {
            if (_validCoefficients)
                return _curve; // no need to update

            _validCoefficients = true;
            return _curve = new Polynomial1D(
                P0,
                3*(-P0+P1),
                3*P0-6*P1+3*P2,
                -P0+3*P1-3*P2+P3
           );
        }
    }

    public Matrix4x1 PointMatrix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix = value, _validCoefficients = false);
    }

    /// <summary>The starting point of the curve</summary>
    public float P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M0 = value, _validCoefficients = false);
    }

    /// <summary>The second control point of the curve, sometimes called the start tangent point</summary>
    public float P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M1 = value, _validCoefficients = false);
    }

    /// <summary>The third control point of the curve, sometimes called the end tangent point</summary>
    public float P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M2 = value, _validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public float P3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M3 = value, _validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
    public float this[int i]
    {
        get => i switch
        {
            0 => P0,
            1 => P1,
            2 => P2,
            3 => P3,
            _ => throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know")
        };
        set
        {
            switch (i)
            {
                case 0: P0 = value; break;
                case 1: P1 = value; break;
                case 2: P2 = value; break;
                case 3: P3 = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know");
            }}
    }

    public static explicit operator HermiteCubic1D(BezierCubic1D s) =>
        new(
            s.P0,
            3*(-s.P0+s.P1),
            s.P3,
            3*(-s.P2+s.P3)
       );

    public static explicit operator CatRomCubic1D(BezierCubic1D s) =>
        new(
            6*s.P0-6*s.P1+s.P3,
            s.P0,
            s.P3,
            s.P0-6*s.P2+6*s.P3
       );

    public static explicit operator UBSCubic1D(BezierCubic1D s) =>
        new(
            6*s.P0-7*s.P1+2*s.P2,
            2*s.P1-s.P2,
            -s.P1+2*s.P2,
            2*s.P1-7*s.P2+6*s.P3
       );

    /// <summary>Returns a linear blend between two bézier curves</summary>
    /// <param name="a">The first spline segment</param>
    /// <param name="b">The second spline segment</param>
    /// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
    public static BezierCubic1D Lerp(BezierCubic1D a, BezierCubic1D b, float t) =>
        new(
            Mathfs.Lerp(a.P0, b.P0, t),
            Mathfs.Lerp(a.P1, b.P1, t),
            Mathfs.Lerp(a.P2, b.P2, t),
            Mathfs.Lerp(a.P3, b.P3, t)
       );

    /// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
    /// <param name="t">The t-value to split at</param>
    public (BezierCubic1D pre, BezierCubic1D post) Split(float t)
    {
        float a = P0 + (P1 - P0) * t;
        float b = P1 + (P2 - P1) * t;
        float c = P2 + (P3 - P2) * t;
        float d = a + (b - a) * t;
        float e = b + (c - b) * t;
        float p = d + (e - d) * t;
        return (new BezierCubic1D(P0, a, d, p), new BezierCubic1D(p, e, c, P3));
    }

    /// <summary>
    /// Returns a string that represents the current <see cref="BezierCubic1D"/>.
    /// </summary>
    /// <returns>A string that represents the current <see cref="BezierCubic1D"/>.</returns>
    public override string ToString() => $"({_pointMatrix.M0}, {_pointMatrix.M1}, {_pointMatrix.M2}, {_pointMatrix.M3})";
}
