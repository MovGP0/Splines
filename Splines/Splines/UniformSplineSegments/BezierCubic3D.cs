using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 3D Cubic bézier segment, with 4 control points</summary>
[Serializable]
public partial struct BezierCubic3D : IParamSplineSegment<Polynomial3D,Vector3Matrix4x1>
{
    Vector3Matrix4x1 pointMatrix;
    [NonSerialized]
    Polynomial3D curve;
    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 3D Cubic bézier segment, from 4 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="p1">The second control point of the curve, sometimes called the start tangent point</param>
    /// <param name="p2">The third control point of the curve, sometimes called the end tangent point</param>
    /// <param name="p3">The end point of the curve</param>
    public BezierCubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) : this(new Vector3Matrix4x1(p0, p1, p2, p3)){}

    /// <summary>Creates a uniform 3D Cubic bézier segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public BezierCubic3D(Vector3Matrix4x1 pointMatrix) => (this.pointMatrix,curve,validCoefficients) = (pointMatrix,default,false);

    public Polynomial3D Curve {
        get {
            if (validCoefficients)
                return curve; // no need to update
            validCoefficients = true;
            return curve = new Polynomial3D(
                P0,
                3*(-P0+P1),
                3*P0-6*P1+3*P2,
                -P0+3*P1-3*P2+P3
          );
        }
    }
    public Vector3Matrix4x1 PointMatrix {[MethodImpl(MethodImplOptions.AggressiveInlining)] get => pointMatrix; [MethodImpl(MethodImplOptions.AggressiveInlining)] set => _ = (pointMatrix = value, validCoefficients = false); }

    /// <summary>The starting point of the curve</summary>
    public Vector3 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The second control point of the curve, sometimes called the start tangent point</summary>
    public Vector3 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The third control point of the curve, sometimes called the end tangent point</summary>
    public Vector3 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public Vector3 P3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M3 = value, validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
    public Vector3 this[int i]
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
            }
        }
    }

    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    /// <summary>Returns this curve flattened to 2D. Effectively setting z = 0</summary>
    /// <param name="curve3D">The 3D curve to flatten to the Z plane</param>
    public static explicit operator BezierCubic2D(BezierCubic3D curve3D) => new(curve3D.P0.ToVector2(), curve3D.P1.ToVector2(), curve3D.P2.ToVector2(), curve3D.P3.ToVector2());
    public static explicit operator HermiteCubic3D(BezierCubic3D s) =>
        new(
            s.P0,
            3*(-s.P0+s.P1),
            s.P3,
            3*(-s.P2+s.P3)
      );
    public static explicit operator CatRomCubic3D(BezierCubic3D s) =>
        new(
            6*s.P0-6*s.P1+s.P3,
            s.P0,
            s.P3,
            s.P0-6*s.P2+6*s.P3
      );
    public static explicit operator UBSCubic3D(BezierCubic3D s) =>
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
    public static BezierCubic3D Lerp(BezierCubic3D a, BezierCubic3D b, float t) =>
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
    public static BezierCubic3D Slerp(BezierCubic3D a, BezierCubic3D b, float t) {
        Vector3 P0 = a.P0.LerpUnclamped(b.P0, t);
        Vector3 P3 = a.P3.LerpUnclamped(b.P3, t);
        return new BezierCubic3D(
            P0,
            P0 + (a.P1 - a.P0).SlerpUnclamped(b.P1 - b.P0, t),
            P3 + (a.P2 - a.P3).SlerpUnclamped(b.P2 - b.P3, t),
            P3
      );
    }

    /// <summary>Splits this curve at the given t-value, into two curves that together form the exact same shape</summary>
    /// <param name="t">The t-value to split at</param>
    public (BezierCubic3D pre, BezierCubic3D post) Split(float t) {
        Vector3 a = new Vector3(
            P0.X + (P1.X - P0.X) * t,
            P0.Y + (P1.Y - P0.Y) * t,
            P0.Z + (P1.Z - P0.Z) * t);
        Vector3 b = new Vector3(
            P1.X + (P2.X - P1.X) * t,
            P1.Y + (P2.Y - P1.Y) * t,
            P1.Z + (P2.Z - P1.Z) * t);
        Vector3 c = new Vector3(
            P2.X + (P3.X - P2.X) * t,
            P2.Y + (P3.Y - P2.Y) * t,
            P2.Z + (P3.Z - P2.Z) * t);
        Vector3 d = new Vector3(
            a.X + (b.X - a.X) * t,
            a.Y + (b.Y - a.Y) * t,
            a.Z + (b.Z - a.Z) * t);
        Vector3 e = new Vector3(
            b.X + (c.X - b.X) * t,
            b.Y + (c.Y - b.Y) * t,
            b.Z + (c.Z - b.Z) * t);
        Vector3 p = new Vector3(
            d.X + (e.X - d.X) * t,
            d.Y + (e.Y - d.Y) * t,
            d.Z + (e.Z - d.Z) * t);
        return (new BezierCubic3D(P0, a, d, p), new BezierCubic3D(p, e, c, P3));
    }
}
