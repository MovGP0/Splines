using System.Numerics;
using System.Runtime.CompilerServices;

namespace Splines.Curves;

/// <summary>Shared functionality for 3D parametric curves of degree 3 or higher</summary>
public static class IParamCurve3DiffExtensions
{
    /// <summary>Returns the torsion at the given t-value on the curve, in radians per distance unit</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EvalTorsion<T>(this T curve, float t)
        where T : IParamCurve3Diff<Vector3>
        => Mathfs.GetTorsion(curve.EvalDerivative(t), curve.EvalSecondDerivative(t), curve.EvalThirdDerivative(t));
}
