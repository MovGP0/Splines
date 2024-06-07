using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>
/// An optimized uniform 2D Cubic bézier segment, with 4 control points
/// </summary>
[Serializable]
public partial struct BezierCubic2D : IParamSplineSegment<Polynomial2D,Vector2Matrix4x1>
{
    private Vector2Matrix4x1 _pointMatrix;

    [NonSerialized]
    private Polynomial2D _curve;

    [NonSerialized]
    private bool _validCoefficients;

    /// <summary>Creates a uniform 2D Cubic bézier segment, from 4 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
    /// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
    /// <param name="p3">The end point of the curve</param>
    public BezierCubic2D(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        : this(new Vector2Matrix4x1(p0, p1, p2, p3))
    {
    }

    /// <summary>Creates a uniform 2D Cubic bézier segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public BezierCubic2D(Vector2Matrix4x1 pointMatrix) => (_pointMatrix,_curve,_validCoefficients) = (pointMatrix,default,false);

    public Polynomial2D Curve
    {
        get
        {
            if (_validCoefficients)
            {
                return _curve; // no need to update
            }

            _validCoefficients = true;
            return _curve = new Polynomial2D(
                P0,
                3*(-P0+P1),
                3*P0-6*P1+3*P2,
                -P0+3*P1-3*P2+P3
            );
        }
    }

    public Vector2Matrix4x1 PointMatrix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix = value, _validCoefficients = false);
    }

    /// <summary>The starting point of the curve</summary>
    public Vector2 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M0 = value, _validCoefficients = false);
    }

    /// <summary>The second control point of the curve, sometimes called the start tangent point</summary>
    public Vector2 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M1 = value, _validCoefficients = false);
    }

    /// <summary>The third control point of the curve, sometimes called the end tangent point</summary>
    public Vector2 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M2 = value, _validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public Vector2 P3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M3 = value, _validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
    public Vector2 this[int i]
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

    public override string ToString() => $"({_pointMatrix.M0}, {_pointMatrix.M1}, {_pointMatrix.M2}, {_pointMatrix.M3})";

    /// <summary>Returns this spline segment in 3D, where z = 0</summary>
    /// <param name="curve2D">The 2D curve to cast to 3D</param>
    public static explicit operator BezierCubic3D(BezierCubic2D curve2D) => new(curve2D.P0.ToVector3(), curve2D.P1.ToVector3(), curve2D.P2.ToVector3(), curve2D.P3.ToVector3());
    public static explicit operator HermiteCubic2D(BezierCubic2D s) =>
        new(
            s.P0,
            3*(-s.P0+s.P1),
            s.P3,
            3*(-s.P2+s.P3)
      );
    public static explicit operator CatRomCubic2D(BezierCubic2D s) =>
        new(
            6*s.P0-6*s.P1+s.P3,
            s.P0,
            s.P3,
            s.P0-6*s.P2+6*s.P3
      );
    public static explicit operator UBSCubic2D(BezierCubic2D s) =>
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
    public static BezierCubic2D Lerp(BezierCubic2D a, BezierCubic2D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.P2.LerpUnclamped(b.P2, t),
            a.P3.LerpUnclamped(b.P3, t)
      );

    /// <summary>Returns a linear blend between two bézier curves, where the tangent directions are spherically interpolated</summary>
    /// <param name="a">The first spline segment</param>
    /// <param name="b">The second spline segment</param>
    /// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
    public static BezierCubic2D Slerp(BezierCubic2D a, BezierCubic2D b, float t) {
        Vector2 P0 = a.P0.LerpUnclamped(b.P0, t);
        Vector2 P3 = a.P3.LerpUnclamped(b.P3, t);
        return new BezierCubic2D(
            P0,
            P0 + (a.P1 - a.P0).ToVector3().SlerpUnclamped((b.P1 - b.P0).ToVector3(), t).ToVector2(),
            P3 + (a.P2 - a.P3).ToVector3().SlerpUnclamped((b.P2 - b.P3).ToVector3(), t).ToVector2(),
            P3
      );
    }

    /// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
    /// <param name="t">The t-value to split at</param>
    public (BezierCubic2D pre, BezierCubic2D post) Split(float t) {
        Vector2 a = new Vector2(
            P0.X + (P1.X - P0.X) * t,
            P0.Y + (P1.Y - P0.Y) * t);
        Vector2 b = new Vector2(
            P1.X + (P2.X - P1.X) * t,
            P1.Y + (P2.Y - P1.Y) * t);
        Vector2 c = new Vector2(
            P2.X + (P3.X - P2.X) * t,
            P2.Y + (P3.Y - P2.Y) * t);
        Vector2 d = new Vector2(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t);
        Vector2 e = new Vector2(
            b.X + (c.X - b.X) * t,
            b.Y + (c.Y - b.Y) * t);
        Vector2 p = new Vector2(
            d.X + (e.X - d.X) * t,
            d.Y + (e.Y - d.Y) * t);
        return (new BezierCubic2D(P0, a, d, p), new BezierCubic2D(p, e, c, P3));
    }
}
