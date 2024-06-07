using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;

namespace Splines.Splines.UniformSplineSegments;

/// <summary>A generalized 3D bezier curve, with an arbitrary number of control points. If you intend to only draw cubic bezier curves, consider using BezierCubic3D instead</summary>
[Serializable]
public sealed partial class Bezier3D : IParamCurve<Vector3>
{
    /// <inheritdoc cref="Bezier2D.Points"/>
    public Vector3[] Points { get; }

    private readonly Vector3[] _ptEvalBuffer;

    /// <inheritdoc cref="Bezier2D.Count"/>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length;
    }

    /// <inheritdoc cref="Bezier2D(Vector2[])"/>
    public Bezier3D(Vector3[] points)
    {
        if (points is not { Length: > 1 })
        {
            throw new ArgumentException("Bézier curves require at least two points");
        }

        Points = points;
        _ptEvalBuffer = new Vector3[points.Length - 1];
    }

    /// <inheritdoc cref="Bezier2D.this[int]"/>
    public Vector3 this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points[i];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Points[i] = value;
    }

    #region Core IParamCurve Implementations

    /// <inheritdoc cref="Bezier2D.Degree"/>
    public int Degree
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length - 1;
    }

    public Vector3 Eval(float t)
    {
        float n = Count - 1;
        for (int i = 0; i < n; i++)
        {
            _ptEvalBuffer[i] = Points[i].LerpUnclamped(Points[i + 1], t);
        }

        while (n > 1) {
            n--;
            for (int i = 0; i < n; i++)
            {
                _ptEvalBuffer[i] = _ptEvalBuffer[i].LerpUnclamped(_ptEvalBuffer[i + 1], t);
            }
        }

        return _ptEvalBuffer[0];
    }

    #endregion

    /// <inheritdoc cref="Bezier2D.Differentiate"/>
    public Bezier3D? Differentiate()
    {
        int n = Count - 1;
        if (n == 0)
        {
            return null; // no derivative
        }

        int d = Degree;
        Vector3[] deltaPts = new Vector3[n];
        for (int i = 0; i < n; i++)
        {
            deltaPts[i] = d * (this[i + 1] - this[i]);
        }

        return new Bezier3D(deltaPts);
    }
}
