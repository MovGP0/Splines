using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 3D Cubic hermite segment, with 4 control points</summary>
[Serializable]
public partial struct HermiteCubic3D : IParamSplineSegment<Polynomial3D,Vector3Matrix4x1>
{
    Vector3Matrix4x1 pointMatrix;

    [NonSerialized]
    Polynomial3D curve;

    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 3D Cubic hermite segment, from 4 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="v0">The rate of change (velocity) at the start of the curve</param>
    /// <param name="p1">The end point of the curve</param>
    /// <param name="v1">The rate of change (velocity) at the end of the curve</param>
    public HermiteCubic3D(Vector3 p0, Vector3 v0, Vector3 p1, Vector3 v1)
        : this(new Vector3Matrix4x1(p0, v0, p1, v1))
    {
    }

    /// <summary>Creates a uniform 3D Cubic hermite segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public HermiteCubic3D(Vector3Matrix4x1 pointMatrix)
        => (this.pointMatrix, curve, validCoefficients) = (pointMatrix, default, false);

    public Polynomial3D Curve
    {
        get
        {
            if (validCoefficients)
                return curve; // no need to update

            validCoefficients = true;
            return curve = new Polynomial3D(
                P0,
                V0,
                -3*P0-2*V0+3*P1-V1,
                2*P0+V0-2*P1+V1
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

    /// <summary>The starting point of the curve</summary>
    public Vector3 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The rate of change (velocity) at the start of the curve</summary>
    public Vector3 V0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public Vector3 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The rate of change (velocity) at the end of the curve</summary>
    public Vector3 V1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M3 = value, validCoefficients = false);
    }

    /// <summary>Get or set a control point position by index. Valid indices from 0 to 3</summary>
    public Vector3 this[ int i ]
    {
        get
        {
            return i switch
            {
                0 => P0,
                1 => V0,
                2 => P1,
                3 => V1,
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
                    V0 = value;
                    break;
                case 2:
                    P1 = value;
                    break;
                case 3:
                    V1 = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know");
            }
        }
    }

    [Pure]
    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    /// <summary>Returns this curve flattened to 2D. Effectively setting z = 0</summary>
    /// <param name="curve3D">The 3D curve to flatten to the Z plane</param>
    public static explicit operator HermiteCubic2D(HermiteCubic3D curve3D)
    {
        return new(curve3D.P0.ToVector2(), curve3D.V0.ToVector2(), curve3D.P1.ToVector2(), curve3D.V1.ToVector2());
    }

    public static explicit operator BezierCubic3D(HermiteCubic3D s)
    {
        return new(
            s.P0,
            s.P0 + 1 / 3f * s.V0,
            s.P1 - 1 / 3f * s.V1,
            s.P1
       );
    }

    public static explicit operator CatRomCubic3D(HermiteCubic3D s) =>
        new(
            -2*s.V0+s.P1,
            s.P0,
            s.P1,
            s.P0+2*s.V1
       );

    public static explicit operator UBSCubic3D(HermiteCubic3D s) =>
        new(
            -s.P0-7/3f*s.V0+2*s.P1-2/3f*s.V1,
            2*s.P0+2/3f*s.V0-s.P1+1/3f*s.V1,
            -s.P0-1/3f*s.V0+2*s.P1-2/3f*s.V1,
            2*s.P0+2/3f*s.V0-s.P1+7/3f*s.V1
       );

    /// <summary>Returns a linear blend between two hermite curves</summary>
    /// <param name="a">The first spline segment</param>
    /// <param name="b">The second spline segment</param>
    /// <param name="t">A value from 0 to 1 to blend between <c>a</c> and <c>b</c></param>
    public static HermiteCubic3D Lerp(HermiteCubic3D a, HermiteCubic3D b, float t) =>
        new(
            a.P0.LerpUnclamped(b.P0, t),
            a.V0.LerpUnclamped(b.V0, t),
            a.P1.LerpUnclamped(b.P1, t),
            a.V1.LerpUnclamped(b.V1, t)
       );
}
