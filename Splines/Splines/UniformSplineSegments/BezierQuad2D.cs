using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 2D Quadratic bézier segment, with 3 control points</summary>
[Serializable]
public partial struct BezierQuad2D : IParamSplineSegment<Polynomial2D,Vector2Matrix3x1>
{
    Vector2Matrix3x1 pointMatrix;

    [NonSerialized]
    Polynomial2D curve;

    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 2D Quadratic bézier segment, from 3 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="p1">The middle control point of the curve, sometimes called a tangent point</param>
    /// <param name="p2">The end point of the curve</param>
    public BezierQuad2D(Vector2 p0, Vector2 p1, Vector2 p2) : this(new Vector2Matrix3x1(p0, p1, p2)){}

    /// <summary>Creates a uniform 2D Quadratic bézier segment, from 3 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public BezierQuad2D(Vector2Matrix3x1 pointMatrix) => (this.pointMatrix,curve,validCoefficients) = (pointMatrix,default,false);

    public Polynomial2D Curve
    {
        get
        {
            if (validCoefficients)
                return curve; // no need to update
            validCoefficients = true;
            return curve = new Polynomial2D(
                P0,
                2*(-P0+P1),
                P0-2*P1+P2
          );
        }
    }

    public Vector2Matrix3x1 PointMatrix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix = value, validCoefficients = false);
    }

    /// <summary>The starting point of the curve</summary>
    public Vector2 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The middle control point of the curve, sometimes called a tangent point</summary>
    public Vector2 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public Vector2 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 2</summary>
    public Vector2 this[int i]
    {
        get => i switch
        {
            0 => P0,
            1 => P1,
            2 => P2,
            _ => throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know")
        };
        set
        {
            switch (i)
            {
                case 0: P0 = value; break;
                case 1: P1 = value; break;
                case 2: P2 = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know");
            }
        }
    }

    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2})";

    /// <summary>Returns a linear blend between two bézier curves</summary>
    /// <param name="a">The first spline segment</param>
    /// <param name="b">The second spline segment</param>
    /// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
    public static BezierQuad2D Lerp(BezierQuad2D a, BezierQuad2D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.P2.LerpUnclamped(b.P2, t)
      );

    /// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
    /// <param name="t">The t-value to split at</param>
    public (BezierQuad2D pre, BezierQuad2D post) Split(float t) {
        Vector2 a = new Vector2(
            P0.X + (P1.X - P0.X) * t,
            P0.Y + (P1.Y - P0.Y) * t);
        Vector2 b = new Vector2(
            P1.X + (P2.X - P1.X) * t,
            P1.Y + (P2.Y - P1.Y) * t);
        Vector2 p = new Vector2(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t);
        return (new BezierQuad2D(P0, a, p), new BezierQuad2D(p, b, P2));
    }
}
