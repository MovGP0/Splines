using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Numerics;
using Splines.Unity;

namespace Splines.Splines.MultiSegmentSplines;

/// <summary>
/// Represents a Catmull-Rom spline in 4D space, with support for different endpoint modes and alpha values.
/// </summary>
public sealed partial class CatRom4DSpline
{
    /// <summary>
    /// Gets the list of nodes in the spline.
    /// </summary>
    public List<Node4> Nodes
    {
        [Pure]
        get;
    }

    // Range 0..1
    private float _alpha;

    private bool _autoCalculateKnots;

    /// <summary>
    /// Gets or sets the endpoint mode of the spline.
    /// </summary>
    public EndpointMode EndpointMode
    {
        [Pure]
        get;
        set;
    }

    [NonSerialized]
    private bool _isDirty;

    #region Properties

    private bool IncludeEndpoints
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => EndpointMode != EndpointMode.None;
    }

    /// <inheritdoc cref="NUCatRomCubic4D.Alpha"/>
    public float Alpha
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _alpha;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _isDirty = true;
            _alpha = value;
        }
    }

    /// <summary>Whether to calculate knots based on the <c>alpha</c> value</summary>
    public bool AutoCalculateKnots
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _autoCalculateKnots;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            _isDirty = true;
            _autoCalculateKnots = value;
        }
    }

    private int IndexSplineStart
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IncludeEndpoints ? 0 : 1;
    }

    private int IndexSplineEnd
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ControlPointCount - (IncludeEndpoints ? 1 : 2);
    }

    /// <summary>The number of control points in this spline</summary>
    public int ControlPointCount
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Nodes.Count;
    }

    /// <summary>The number of curves in this spline</summary>
    public int CurveCount
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => ControlPointCount - (IncludeEndpoints ? 1 : 2);
    }

    /// <summary>The knot value at the start of the spline</summary>
    public float KnotStart
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetKnot(IndexSplineStart);
    }

    /// <summary>The knot value at the end of the spline</summary>
    public float KnotEnd
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetKnot(IndexSplineEnd);
    }
    /// <summary>The knot range of the spline from start to end</summary>
    public float KnotRange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => KnotEnd - KnotStart;
    }

    /// <summary>The starting point of this spline</summary>
    public Vector4 StartPoint
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Nodes[IndexSplineStart].Position;
    }

    /// <summary>The endpoint of this spline</summary>
    public Vector4 EndPoint
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Nodes[IndexSplineEnd].Position;
    }

    #endregion

    #region Constructors

    /// <summary>Creates a cubic catmull-rom spline, given a set of control points</summary>
    /// <param name="points">The control points of the spline</param>
    /// <param name="knots">The knot values of the spline</param>
    /// <param name="endpointMode">Whether the spline should reach the endpoints, and how</param>
    public CatRom4DSpline(
        IReadOnlyCollection<Vector4> points,
        IReadOnlyCollection<float> knots,
        EndpointMode endpointMode = EndpointMode.None)
    {
        if (points == null || knots == null)
            throw new NullReferenceException($"{GetType().Name} requires non-null inputs");

        if (points.Count != knots.Count)
            throw new Exception($"{GetType().Name} points[{points.Count}] and knots[{knots.Count}] have to have the same count");

        if (points.Count < 2)
            throw new Exception($"{GetType().Name} requires at least 2 points");

        Nodes = points.Zip(knots, (p, k) => new Node4 { Position = p, Knot = k }).ToList();
        _isDirty = true;
        _autoCalculateKnots = false;
        EndpointMode = endpointMode;
    }

    /// <summary>Creates a cubic catmull-rom spline, given a set of control points</summary>
    /// <param name="points">The control points of the spline</param>
    /// <param name="alpha">The alpha parameter controls how much the length of each segment should influence the shape of the curve.
    /// A value of 0 is called a uniform catrom, and is fast to evaluate but has a tendency to overshoot.
    /// A value of 0.5 is a centripetal catrom, which follows points very tightly, and prevents cusps and loops.
    /// A value of 1 is a chordal catrom, which follows the points very smoothly with wide arcs</param>
    /// <param name="endpointMode">Whether the spline should reach the endpoints, and how</param>
    public CatRom4DSpline(
        IReadOnlyCollection<Vector4> points,
        float alpha,
        EndpointMode endpointMode = EndpointMode.None)
    {
        if (points == null)
            throw new NullReferenceException($"{GetType().Name} requires non-null points");

        if (points.Count < 2)
            throw new Exception($"{GetType().Name} requires at least 2 points");

        Nodes = points.Select(p => new Node4 { Position = p }).ToList();
        _alpha = alpha;
        _isDirty = true;
        _autoCalculateKnots = true;
        EndpointMode = endpointMode;
    }

    /// <summary>Creates a cubic catmull-rom spline, given a set of control points</summary>
    /// <param name="points">The control points of the spline</param>
    /// <param name="type">The type of catrom curve to use. This will internally determine the value of the <c>alpha</c> parameter</param>
    /// <param name="endpointMode">Whether the spline should reach the endpoints, and how</param>
    public CatRom4DSpline(
        IReadOnlyCollection<Vector4> points,
        CatRomType type,
        EndpointMode endpointMode = EndpointMode.None)
    {
        if (points == null)
            throw new NullReferenceException($"{GetType().Name} requires non-null points");

        if (points.Count < 2)
            throw new Exception($"{GetType().Name} requires at least 2 points");

        Nodes = points.Select(p => new Node4 { Position = p }).ToList();
        _alpha = type.AlphaValue();
        _isDirty = true;
        _autoCalculateKnots = true;
        EndpointMode = endpointMode;
    }

    #endregion

    #region Points & Derivatives

    /// <summary>Returns the point at parameter value <c>u</c></summary>
    /// <param name="u">The parameter space position to sample the point at</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetPoint(float u)
    {
        u = ReadyAndClampU(u);
        return GetPointInternal(GetIntervalIndexForKnotValue(u), u);
    }

    /// <summary>Returns the derivative with respect to <c>u</c> at the input parameter value</summary>
    /// <param name="u">The parameter space position to sample the derivative at</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetDerivative(float u)
    {
        u = ReadyAndClampU(u);
        return GetDerivativeInternal(GetIntervalIndexForKnotValue(u), u);
    }

    /// <summary>Returns the second derivative with respect to <c>u</c> at the input parameter value</summary>
    /// <param name="u">The parameter space position to sample the second derivative at</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetSecondDerivative(float u)
    {
        u = ReadyAndClampU(u);
        return GetSecondDerivativeInternal(GetIntervalIndexForKnotValue(u), u);
    }

    /// <summary>Returns the third derivative with respect to <c>u</c> at the input parameter value</summary>
    /// <param name="u">The parameter space position to sample the third derivative at</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetThirdDerivative(float u)
    {
        return GetThirdDerivativeInternal(GetIntervalIndexForKnotValue(ReadyAndClampU(u)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float ReadyAndClampU(float u)
    {
        ReadyKnotsAndCoefficients();
        return ClampToKnotRange(u);
    }

    #endregion

    #region Recalculations

    /// <summary>Ensures the knot vector is ready (if <c>autoCalculateKnots</c> is on) and the coefficients are up to date</summary>
    public void ReadyKnotsAndCoefficients()
    {
        if (_isDirty)
        {
            _isDirty = false;
            if (_autoCalculateKnots)
                RecalculateKnots();

            for (int i = 0; i < ControlPointCount - 1; i++)
            {
                Node4 n = Nodes[i];
                // knot parameters are local to each spline's main (second) knot
                Vector4Matrix4x1 pts = new(GetControlPoint(i - 1), n.Position, GetControlPoint(i + 1), GetControlPoint(i + 2));
                Matrix4x1 knots = new(GetKnot(i - 1) - n.Knot, 0, GetKnot(i + 1) - n.Knot, GetKnot(i + 2) - n.Knot);
                n.Curve = SplineUtils.CalculateCatRomCurve(pts, knots);
                Nodes[i] = n;
            }
        }
    }

    /// <summary>Recalculates the knot vector based on alpha and the point distances</summary>
    public void RecalculateKnots()
    {
        if (_alpha == 0)
        { // uniform catrom
            for (int i = 0; i < ControlPointCount; i++)
            {
                SetKnotInternal(i, i);
            }
        }
        else
        { // non-uniform
            SetKnotInternal(0, 0); // first knot is 0
            // todo: it's possible to cache and optimize these distance checks
            // todo: by caching distances and only recalculating the necessary ones
            for (int i = 1; i < ControlPointCount; i++)
            {
                float sqDist = (Nodes[i - 1].Position - Nodes[i].Position).SqrMagnitude();
                SetKnotInternal(i, SplineUtils.CalcCatRomKnot(Nodes[i - 1].Knot, sqDist, _alpha, true));
            }
        }
    }

    #endregion

    #region Point/Knot getter/setters

    /// <summary>Get the position of a control point by index</summary>
    /// <param name="index">The index of the point</param>
    public Vector4 GetControlPoint(int index)
    {
        if (EndpointMode == EndpointMode.Collapse)
        {
            index = index.Clamp(0, ControlPointCount - 1);
        }

        if (index == -1) // extrapolate at the ends
        {
            return Nodes[1].Position.LerpUnclamped(Nodes[0].Position, 2);
        }

        if (index == ControlPointCount)
        {
            return Nodes[ControlPointCount - 2].Position.LerpUnclamped(Nodes[ControlPointCount - 1].Position, 2);
        }

        return Nodes[index].Position;
    }

    /// <summary>Set the position of a control point by index</summary>
    /// <param name="index">The index of the knot</param>
    /// <param name="position">The position to assign to the control point</param>
    public void SetControlPoint(int index, Vector4 position)
    {
        _isDirty = true;
        Node4 n = Nodes[index];
        n.Position = position;
        Nodes[index] = n;
    }

    /// <summary>Get the value of knot by index</summary>
    /// <param name="index">The index of the knot</param>
    [Pure]
    public float GetKnot(int index)
    {
        ReadyKnotsAndCoefficients();
        if (index == -1) // extrapolate at the ends
        {
            return Mathf.Lerp(Nodes[1].Knot, Nodes[0].Knot, 2);
        }

        if (index == ControlPointCount)
        {
            return Mathf.Lerp(Nodes[ControlPointCount - 2].Knot, Nodes[ControlPointCount - 1].Knot, 2);
        }

        return Nodes[index].Knot;
    }

    /// <summary>Get the knot value at a given t-value along the whole spline</summary>
    /// <param name="t">The percentage along the spline from 0 to 1</param>
    [Pure]
    public float GetKnotValue(float t) => Mathf.Lerp(KnotStart, KnotEnd, t);

    /// <summary>Sets the given knot to a specific value</summary>
    /// <param name="index">The index of the knot to edit</param>
    /// <param name="value">The value to assign to the knot</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetKnot(int index, float value)
    {
        _isDirty = true;
        SetKnotInternal(index, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetKnotInternal(int index, float value)
    {
        Node4 n = Nodes[index];
        n.Knot = value;
        Nodes[index] = n;
    }

    #endregion

    #region Get point/derivative by curve index

    /// <summary>Returns a point along a curve by index</summary>
    /// <param name="curve">The index of the curve to sample</param>
    /// <param name="t">The fraction along this segment from 0 to 1</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetPoint(int curve, float t) => GetPointInternal(curve, RangeCheckAndGetU(curve, t));

    /// <summary>Returns the derivative with respect to <c>u</c> along a curve by index</summary>
    /// <param name="curve">The index of the curve to sample</param>
    /// <param name="t">The fraction along this segment from 0 to 1</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetDerivative(int curve, float t) => GetDerivativeInternal(curve, RangeCheckAndGetU(curve, t));

    /// <summary>Returns the second derivative with respect to <c>u</c> along a curve by index</summary>
    /// <param name="curve">The index of the curve to sample</param>
    /// <param name="t">The fraction along this segment from 0 to 1</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetSecondDerivative(int curve, float t) => GetSecondDerivativeInternal(curve, RangeCheckAndGetU(curve, t));

    /// <summary>Returns the third derivative with respect to <c>u</c> of a curve by index</summary>
    /// <param name="curve">The index of the curve to sample</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector4 GetThirdDerivative(int curve) => GetThirdDerivativeInternal(curve);

    private float RangeCheckAndGetU(int curve, float t)
    {
        if (curve < 0 || curve >= CurveCount)
            throw new IndexOutOfRangeException($"Curve index {curve} is out of the range 0 to {CurveCount - 1}");
        ReadyKnotsAndCoefficients();
        return Mathf.Lerp(Nodes[curve].Knot, Nodes[curve + 1].Knot, t);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector4 GetPointInternal(int curve, float u) => Nodes[curve].EvalPoint(u);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector4 GetDerivativeInternal(int curve, float u) => Nodes[curve].EvalDerivative(u);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector4 GetSecondDerivativeInternal(int curve, float u) => Nodes[curve].EvalSecondDerivative(u);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector4 GetThirdDerivativeInternal(int curve) => Nodes[curve].EvalThirdDerivative();

    #endregion

    #region Knot Utilities

    /// <summary>Clamps the input value <c>u</c> to the range of this spline</summary>
    /// <param name="u">The parameter space value to clamp</param>
    public float ClampToKnotRange(float u) => Mathf.Clamp(u, KnotStart, KnotEnd);

    /// <summary>Returns the index of the curve containing knot value <c>u</c></summary>
    /// <param name="u">The knot value to get the curve of</param>
    public int GetIntervalIndexForKnotValue(float u)
    {
        ReadyKnotsAndCoefficients();

        if (u <= KnotStart)
        {
            return 0;
        }

        if (u >= GetKnot(ControlPointCount - 2))
        {
            return ControlPointCount - 2; // the last knot is never used as an interval (fencepost issue |-|-|)
        }

        // todo: linear search, but, might want to use binary search if more than ~30 nodes

        for (int i = 0; i < ControlPointCount - 1; i++)
        {
            if (u < GetKnot(i + 1))
            {
                return i;
            }
        }

        throw new Exception($"Failed to get spline interval for knot value {u} in the range {KnotStart} to {KnotEnd}");
    }

    #endregion
}
