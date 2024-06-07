namespace Splines.Curves;

/// <summary>An interface representing a parametric curve of degree 3 or higher</summary>
/// <typeparam name="V">The vector type of the curve</typeparam>
public interface IParamCurve4Diff<out V> : IParamCurve3Diff<V> where V : struct
{
    /// <summary>
    /// Returns the forth derivative of the curve.
    /// Loosely analogous to "snap/jounce" (rate of change of jerk) of the point along the curve
    /// </summary>
    V EvalForthDerivative(float t);
}
