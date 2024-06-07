using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Numerics;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>Shared functionality for all 3D parametric curves</summary>
public static class IParamCurveExt3D
{
    /// <inheritdoc cref="IParamCurveExt2D.GetArcLength{T}"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetArcLength<T>(this T curve, int accuracy = 8)
        where T : IParamCurve<Vector3>
        => curve.GetArcLength(FloatRange.Unit, accuracy);

    /// <inheritdoc cref="IParamCurveExt2D.GetArcLength{T}(T,FloatRange,int)"/>
    [Pure]
    public static float GetArcLength<T>(this T curve, FloatRange interval, int accuracy = 8)
        where T : IParamCurve<Vector3>
    {
        accuracy = accuracy.AtLeast(2);
        bool unit = interval == FloatRange.Unit;
        float totalDist = 0;
        Vector3 prev = curve.Eval(interval.Start);

        for (int i = 1; i < accuracy; i++)
        {
            float t = i / (accuracy - 1f);
            Vector3 p = curve.Eval(unit ? t : interval.Lerp(t));
            float dx = p.X - prev.X;
            float dy = p.Y - prev.Y;
            float dz = p.Z - prev.Z;
            totalDist += Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
            prev = p;
        }

        return totalDist;
    }
}
