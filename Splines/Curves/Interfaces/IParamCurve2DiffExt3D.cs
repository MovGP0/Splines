using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.GeometricAlgebra;
using Splines.GeometricShapes;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>Shared functionality for 3D parametric curves of degree 2 or higher</summary>
public static class IParamCurve2DiffExt3D
{
    /// <summary>Returns a pseudovector at the given t-value on the curve, where the magnitude is the curvature in radians per distance unit, and the direction is the axis of curvature</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bivector3 EvalCurvature<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3> => Vector3Extensions.GetCurvature(curve.EvalDerivative(t), curve.EvalSecondDerivative(t));

    /// <inheritdoc cref="IParamCurve2DiffExtensions.EvalOsculatingCircle{T}"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Circle3D EvalOsculatingCircle<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3> => Circle3D.GetOsculatingCircle(curve.Eval(t), curve.EvalDerivative(t), curve.EvalSecondDerivative(t));

    /// <summary>Returns the frenet-serret (curvature-based) normal direction at the given t-value on the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EvalArcNormal<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3> => Mathfs.GetArcNormal(curve.EvalDerivative(t), curve.EvalSecondDerivative(t));

    /// <summary>Returns the frenet-serret (curvature-based) binormal direction at the given t-value on the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EvalArcBinormal<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3> => Mathfs.GetArcBinormal(curve.EvalDerivative(t), curve.EvalSecondDerivative(t));

    /// <summary>Returns the frenet-serret (curvature-based) orientation of curve at the given point t, where the Z direction is tangent to the curve.
    /// The X axis will point to the inner arc of the current curvature</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion EvalArcOrientation<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3> => Mathfs.GetArcOrientation(curve.EvalDerivative(t), curve.EvalSecondDerivative(t));

    /// <summary>Returns the position and the frenet-serret (curvature-based) orientation of curve at the given point t, where the Z direction is tangent to the curve.
    /// The X axis will point to the inner arc of the current curvature</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Pose EvalArcPose<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3>
    {
        (Vector3 pt, Vector3 vel, Vector3 acc) = (curve.Eval(t), curve.EvalDerivative(t), curve.EvalSecondDerivative(t));
        Vector3 binormal = Vector3.Cross(vel, acc);
        return new Pose(pt, QuaternionExtensions.LookRotation(vel, binormal));
    }

    /// <summary>Returns the position and the frenet-serret (curvature-based) orientation of curve at the given point t, expressed as a matrix, where the Z direction is tangent to the curve.
    /// The X axis will point to the inner arc of the current curvature</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    [Pure]
    public static Matrix4x4 EvalArcMatrix<T>(this T curve, float t) where T : IParamCurve2Diff<Vector3> {
        Vector3 P = curve.Eval(t);
        Vector3 vel = curve.EvalDerivative(t);
        Vector3 acc = curve.EvalSecondDerivative(t);
        Vector3 Tn = vel.Normalized();
        Vector3 B = Vector3.Cross(vel, acc).Normalized();
        Vector3 N = Vector3.Cross(B, Tn);

        return new Matrix4x4(
            N.X, N.Y, N.Z, 0,
            B.X, B.Y, B.Z, 0,
            Tn.X, Tn.Y, Tn.Z, 0,
            P.X, P.Y, P.Z, 1
       );
    }
}
