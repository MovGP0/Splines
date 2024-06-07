namespace Splines.Curves;

/// <summary>An interface representing a parametric curve of degree 1 or higher</summary>
/// <typeparam name="V">The vector type of the curve</typeparam>
public interface IParamCurve1Diff<out V> : IParamCurve<V> where V : struct
{
    /// <summary>
    /// Returns the derivative at the given t-value on the curve.
    /// Loosely analogous to "velocity" of the point along the curve
    /// </summary>
    /// <param name="t">The t-value along the curve to sample</param>
    V EvalDerivative(float t);
}
