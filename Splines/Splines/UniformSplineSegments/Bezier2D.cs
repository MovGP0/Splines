using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>
/// A generalized 2D bezier curve, with an arbitrary number of control points.
/// If you intend to only draw cubic bezier curves, consider using BezierCubic2D instead
/// </summary>
[Serializable]
public sealed partial class Bezier2D : IParamCurve<Vector2>
{
    /// <summary>The control points of the curve</summary>
    public Vector2[] Points { get; }

    private readonly Vector2[] _ptEvalBuffer;

    /// <summary>The number of control points in this curve</summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length;
    }

    /// <summary>Creates a general bezier curve, from any number of control points</summary>
    /// <param name="points">The control points of this curve</param>
    public Bezier2D(params Vector2[] points)
    {
        if (points is not { Length: > 1 })
        {
            throw new ArgumentException("Bézier curves require at least two points");
        }

        Points = points;
        _ptEvalBuffer = new Vector2[points.Length - 1];
    }

    /// <summary>Get or set a control point position by index</summary>
    public Vector2 this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points[i];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Points[i] = value;
    }

    /// <summary>The degree of the curve, equal to the number of control points minus 1. 2 points = degree 1 (linear), 3 points = degree 2 (quadratic), 4 points = degree 3 (cubic)</summary>
    public int Degree
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length - 1;
    }

    public Vector2 Eval(float t)
    {
        float n = Count - 1;
        for (int i = 0; i < n; i++)
        {
            _ptEvalBuffer[i] = Points[i].LerpUnclamped(Points[i + 1], t);
        }

        while (n > 1)
        {
            n--;
            for (int i = 0; i < n; i++)
            {
                _ptEvalBuffer[i] = _ptEvalBuffer[i].LerpUnclamped(_ptEvalBuffer[i + 1], t);
            }
        }

        return _ptEvalBuffer[0];
    }

    /// <summary>Returns the derivative bezier curve if possible, otherwise returns null</summary>
    public Bezier2D? Differentiate()
    {
        int n = Count - 1;
        if (n == 0)
        {
            return null; // no derivative
        }

        int d = Degree;
        Vector2[] deltaPts = new Vector2[n];
        for (int i = 0; i < n; i++)
            deltaPts[i] = d * (this[i + 1] - this[i]);
        return new Bezier2D(deltaPts);
    }
}
