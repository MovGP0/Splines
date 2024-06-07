using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 2D Cubic catmull-rom segment, with 4 control points</summary>
[Serializable]
public partial struct CatRomCubic2D : IParamSplineSegment<Polynomial2D,Vector2Matrix4x1>
{
    private Vector2Matrix4x1 _pointMatrix;

    [NonSerialized]
    private Polynomial2D _curve;

    [NonSerialized]
    private bool _validCoefficients;

    /// <summary>Creates a uniform 2D Cubic catmull-rom segment, from 4 control points</summary>
    /// <param name="p0">The first control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</param>
    /// <param name="p1">The second control point, and the start of the catmull-rom curve</param>
    /// <param name="p2">The third control point, and the end of the catmull-rom curve</param>
    /// <param name="p3">The last control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</param>
    public CatRomCubic2D(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3) : this(new Vector2Matrix4x1(p0, p1, p2, p3))
    {
    }

    /// <summary>Creates a uniform 2D Cubic catmull-rom segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public CatRomCubic2D(Vector2Matrix4x1 pointMatrix) => (this._pointMatrix, _curve, _validCoefficients) = (pointMatrix, default, false);

    public Polynomial2D Curve
    {
        get
        {
            if (_validCoefficients)
                return _curve; // no need to update
            _validCoefficients = true;
            return _curve = new Polynomial2D(
                P1,
                (-P0+P2)/2,
                P0-5/2f*P1+2*P2-1/2f*P3,
                -(1/2f)*P0+3/2f*P1-3/2f*P2+1/2f*P3
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

    /// <summary>The first control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</summary>
    public Vector2 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M0 = value, _validCoefficients = false);
    }

    /// <summary>The second control point, and the start of the catmull-rom curve</summary>
    public Vector2 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M1 = value, _validCoefficients = false);
    }

    /// <summary>The third control point, and the end of the catmull-rom curve</summary>
    public Vector2 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (_pointMatrix.M2 = value, _validCoefficients = false);
    }

    /// <summary>The last control point of the catmull-rom curve. Note that this point is not included in the curve itself, and only helps to shape it</summary>
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
            }
        }
    }

    public override string ToString() => $"({_pointMatrix.M0}, {_pointMatrix.M1}, {_pointMatrix.M2}, {_pointMatrix.M3})";

    /// <summary>Returns this spline segment in 3D, where z = 0</summary>
    /// <param name="curve2D">The 2D curve to cast to 3D</param>
    public static explicit operator CatRomCubic3D(CatRomCubic2D curve2D) => new(curve2D.P0.ToVector3(), curve2D.P1.ToVector3(), curve2D.P2.ToVector3(), curve2D.P3.ToVector3());
    public static explicit operator BezierCubic2D(CatRomCubic2D s) =>
        new(
            s.P1,
            -(1/6f)*s.P0+s.P1+1/6f*s.P2,
            1/6f*s.P1+s.P2-1/6f*s.P3,
            s.P2
       );

    public static explicit operator HermiteCubic2D(CatRomCubic2D s) =>
        new(
            s.P1,
            (-s.P0+s.P2)/2,
            s.P2,
            (-s.P1+s.P3)/2
       );

    public static explicit operator UBSCubic2D(CatRomCubic2D s) =>
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
    public static CatRomCubic2D Lerp(CatRomCubic2D a, CatRomCubic2D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.P2.LerpUnclamped(b.P2, t),
            a.P3.LerpUnclamped(b.P3, t)
       );
}
