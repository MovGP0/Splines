using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.GeometricShapes;

namespace Splines.Curves;

/// <summary>Shared functionality for 2D parametric curves of degree 2 or higher</summary>
public static class IParamCurve2DiffExtensions
{
    /// <summary>Returns the signed curvature at the given t-value on the curve, in radians per distance unit (equivalent to the reciprocal radius of the osculating circle)</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EvalCurvature<T>(this T curve, float t)
        where T : IParamCurve2Diff<Vector2> => curve.EvalDerivative(t).GetCurvature(curve.EvalSecondDerivative(t));

    /// <summary>Returns the osculating circle at the given t-value in the curve, if possible. Osculating circles are defined everywhere except on inflection points, where curvature is 0</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Circle2D EvalOsculatingCircle<T>(this T curve, float t)
        where T : IParamCurve2Diff<Vector2>
        => Circle2D.GetOsculatingCircle(curve.Eval(t), curve.EvalDerivative(t), curve.EvalSecondDerivative(t));
}
