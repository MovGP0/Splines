using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 4D Cubic bézier segment, with 4 control points</summary>
[Serializable]
public partial struct BezierCubic4D : IParamSplineSegment<Polynomial4D,Vector4Matrix4x1>
{
    Vector4Matrix4x1 pointMatrix;

    [NonSerialized]
    Polynomial4D curve;

    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 4D Cubic bézier segment, from 4 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
    /// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
    /// <param name="p3">The end point of the curve</param>
    public BezierCubic4D(Vector4 p0, Vector4 p1, Vector4 p2, Vector4 p3) : this(new Vector4Matrix4x1(p0, p1, p2, p3))
    {
    }

    /// <summary>Creates a uniform 4D Cubic bézier segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public BezierCubic4D(Vector4Matrix4x1 pointMatrix) => (this.pointMatrix,curve,validCoefficients) = (pointMatrix,default,false);

    public Polynomial4D Curve {
        get {
            if (validCoefficients)
                return curve; // no need to update
            validCoefficients = true;
            return curve = new Polynomial4D(
                P0,
                3*(-P0+P1),
                3*P0-6*P1+3*P2,
                -P0+3*P1-3*P2+P3
          );
        }
    }
    public Vector4Matrix4x1 PointMatrix {[MethodImpl(MethodImplOptions.AggressiveInlining)] get => pointMatrix; [MethodImpl(MethodImplOptions.AggressiveInlining)] set => _ = (pointMatrix = value, validCoefficients = false); }

    /// <summary>The starting point of the curve</summary>
    public Vector4 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The second control point of the curve, sometimes called the start tangent point</summary>
    public Vector4 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The third control point of the curve, sometimes called the end tangent point</summary>
    public Vector4 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public Vector4 P3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M3 = value, validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
    public Vector4 this[int i]
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

    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    public static explicit operator HermiteCubic4D(BezierCubic4D s) =>
        new(
            s.P0,
            3*(-s.P0+s.P1),
            s.P3,
            3*(-s.P2+s.P3)
      );

    public static explicit operator CatRomCubic4D(BezierCubic4D s) =>
        new(
            6*s.P0-6*s.P1+s.P3,
            s.P0,
            s.P3,
            s.P0-6*s.P2+6*s.P3
      );

    public static explicit operator UBSCubic4D(BezierCubic4D s) =>
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
    public static BezierCubic4D Lerp(BezierCubic4D a, BezierCubic4D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.P2.LerpUnclamped(b.P2, t),
            a.P3.LerpUnclamped(b.P3, t)
      );

    /// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
    /// <param name="t">The t-value to split at</param>
    public (BezierCubic4D pre, BezierCubic4D post) Split(float t)
    {
        Vector4 a = new Vector4(
            P0.X + (P1.X - P0.X) * t,
            P0.Y + (P1.Y - P0.Y) * t,
            P0.Z + (P1.Z - P0.Z) * t,
            P0.X + (P1.X - P0.X) * t);
        Vector4 b = new Vector4(
            P1.X + (P2.X - P1.X) * t,
            P1.Y + (P2.Y - P1.Y) * t,
            P1.Z + (P2.Z - P1.Z) * t,
            P1.X + (P2.X - P1.X) * t);
        Vector4 c = new Vector4(
            P2.X + (P3.X - P2.X) * t,
            P2.Y + (P3.Y - P2.Y) * t,
            P2.Z + (P3.Z - P2.Z) * t,
            P2.X + (P3.X - P2.X) * t);
        Vector4 d = new Vector4(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t,
            a.Z + (b.Z - a.Z) * t,
            a.X + (b.X - a.X) * t);
        Vector4 e = new Vector4(
            b.X + (c.X - b.X) * t,
            b.Y + (c.Y - b.Y) * t,
            b.Z + (c.Z - b.Z) * t,
            b.X + (c.X - b.X) * t);
        Vector4 p = new Vector4(
            d.X + (e.X - d.X) * t,
            d.Y + (e.Y - d.Y) * t,
            d.Z + (e.Z - d.Z) * t,
            d.X + (e.X - d.X) * t);
        return (new BezierCubic4D(P0, a, d, p), new BezierCubic4D(p, e, c, P3));
    }
}
