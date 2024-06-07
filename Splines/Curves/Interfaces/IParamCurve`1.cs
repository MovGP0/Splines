namespace Splines.Curves;

/// <summary>An interface representing a parametric curve</summary>
/// <typeparam name="V">The vector type of the curve</typeparam>
public interface IParamCurve<out V> where V : struct
{
    /// <summary>Returns the degree of this curve. Quadratic = 2, Cubic = 3, etc</summary>
    int Degree { get; }

    /// <summary>Returns the point at the given t-value on the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    V Eval(float t);
}
