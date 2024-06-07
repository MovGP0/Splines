﻿using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Numerics;

namespace Splines.Splines.NonUniformSplineSegments;

/// <summary>A non-uniform cubic catmull-rom 3D curve</summary>
[Serializable]
public struct NUCatRomCubic3D : IParamSplineSegment<Polynomial3D, Vector3Matrix4x1>
{
    #region Constructors

    /// <inheritdoc cref="NUCatRomCubic2D(Vector2Matrix4x1,Matrix4x1)"/>
    public NUCatRomCubic3D(Vector3Matrix4x1 pointMatrix, Matrix4x1 knotVector) {
        this.pointMatrix = pointMatrix;
        this.knotVector = knotVector;
        validCoefficients = false;
        curve = default;
        knotCalcMode = KnotCalcMode.Manual;
        alpha = default; // unused when using manual knots
    }

    /// <inheritdoc cref="NUCatRomCubic2D(Vector2,Vector2,Vector2,Vector2,float,float,float,float)"/>
    public NUCatRomCubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float k0, float k1, float k2, float k3)
        : this(new Vector3Matrix4x1(p0, p1, p2, p3), new Matrix4x1(k0, k1, k2, k3))
    {
    }

    /// <inheritdoc cref="NUCatRomCubic2D(Vector2,Vector2,Vector2,Vector2)"/>
    public NUCatRomCubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        : this(p0, p1, p2, p3, -1, 0, 1, 2)
    {
    }

    /// <inheritdoc cref="NUCatRomCubic2D(Vector2,Vector2,Vector2,Vector2,CatRomType,bool)"/>
    public NUCatRomCubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, CatRomType type, bool parameterizeToUnitInterval = true)
        : this(p0, p1, p2, p3, type.AlphaValue(), parameterizeToUnitInterval) {
    }

    /// <inheritdoc cref="NUCatRomCubic2D(Vector2,Vector2,Vector2,Vector2,float,bool)"/>
    public NUCatRomCubic3D(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float alpha, bool parameterizeToUnitInterval = true) {
        pointMatrix = new Vector3Matrix4x1(p0, p1, p2, p3);
        validCoefficients = false;
        curve = default;
        knotVector = default;
        knotCalcMode = parameterizeToUnitInterval ? KnotCalcMode.AutoUnitInterval : KnotCalcMode.Auto;
        this.alpha = alpha;
    }

    #endregion

    // serialized data
    Vector3Matrix4x1 pointMatrix;
    public Vector3Matrix4x1 PointMatrix {
        get => pointMatrix;
        set => _ = (pointMatrix = value, validCoefficients = false);
    }
    Matrix4x1 knotVector;
    public Matrix4x1 KnotVector {
        get {
            if (knotCalcMode != KnotCalcMode.Manual)
                ReadyCoefficients();
            return knotVector;
        }
        set => _ = (knotVector = value, validCoefficients = false);
    }

    // knot auto-calculation fields
    KnotCalcMode knotCalcMode; // knot recalculation mode
    float alpha; // alpha parameterization

    Polynomial3D curve;
    public Polynomial3D Curve {
        get {
            ReadyCoefficients();
            return curve;
        }
    }

    #region Properties

    /// <inheritdoc cref="NUCatRomCubic2D.P0"/>
    public Vector3 P0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M0;
        set => _ = (pointMatrix.M0 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.P1"/>
    public Vector3 P1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M1;
        set => _ = (pointMatrix.M1 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.P2"/>
    public Vector3 P2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M2;
        set => _ = (pointMatrix.M2 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.P3"/>
    public Vector3 P3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => pointMatrix.M3;
        set => _ = (pointMatrix.M3 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.K0"/>
    public float K0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => KnotVector.M0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (knotVector.M0 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.K1"/>
    public float K1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => KnotVector.M1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (knotVector.M1 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.K2"/>
    public float K2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => KnotVector.M2;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (knotVector.M2 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.K3"/>
    public float K3
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => KnotVector.M3;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _ = (knotVector.M3 = value, validCoefficients = false);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.Alpha"/>
    public float Alpha
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => alpha;
        set => _ = (alpha = value, validCoefficients = false);
    }

    #endregion

    // cached data to accelerate calculations
    [NonSerialized]
    bool validCoefficients; // inverted isDirty flag (can't default to true in structs)

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ReadyCoefficients()
    {
        if (validCoefficients)
        {
            return; // no need to update
        }

        validCoefficients = true;
        if (knotCalcMode != KnotCalcMode.Manual)
        {
            KnotVector = SplineUtils.CalcCatRomKnots(pointMatrix, alpha, knotCalcMode == KnotCalcMode.AutoUnitInterval);
        }

        curve = SplineUtils.CalculateCatRomCurve(pointMatrix, knotVector);
    }

    /// <inheritdoc cref="NUCatRomCubic2D.GetPointWeightAtKnotValue(int,float)"/>
    public float GetPointWeightAtKnotValue(int i, float u)
    {
        float a = Mathfs.InverseLerp(K0, K1, u);
        float b = Mathfs.InverseLerp(K1, K2, u);
        float c = Mathfs.InverseLerp(K2, K3, u);
        float d = Mathfs.InverseLerp(K0, K2, u);
        float g = Mathfs.InverseLerp(K1, K3, u);
        switch (i) {
            case 0:  return (1 - a) * (1 - b) * (1 - d);
            case 1:  return (b - 1) * (a * d - a + b * (d + g - 1) - d);
            case 2:  return -b * (b * (d + g - 1) + g * (c - 1) - d);
            case 3:  return b * c * g;
            default: throw new IndexOutOfRangeException($"Catrom point has to be either 0, 1, 2 or 3. Got: {i}");
        }
    }
}