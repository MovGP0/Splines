using System.Runtime.CompilerServices;
using Splines.Unity;

namespace Splines.Extensions;

public static class FloatExtensions
{
    [Pure]
    public static bool Between(this float value, float min, float max) => value >= min && value <= max;

    [Pure]
    public static float Square(this float value) => value * value;

    /// <inheritdoc cref="Mathfs.ClampNeg1to1(float)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ClampNeg1to1(this float value) => Mathfs.ClampNeg1to1(value);

    /// <inheritdoc cref="Mathfs.Cbrt(float)"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cbrt(this float value) => Mathf.Cbrt(value);

    [Pure]
    public static int Sign(this float value) => Math.Sign(value);

    [Pure]
    public static float Abs(this float value) => Math.Abs(value);

    [Pure]
    public static float Lerp(this float t, float left, float right) => left + (right - left) * t;

    [Pure]
    public static bool Within(this float value, float min, float max) => value >= min && value <= max;

    [Pure]
    public static float AtLeast(this float @value, float other) => value < other ? other : value;

    [Pure]
    public static float AtMost(this float @value, float other) => value > other ? other : value;

    [Pure]
    public static float Pow(this float b, float e) => (float)Math.Pow(b, e);

    [Pure]
    public static float Clamp(this float value, float a, float b)
    {
        if (value < a)
            return a;
        if (value > b)
            return b;
        return value;
    }
}
