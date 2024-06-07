namespace Splines.Curves;

/// <summary>An interface representing a parametric curve of degree 3 or higher</summary>
/// <typeparam name="V">The vector type of the curve</typeparam>
public interface IParamCurve3Diff<out V> : IParamCurve2Diff<V> where V : struct
{
    /// <summary>
    /// Returns the third derivative of the curve.
    /// Loosely analogous to "jerk/jolt" (rate of change of acceleration) of the point along the curve
    /// </summary>
    V EvalThirdDerivative(float t);
}
