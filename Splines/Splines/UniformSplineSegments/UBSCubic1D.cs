using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 1D Cubic b-spline segment, with 4 control points</summary>
[Serializable]
public partial struct UBSCubic1D : IParamSplineSegment<Polynomial1D,Matrix4x1>
{
    private Matrix4x1 pointMatrix;

    [NonSerialized]
    private Polynomial1D curve;

    [NonSerialized]
    private bool validCoefficients;

    /// <summary>Creates a uniform 1D Cubic b-spline segment, from 4 control points</summary>
    /// <param name="p0">The first point of the B-spline hull</param>
    /// <param name="p1">The second point of the B-spline hull</param>
    /// <param name="p2">The third point of the B-spline hull</param>
    /// <param name="p3">The fourth point of the B-spline hull</param>
    public UBSCubic1D(float p0, float p1, float p2, float p3) : this(new Matrix4x1(p0, p1, p2, p3))
    {
    }

    /// <summary>Creates a uniform 1D Cubic b-spline segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public UBSCubic1D(Matrix4x1 pointMatrix) => (this.pointMatrix,curve,validCoefficients) = (pointMatrix, default, false);

    public Polynomial1D Curve
    {
        get
        {
            if (validCoefficients)
                return curve; // no need to update

            validCoefficients = true;
            return curve = new Polynomial1D(
                1/6f*P0+2/3f*P1+1/6f*P2,
                (-P0+P2)/2,
                1/2f*P0-P1+1/2f*P2,
                -(1/6f)*P0+1/2f*P1-1/2f*P2+1/6f*P3
           );
        }
    }

    public Matrix4x1 PointMatrix
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix = value, validCoefficients = false);
    }

    /// <summary>The first point of the B-spline hull</summary>
    public float P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The second point of the B-spline hull</summary>
    public float P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The third point of the B-spline hull</summary>
    public float P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The fourth point of the B-spline hull</summary>
    public float P3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M3 = value, validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
    public float this[int i]
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
            }}
    }

    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    public static explicit operator BezierCubic1D(UBSCubic1D s) =>
        new(
            1/6f*s.P0+2/3f*s.P1+1/6f*s.P2,
            2/3f*s.P1+1/3f*s.P2,
            1/3f*s.P1+2/3f*s.P2,
            1/6f*s.P1+2/3f*s.P2+1/6f*s.P3
       );
    public static explicit operator HermiteCubic1D(UBSCubic1D s) =>
        new(
            1/6f*s.P0+2/3f*s.P1+1/6f*s.P2,
            (-s.P0+s.P2)/2,
            1/6f*s.P1+2/3f*s.P2+1/6f*s.P3,
            (-s.P1+s.P3)/2
       );

    public static explicit operator CatRomCubic1D(UBSCubic1D s) =>
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
    public static UBSCubic1D Lerp(UBSCubic1D a, UBSCubic1D b, float t) =>
        new(
            Mathfs.Lerp(a.P0, b.P0, t),
            Mathfs.Lerp(a.P1, b.P1, t),
            Mathfs.Lerp(a.P2, b.P2, t),
            Mathfs.Lerp(a.P3, b.P3, t)
       );
}
