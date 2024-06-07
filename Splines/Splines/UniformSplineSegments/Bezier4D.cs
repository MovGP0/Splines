using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Extensions;

namespace Splines.Splines.UniformSplineSegments;

[Serializable]
public sealed partial class Bezier4D : IParamCurve<Vector4>
{
    /// <summary>Points defining the bezier curve.</summary>
    public Vector4[] Points { get; }

    private readonly Vector4[] _ptEvalBuffer;

    /// <summary>Gets the number of control points.</summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="Bezier4D"/> class.</summary>
    /// <param name="points">The control points for the bezier curve.</param>
    public Bezier4D(Vector4[] points)
    {
        if (points is not { Length: > 1 })
        {
            throw new ArgumentException("Bézier curves require at least two points");
        }

        Points = points;
        _ptEvalBuffer = new Vector4[points.Length - 1];
    }

    /// <summary>Gets or sets the control point at the specified index.</summary>
    /// <param name="i">The index of the control point.</param>
    /// <returns>The control point at the specified index.</returns>
    public Vector4 this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points[i];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Points[i] = value;
    }

    /// <summary>Gets the degree of the bezier curve.</summary>
    public int Degree
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length - 1;
    }

    /// <summary>Evaluates the bezier curve at the specified parameter value.</summary>
    /// <param name="t">The parameter value at which to evaluate the curve.</param>
    /// <returns>The point on the curve corresponding to the specified parameter value.</returns>
    public Vector4 Eval(float t)
    {
        int n = Count - 1;
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

    /// <summary>Computes the derivative of the bezier curve.</summary>
    /// <returns>A new bezier curve representing the derivative of this curve.</returns>
    public Bezier4D? Differentiate()
    {
        int n = Count - 1;
        if (n == 0)
        {
            return null; // no derivative
        }

        int d = Degree;
        Vector4[] deltaPts = new Vector4[n];
        for (int i = 0; i < n; i++)
        {
            deltaPts[i] = d * (this[i + 1] - this[i]);
        }

        return new Bezier4D(deltaPts);
    }
}
