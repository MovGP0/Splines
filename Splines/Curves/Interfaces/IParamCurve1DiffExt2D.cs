using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.Curves;

public static class IParamCurve1DiffExt2D
{
    /// <summary>Returns the normalized tangent direction at the given t-value on the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 EvalTangent<T>(this T curve, float t) where T : IParamCurve1Diff<Vector2> => curve.EvalDerivative(t).Normalized();

    /// <summary>Returns the normal direction at the given t-value on the curve.
    /// This normal will point to the inner arc of the current curvature</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 EvalNormal<T>(this T curve, float t) where T : IParamCurve1Diff<Vector2> => curve.EvalTangent(t).Rotate90CCW();

    /// <summary>Returns the 2D angle of the direction of the curve at the given point, in radians</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float EvalAngle<T>(this T curve, float t) where T : IParamCurve1Diff<Vector2> => Mathfs.DirToAng(curve.EvalDerivative(t));

    /// <summary>Returns the orientation at the given point t, where the X axis is tangent to the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion EvalOrientation<T>(this T curve, float t) where T : IParamCurve1Diff<Vector2> => Mathfs.DirToOrientation(curve.EvalDerivative(t));

    /// <summary>Returns the position and orientation at the given t-value on the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Pose EvalPose<T>(this T curve, float t) where T : IParamCurve1Diff<Vector2> => Mathfs.PointDirToPose(curve.Eval(t), curve.EvalTangent(t));

    /// <summary>Returns the position and orientation at the given t-value on the curve, expressed as a matrix</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix4x4 EvalMatrix<T>(this T curve, float t) where T : IParamCurve1Diff<Vector2> => Mathfs.GetMatrixFrom2DPointDir(curve.Eval(t), curve.EvalTangent(t));
}
