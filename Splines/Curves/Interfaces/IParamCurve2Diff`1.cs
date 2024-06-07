namespace Splines.Curves;

/// <summary>An interface representing a parametric curve of degree 2 or higher</summary>
/// <typeparam name="V">The vector type of the curve</typeparam>
public interface IParamCurve2Diff<out V> : IParamCurve1Diff<V> where V : struct
{
    /// <summary>
    /// Returns the second derivative at the given t-value on the curve.
    /// Loosely analogous to "acceleration" of the point along the curve
    /// </summary>
    /// <param name="t">The t-value along the curve to sample</param>
    V EvalSecondDerivative(float t);
}
