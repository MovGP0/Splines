﻿using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Numerics;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>An optimized uniform 1D Cubic hermite segment, with 4 control points</summary>
[Serializable]
public partial struct HermiteCubic1D : IParamSplineSegment<Polynomial1D,Matrix4x1>
{
    Matrix4x1 pointMatrix;

    [NonSerialized]
    Polynomial1D curve;

    [NonSerialized]
    bool validCoefficients;

    /// <summary>Creates a uniform 1D Cubic hermite segment, from 4 control points</summary>
    /// <param name="p0">The starting point of the curve</param>
    /// <param name="v0">The rate of change (velocity) at the start of the curve</param>
    /// <param name="p1">The end point of the curve</param>
    /// <param name="v1">The rate of change (velocity) at the end of the curve</param>
    public HermiteCubic1D(float p0, float v0, float p1, float v1) : this(new Matrix4x1(p0, v0, p1, v1))
    {
    }

    /// <summary>Creates a uniform 1D Cubic hermite segment, from 4 control points</summary>
    /// <param name="pointMatrix">The matrix containing the control points of this spline</param>
    public HermiteCubic1D(Matrix4x1 pointMatrix) => (this.pointMatrix, curve, validCoefficients) = (pointMatrix, default, false);

    public Polynomial1D Curve
    {
        get
        {
            if (validCoefficients)
                return curve; // no need to update

            validCoefficients = true;
            return curve = new Polynomial1D(
                P0,
                V0,
                -3*P0-2*V0+3*P1-V1,
                2*P0+V0-2*P1+V1
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

    /// <summary>The starting point of the curve</summary>
    public float P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <summary>The rate of change (velocity) at the start of the curve</summary>
    public float V0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <summary>The end point of the curve</summary>
    public float P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <summary>The rate of change (velocity) at the end of the curve</summary>
    public float V1
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
                1 => V0,
                2 => P1,
                3 => V1,
                _ => throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know")
            };
        }
        set
        {
            switch (i)
            {
                case 0: P0 = value; break;
                case 1: V0 = value; break;
                case 2: P1 = value; break;
                case 3: V1 = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 3 range, and I think {i} is outside that range you know");
            }
        }
    }

    public override string ToString() => $"({pointMatrix.M0}, {pointMatrix.M1}, {pointMatrix.M2}, {pointMatrix.M3})";

    public static explicit operator BezierCubic1D(HermiteCubic1D s) =>
        new(
            s.P0,
            s.P0+1/3f*s.V0,
            s.P1-1/3f*s.V1,
            s.P1
       );

    public static explicit operator CatRomCubic1D(HermiteCubic1D s) =>
        new(
            -2*s.V0+s.P1,
            s.P0,
            s.P1,
            s.P0+2*s.V1
       );

    public static explicit operator UBSCubic1D(HermiteCubic1D s) =>
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
    public static HermiteCubic1D Lerp(HermiteCubic1D a, HermiteCubic1D b, float t) =>
        new(
            Mathfs.Lerp(a.P0, b.P0, t),
            Mathfs.Lerp(a.V0, b.V0, t),
            Mathfs.Lerp(a.P1, b.P1, t),
            Mathfs.Lerp(a.V1, b.V1, t)
       );
}
