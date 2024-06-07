using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 3D Cubic b-spline segment, with 4 control points</summary>
[Serializable]
public partial struct UBSCubic3D : IParamSplineSegment<Polynomial3D,Vector3Matrix4x1>
{
    Vector3Matrix4x1 pointMatrix;

    [NonSerialized]
    Polynomial3D curve;

    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 3D Cubic b-spline segment, from 4 control points</summary>
    /// <param name="p0">The first point of the B-spline hull</param>
    /// <param name="p1">The second point of the B-spline hull</param>
    /// <param name="p2">The third point of the B-spline hull</param>
    /// <param name="p3">The fourth point of the B-spline hull</param>
    public UBSCubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) : this(new Vector3Matrix4x1(p0, p1, p2, p3)){}

    /// <summary>Creates a uniform 3D Cubic b-spline segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public UBSCubic3D(Vector3Matrix4x1 pointMatrix) => (this.pointMatrix,curve,validCoefficients) = (pointMatrix, default, false);

    public Polynomial3D Curve
    {
        get
        {
            if (validCoefficients)
                return curve; // no need to update

            validCoefficients = true;
            return curve = new Polynomial3D(
                1/6f*P0+2/3f*P1+1/6f*P2,
                (-P0+P2)/2,
                1/2f*P0-P1+1/2f*P2,
                -(1/6f)*P0+1/2f*P1-1/2f*P2+1/6f*P3
           );
        }
    }

    public Vector3Matrix4x1 PointMatrix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix = value, validCoefficients = false);
    }

    /// <summary>The first point of the B-spline hull</summary>
    public Vector3 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The second point of the B-spline hull</summary>
    public Vector3 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The third point of the B-spline hull</summary>
    public Vector3 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The fourth point of the B-spline hull</summary>
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
        get
        {
            return i switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                3 => P3,
                _ => throw new ArgumentOutOfRangeException(nameof(i),
                    $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know")
            };
        }
        set
        {
            switch (i)
            {
                case 0:
                    P0 = value;
                    break;
                case 1:
                    P1 = value;
                    break;
                case 2:
                    P2 = value;
                    break;
                case 3:
                    P3 = value;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know");
            }
        }
    }

    [Pure]
    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    /// <summary>Returns this curve flattened to 2D. Effectively setting z = 0</summary>
    /// <param name="curve3D">The 3D curve to flatten to the Z plane</param>
    public static explicit operator UBSCubic2D(UBSCubic3D curve3D) => new(curve3D.P0.ToVector2(), curve3D.P1.ToVector2(), curve3D.P2.ToVector2(), curve3D.P3.ToVector2());
    public static explicit operator BezierCubic3D(UBSCubic3D s) =>
        new(
            1/6f*s.P0+2/3f*s.P1+1/6f*s.P2,
            2/3f*s.P1+1/3f*s.P2,
            1/3f*s.P1+2/3f*s.P2,
            1/6f*s.P1+2/3f*s.P2+1/6f*s.P3
       );

    public static explicit operator HermiteCubic3D(UBSCubic3D s) =>
        new(
            1/6f*s.P0+2/3f*s.P1+1/6f*s.P2,
            (-s.P0+s.P2)/2,
            1/6f*s.P1+2/3f*s.P2+1/6f*s.P3,
            (-s.P1+s.P3)/2
       );

    public static explicit operator CatRomCubic3D(UBSCubic3D s) =>
        new(
            s.P0+1/6f*s.P1-1/3f*s.P2+1/6f*s.P3,
            1/6f*s.P0+2/3f*s.P1+1/6f*s.P2,
            1/6f*s.P1+2/3f*s.P2+1/6f*s.P3,
            1/6f*s.P0-1/3f*s.P1+1/6f*s.P2+s.P3
       );

    /// <summary>Returns a linear blend between two b-spline curves</summary>
    /// <param name="a">The first spline segment</param>
    /// <param name="b">The second spline segment</param>
    /// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
    public static UBSCubic3D Lerp(UBSCubic3D a, UBSCubic3D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.P2.LerpUnclamped(b.P2, t),
            a.P3.LerpUnclamped(b.P3, t)
       );
}
