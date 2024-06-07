using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 4D Cubic catmull-rom segment, with 4 control points</summary>
[Serializable]
public partial struct CatRomCubic4D : IParamSplineSegment<Polynomial4D,Vector4Matrix4x1>
{
    Vector4Matrix4x1 pointMatrix;

    [NonSerialized]
    Polynomial4D curve;

    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 4D Cubic catmull-rom segment, from 4 control points</summary>
    /// <param name="p0">The first control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</param>
    /// <param name="p1">The second control point, and the start of the catmull-rom curve</param>
    /// <param name="p2">The third control point, and the end of the catmull-rom curve</param>
    /// <param name="p3">The last control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</param>
    public CatRomCubic4D(Vector4 p0, Vector4 p1, Vector4 p2, Vector4 p3) : this(new Vector4Matrix4x1(p0, p1, p2, p3)){}

    /// <summary>Creates a uniform 4D Cubic catmull-rom segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public CatRomCubic4D(Vector4Matrix4x1 pointMatrix) => (this.pointMatrix, curve, validCoefficients) = (pointMatrix, default, false);

    public Polynomial4D Curve
    {
        get
        {
            if (validCoefficients)
                return curve; // no need to update

            validCoefficients = true;
            return curve = new Polynomial4D(
                P1,
                (-P0+P2)/2,
                P0-5/2f*P1+2*P2-1/2f*P3,
                -(1/2f)*P0+3/2f*P1-3/2f*P2+1/2f*P3
           );
        }
    }

    public Vector4Matrix4x1 PointMatrix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix = value, validCoefficients = false);
    }

    /// <summary>The first control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</summary>
    public Vector4 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The second control point, and the start of the catmull-rom curve</summary>
    public Vector4 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The third control point, and the end of the catmull-rom curve</summary>
    public Vector4 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The last control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</summary>
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
            }
        }
    }

    [Pure]
    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    public static explicit operator BezierCubic4D(CatRomCubic4D s) =>
        new(
            s.P1,
            -(1/6f)*s.P0+s.P1+1/6f*s.P2,
            1/6f*s.P1+s.P2-1/6f*s.P3,
            s.P2
       );

    public static explicit operator HermiteCubic4D(CatRomCubic4D s) =>
        new(
            s.P1,
            (-s.P0+s.P2)/2,
            s.P2,
            (-s.P1+s.P3)/2
       );

    public static explicit operator UBSCubic4D(CatRomCubic4D s) =>
        new(
            7/6f*s.P0-2/3f*s.P1+5/6f*s.P2-1/3f*s.P3,
            -(1/3f)*s.P0+11/6f*s.P1-2/3f*s.P2+1/6f*s.P3,
            1/6f*s.P0-2/3f*s.P1+11/6f*s.P2-1/3f*s.P3,
            -(1/3f)*s.P0+5/6f*s.P1-2/3f*s.P2+7/6f*s.P3
       );

    /// <summary>Returns a linear blend between two catmull-rom curves</summary>
    /// <param name="a">The first spline segment</param>
    /// <param name="b">The second spline segment</param>
    /// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
    public static CatRomCubic4D Lerp(CatRomCubic4D a, CatRomCubic4D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.P2.LerpUnclamped(b.P2, t),
            a.P3.LerpUnclamped(b.P3, t)
       );
}
