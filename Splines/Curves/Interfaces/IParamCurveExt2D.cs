using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Numerics;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>Shared functionality for all 2D parametric curves</summary>
public static class IParamCurveExt2D
{
    /// <summary>Returns the approximate length of the curve in the 0 to 1 interval</summary>
    /// <param name="accuracy">The number of subdivisions to approximate the length with. Higher values are more accurate, but more expensive to calculate</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetArcLength<T>(this T curve, int accuracy = 8)
        where T : IParamCurve<Vector2>
        => curve.GetArcLength(FloatRange.Unit, accuracy);

    /// <summary>Returns the approximate length of the curve in the given interval</summary>
    /// <param name="interval">The parameter interval of the curve to get the length of</param>
    /// <param name="accuracy">The number of subdivisions to approximate the length with. Higher values are more accurate, but more expensive to calculate</param>
    [Pure]
    public static float GetArcLength<T>(this T curve, FloatRange interval, int accuracy = 8)
        where T : IParamCurve<Vector2>
    {
        accuracy = accuracy.AtLeast(2);
        bool unit = interval == FloatRange.Unit;
        float totalDist = 0;
        Vector2 prev = curve.Eval(interval.Start);
        for (int i = 1; i < accuracy; i++)
        {
            float t = i / (accuracy - 1f);
            Vector2 p = curve.Eval(unit ? t : interval.Lerp(t));
            float dx = p.X - prev.X;
            float dy = p.Y - prev.Y;
            totalDist += Mathf.Sqrt(dx * dx + dy * dy);
            prev = p;
        }

        return totalDist;
    }
}
