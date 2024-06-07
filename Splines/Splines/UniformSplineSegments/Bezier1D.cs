using System.Runtime.CompilerServices;
using Splines.Curves;

namespace Splines.Splines.UniformSplineSegments;

[Serializable]
public sealed partial class Bezier1D : IParamCurve<float>
{
    /// <summary>Points defining the Bezier curve.</summary>
    public float[] Points { get; }

    private readonly float[] _ptEvalBuffer;

    /// <summary>Gets the number of control points.</summary>
    public int Count
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length;
    }

    /// <summary>Initializes a new instance of the <see cref="Bezier1D"/> class.</summary>
    /// <param name="points">The control points for the Bezier curve.</param>
    public Bezier1D(float[] points)
    {
        if (points is not { Length: > 1 })
        {
            throw new ArgumentException("Bézier curves require at least two points");
        }

        Points = points;
        _ptEvalBuffer = new float[points.Length - 1];
    }

    /// <summary>Gets or sets the control point at the specified index.</summary>
    /// <param name="i">The index of the control point.</param>
    /// <returns>The control point at the specified index.</returns>
    public float this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points[i];
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Points[i] = value;
    }

    /// <summary>Gets the degree of the Bezier curve.</summary>
    public int Degree
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Points.Length - 1;
    }

    /// <summary>Evaluates the Bezier curve at the specified parameter value.</summary>
    /// <param name="t">The parameter value at which to evaluate the curve.</param>
    /// <returns>The point on the curve corresponding to the specified parameter value.</returns>
    public float Eval(float t)
    {
        int n = Count - 1;
        for (int i = 0; i < n; i++)
        {
            _ptEvalBuffer[i] = Lerp(Points[i], Points[i + 1], t);
        }

        while (n > 1)
        {
            n--;
            for (int i = 0; i < n; i++)
            {
                _ptEvalBuffer[i] = Lerp(_ptEvalBuffer[i], _ptEvalBuffer[i + 1], t);
            }
        }

        return _ptEvalBuffer[0];
    }

    /// <summary>Computes the derivative of the Bezier curve.</summary>
    /// <returns>A new Bezier curve representing the derivative of this curve.</returns>
    public Bezier1D? Differentiate()
    {
        int n = Count - 1;
        if (n == 0)
        {
            return null; // no derivative
        }

        int d = Degree;
        float[] deltaPts = new float[n];
        for (int i = 0; i < n; i++)
        {
            deltaPts[i] = d * (this[i + 1] - this[i]);
        }

        return new Bezier1D(deltaPts);
    }

    /// <summary>Linear interpolation between two values.</summary>
    /// <param name="a">The start value.</param>
    /// <param name="b">The end value.</param>
    /// <param name="t">The interpolation parameter.</param>
    /// <returns>The interpolated value.</returns>
    private static float Lerp(float a, float b, float t) => a + t * (b - a);
}
