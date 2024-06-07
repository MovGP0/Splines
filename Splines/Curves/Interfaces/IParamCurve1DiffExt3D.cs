using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>Shared functionality for 3D parametric curves of degree 1 or higher</summary>
public static class IParamCurve1DiffExt3D
{
    /// <inheritdoc cref="IParamCurve1DiffExt2D.EvalTangent{T}(T,float)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EvalTangent<T>(this T curve, float t) where T : IParamCurve1Diff<Vector3> => curve.EvalDerivative(t).Normalized();

    /// <summary>Returns a normal of the curve given a reference up vector and t-value on the curve.
    /// The normal will be perpendicular to both the supplied up vector and the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    /// <param name="up">The reference up vector. The normal will be perpendicular to both the supplied up vector and the curve</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EvalNormal<T>(this T curve, float t, Vector3 up) where T : IParamCurve1Diff<Vector3> => Mathfs.GetNormalFromLookTangent(curve.EvalDerivative(t), up);

    /// <summary>Returns the binormal of the curve given a reference up vector and t-value on the curve.
    /// The binormal will attempt to be as aligned with the reference vector as possible,
    /// while still being perpendicular to the curve</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    /// <param name="up">The reference up vector. The binormal will attempt to be as aligned with the reference vector as possible, while still being perpendicular to the curve</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 EvalBinormal<T>(this T curve, float t, Vector3 up) where T : IParamCurve1Diff<Vector3> => Mathfs.GetBinormalFromLookTangent(curve.EvalDerivative(t), up);

    /// <summary>Returns the orientation at the given point t, where the Z direction is tangent to the curve.
    /// The Y axis will attempt to align with the supplied up vector</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    /// <param name="up">The reference up vector. The Y axis will attempt to be as aligned with this vector as much as possible</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quaternion EvalOrientation<T>(this T curve, float t, Vector3 up) where T : IParamCurve1Diff<Vector3> => QuaternionExtensions.LookRotation(curve.EvalDerivative(t), up);

    /// <summary>Returns the position and orientation of curve at the given point t, where the Z direction is tangent to the curve.
    /// The Y axis will attempt to align with the supplied up vector</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    /// <param name="up">The reference up vector. The Y axis will attempt to be as aligned with this vector as much as possible</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Pose EvalPose<T>(this T curve, float t, Vector3 up) where T : IParamCurve1Diff<Vector3> => new(curve.Eval(t), QuaternionExtensions.LookRotation(curve.EvalDerivative(t), up));

    /// <summary>Returns the position and orientation of curve at the given point t, expressed as a matrix, where the Z direction is tangent to the curve.
    /// The Y axis will attempt to align with the supplied up vector</summary>
    /// <param name="t">The t-value along the curve to sample</param>
    /// <param name="up">The reference up vector. The Y axis will attempt to be as aligned with this vector as much as possible</param>
    public static Matrix4x4 EvalMatrix<T>(this T curve, float t, Vector3 up) where T : IParamCurve1Diff<Vector3> {
        (Vector3 Pt, Vector3 Tn) = (curve.Eval(t), curve.EvalTangent(t));
        Vector3 Nm = Vector3.Cross(up, Tn).Normalized(); // X axis
        Vector3 Bn = Vector3.Cross(Tn, Nm); // Y axis
        return new Matrix4x4
        (
            Nm.X, Nm.Y, Nm.Z, 0,
            Bn.X, Bn.Y, Bn.Z, 0,
            Tn.X, Tn.Y, Tn.Z, 0,
            Pt.X, Pt.Y, Pt.Z, 1
       );
    }
}
