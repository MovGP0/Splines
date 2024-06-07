using System.Numerics;
using System.Runtime.CompilerServices;

namespace Splines.Unity;

/// <summary>
/// Backport of Unity's Mathf class
/// </summary>
public struct Mathf
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ceiling(float value) => (float)Math.Ceiling(value);

    /// <summary>
    /// Returns the sine of angle <paramref name="f"/> in radians.
    /// </summary>
    /// <param name="f">The angle in radians.</param>
    /// <returns>The sine of <paramref name="f"/>.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sin(float f) => (float)Math.Sin(f);

    // Returns the cosine of angle /f/ in radians.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cos(float f) => (float)Math.Cos(f);

    // Returns the tangent of angle /f/ in radians.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Tan(float f) => (float)Math.Tan(f);

    // Returns the arc-sine of /f/ - the angle in radians whose sine is /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Asin(float f) => (float)Math.Asin(f);

    // Returns the arc-cosine of /f/ - the angle in radians whose cosine is /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Acos(float f) => (float)Math.Acos(f);

    // Returns the arc-tangent of /f/ - the angle in radians whose tangent is /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Atan(float f) => (float)Math.Atan(f);

    // Returns the angle in radians whose ::ref::Tan is @@y/x@@.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Atan2(float y, float x) => (float)Math.Atan2(y, x);

    /// <summary>
    /// Returns square root of /f/
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sqrt(float f) => (float)Math.Sqrt(f);

    /// <summary>
    /// Returns the absolute value of /f/
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Abs(float f) => Math.Abs(f);

    /// <summary>
    /// Returns the absolute value of /value/.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int value) => Math.Abs(value);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Min(float a, float b) => a < b ? a : b;

    /// <summary>
    /// Returns the smallest of two or more values.
    /// </summary>
    [Pure]
    public static float Min(params float[] values)
    {
        int len = values.Length;
        if (len == 0)
            return 0;
        float m = values[0];
        for (int i = 1; i < len; i++)
        {
            if (values[i] < m)
                m = values[i];
        }

        return m;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Min(int a, int b) => a < b ? a : b;

    /// <summary>
    /// Returns the smallest of two or more values.
    /// </summary>
    [Pure]
    public static int Min(params int[] values)
    {
        int len = values.Length;
        if (len == 0)
            return 0;
        int m = values[0];
        for (int i = 1; i < len; i++)
        {
            if (values[i] < m)
                m = values[i];
        }

        return m;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Max(float a, float b) => a > b ? a : b;

    // Returns largest of two or more values.
    [Pure]
    public static float Max(params float[] values)
    {
        int len = values.Length;
        if (len == 0)
            return 0;
        float m = values[0];
        for (int i = 1; i < len; i++)
        {
            if (values[i] > m)
                m = values[i];
        }

        return m;
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Max(int a, int b) => a > b ? a : b;

    // Returns the largest of two or more values.
    [Pure]
    public static int Max(params int[] values)
    {
        int len = values.Length;
        if (len == 0)
            return 0;
        int m = values[0];
        for (int i = 1; i < len; i++)
        {
            if (values[i] > m)
                m = values[i];
        }

        return m;
    }

    // Returns /f/ raised to power /p/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow(float f, float p) => (float)Math.Pow(f, p);

    // Returns e raised to the specified power.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Exp(float power) => (float)Math.Exp(power);

    // Returns the logarithm of a specified number in a specified base.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log(float f, float p) => (float)Math.Log(f, p);

    // Returns the natural (base e) logarithm of a specified number.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log(float f) => (float)Math.Log(f);

    // Returns the base 10 logarithm of a specified number.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Log10(float f) => (float)Math.Log10(f);

    // Returns the smallest integer greater to or equal to /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Ceil(float f) => (float)Math.Ceiling(f);

    // Returns the largest integer smaller to or equal to /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Floor(float f) => (float)Math.Floor(f);

    // Returns /f/ rounded to the nearest integer.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float f) => (float)Math.Round(f);

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Round(float f, MidpointRounding mode) => (float)Math.Round(f, mode);

    // Returns the smallest integer greater to or equal to /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CeilToInt(float f) => (int)Math.Ceiling(f);

    // Returns the largest integer smaller to or equal to /f/.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FloorToInt(float f) => (int)Math.Floor(f);

    // Returns /f/ rounded to the nearest integer.
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int RoundToInt(float f) => (int)Math.Round(f);

    /// <summary>
    /// Returns the sign of /f/.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sign(float f) => f >= 0F ? 1F : -1F;

    /// <summary>
    /// The infamous ''3.14159265358979...'' value (RO).
    /// </summary>
    public const float PI = (float)Math.PI;

    /// <summary>
    /// A representation of positive infinity (RO).
    /// </summary>
    public const float Infinity = float.PositiveInfinity;

    /// <summary>
    /// A representation of negative infinity (RO).
    /// </summary>
    public const float NegativeInfinity = float.NegativeInfinity;

    /// <summary>
    /// Degrees-to-radians conversion constant (RO).
    /// </summary>
    public const float Deg2Rad = PI * 2F / 360F;

    /// <summary>
    /// Radians-to-degrees conversion constant (RO).
    /// </summary>
    public const float Rad2Deg = 1F / Deg2Rad;

    /// <summary>
    /// We cannot round to more decimals than 15 according to docs for System.Math.Round.
    /// </summary>
    internal const int kMaxDecimals = 15;

    /// <summary>
    /// A tiny floating point value (RO).
    /// </summary>
    public static readonly float Epsilon =
        MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinNormal
            : MathfInternal.FloatMinDenormal;

    /// <summary>
    /// Clamps a value between a minimum float and maximum float value.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
        => value < min ? min : value > max ? max : value;

    /// <summary>
    /// Clamps value between min and max and returns value.
    /// Set the position of the transform to be that of the time
    /// but never less than 1 or more than 3
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
        => value < min ? min : value > max ? max : value;

    /// <summary>
    /// Clamps value between 0 and 1 and returns value
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp01(float value)
        => value < 0F ? 0F : value > 1F ? 1F : value;

    /// <summary>
    /// Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float a, float b, float t)
        => a + (b - a) * Clamp01(t);

    /// <summary>
    /// Interpolates between /a/ and /b/ by /t/ without clamping the interpolant.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LerpUnclamped(float a, float b, float t)
        => a + (b - a) * t;

    /// <summary>
    /// Same as <see cref="Lerp"/> but makes sure the values interpolate correctly when they wrap around 360 degrees.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float LerpAngle(float a, float b, float t)
    {
        float delta = Repeat(b - a, 360f);
        if (delta > 180f)
            delta -= 360f;
        return a + delta * Clamp01(t);
    }

    /// <summary>
    /// Moves a value /current/ towards /target/
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float MoveTowards(float current, float target, float maxDelta)
    {
        if (Abs(target - current) <= maxDelta)
            return target;
        return current + Sign(target - current) * maxDelta;
    }

    /// <summary>
    /// Same as <see cref="MoveTowards"/> but makes sure the values interpolate correctly when they wrap around 360 degrees.
    /// </summary>
    [Pure]
    public static float MoveTowardsAngle(float current, float target, float maxDelta)
    {
        float deltaAngle = DeltaAngle(current, target);
        if (-maxDelta < deltaAngle && deltaAngle < maxDelta)
            return target;
        target = current + deltaAngle;
        return MoveTowards(current, target, maxDelta);
    }

    /// <summary>
    /// Interpolates between /min/ and /max/ with smoothing at the limits.
    /// </summary>
    [Pure]
    public static float SmoothStep(float from, float to, float t)
    {
        t = Clamp01(t);
        t = -2.0F * t * t * t + 3.0F * t * t;
        return to * t + from * (1F - t);
    }

    [Pure]
    public static float Gamma(float value, float absmax, float gamma)
    {
        bool negative = value < 0F;
        float absval = Abs(value);
        if (absval > absmax)
            return negative ? -absval : absval;

        float result = Pow(absval / absmax, gamma) * absmax;
        return negative ? -result : result;
    }

    /// <summary>
    /// Compares two floating point values if they are similar.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Approximately(float a, float b)
    {
        // If a or b is zero, compare that the other is less or equal to epsilon.
        // If neither a or b are 0, then find an epsilon that is good for
        // comparing numbers at the maximum magnitude of a and b.
        // Floating points have about 7 significant digits, so
        // 1.000001f can be represented while 1.0000001f is rounded to zero,
        // thus we could use an epsilon of 0.000001f for comparing values close to 1.
        // We multiply this epsilon by the biggest magnitude of a and b.
        return Abs(b - a) < Max(0.000001f * Max(Abs(a), Abs(b)), Epsilon * 8);
    }

    /// <summary>
    /// Gradually changes a value towards a desired goal over time.
    /// </summary>
    [Pure]
    public static float SmoothDamp(
        float current,
        float target,
        ref float currentVelocity,
        float smoothTime,
        float maxSpeed,
        float deltaTime)
    {
        // Based on Game Programming Gems 4 Chapter 1.10
        smoothTime = Max(0.0001F, smoothTime);
        float omega = 2F / smoothTime;

        float x = omega * deltaTime;
        float exp = 1F / (1F + x + 0.48F * x * x + 0.235F * x * x * x);
        float change = current - target;
        float originalTo = target;

        // Clamp maximum speed
        float maxChange = maxSpeed * smoothTime;
        change = Clamp(change, -maxChange, maxChange);
        target = current - change;

        float temp = (currentVelocity + omega * change) * deltaTime;
        currentVelocity = (currentVelocity - omega * temp) * exp;
        float output = target + (change + temp) * exp;

        // Prevent overshooting
        if (originalTo - current > 0.0F == output > originalTo)
        {
            output = originalTo;
            currentVelocity = (output - originalTo) / deltaTime;
        }

        return output;
    }

    /// <summary>
    /// Gradually changes an angle given in degrees towards a desired goal angle over time.
    /// </summary>
    [Pure]
    public static float SmoothDampAngle(
        float current,
        float target,
        ref float currentVelocity,
        float smoothTime,
        float maxSpeed,
        float deltaTime)
    {
        target = current + DeltaAngle(current, target);
        return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    /// <summary>
    /// Loops the value t, so that it is never larger than length and never smaller than 0.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Repeat(float t, float length)
    {
        return Clamp(t - Floor(t / length) * length, 0.0f, length);
    }

    /// <summary>
    /// PingPongs the value t, so that it is never larger than length and never smaller than 0.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PingPong(float t, float length)
    {
        t = Repeat(t, length * 2F);
        return length - Abs(t - length);
    }

    /// <summary>
    /// Calculates the <see cref="Lerp"/> parameter between of two values.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float InverseLerp(float a, float b, float value)
    {
        if (a != b)
            return Clamp01((value - a) / (b - a));
        return 0.0f;
    }

    /// <summary>
    /// Calculates the shortest difference between two given angles.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DeltaAngle(float current, float target)
    {
        float delta = Repeat(target - current, 360.0F);
        if (delta > 180.0F)
            delta -= 360.0F;
        return delta;
    }

    /// <summary>
    /// Infinite Line Intersection (line1 is p1-p2 and line2 is p3-p4)
    /// </summary>
    [Pure]
    internal static bool LineIntersection(
        Vector2 p1,
        Vector2 p2,
        Vector2 p3,
        Vector2 p4,
        ref Vector2 result)
    {
        float bx = p2.X - p1.X;
        float by = p2.Y - p1.Y;
        float dx = p4.X - p3.X;
        float dy = p4.Y - p3.Y;
        float bDotDPerp = bx * dy - by * dx;
        if (bDotDPerp == 0)
        {
            return false;
        }

        float cx = p3.X - p1.X;
        float cy = p3.Y - p1.Y;
        float t = (cx * dy - cy * dx) / bDotDPerp;

        result.X = p1.X + t * bx;
        result.Y = p1.Y + t * by;
        return true;
    }

    /// <summary>
    /// Line Segment Intersection (line1 is p1-p2 and line2 is p3-p4)
    /// </summary>
    [Pure]
    internal static bool LineSegmentIntersection(
        Vector2 p1,
        Vector2 p2,
        Vector2 p3,
        Vector2 p4,
        ref Vector2 result)
    {
        float bx = p2.X - p1.X;
        float by = p2.Y - p1.Y;
        float dx = p4.X - p3.X;
        float dy = p4.Y - p3.Y;
        float bDotDPerp = bx * dy - by * dx;
        if (bDotDPerp == 0)
        {
            return false;
        }

        float cx = p3.X - p1.X;
        float cy = p3.Y - p1.Y;
        float t = (cx * dy - cy * dx) / bDotDPerp;
        if (t is < 0 or > 1)
        {
            return false;
        }

        float u = (cx * by - cy * bx) / bDotDPerp;
        if (u is < 0 or > 1)
        {
            return false;
        }

        result.X = p1.X + t * bx;
        result.Y = p1.Y + t * by;
        return true;
    }

    [Pure]
    internal static long RandomToLong(Random r)
    {
        var buffer = new byte[8];
        r.NextBytes(buffer);
        return (long)(BitConverter.ToUInt64(buffer, 0) & long.MaxValue);
    }

    [Pure]
    internal static float ClampToFloat(double value)
    {
        if (double.IsPositiveInfinity(value))
            return float.PositiveInfinity;

        if (double.IsNegativeInfinity(value))
            return float.NegativeInfinity;

        if (value < float.MinValue)
            return float.MinValue;

        if (value > float.MaxValue)
            return float.MaxValue;

        return (float)value;
    }

    [Pure]
    internal static int ClampToInt(long value)
    {
        if (value < int.MinValue)
            return int.MinValue;

        if (value > int.MaxValue)
            return int.MaxValue;

        return (int)value;
    }

    [Pure]
    internal static uint ClampToUInt(long value)
    {
        if (value < uint.MinValue)
            return uint.MinValue;

        if (value > uint.MaxValue)
            return uint.MaxValue;

        return (uint)value;
    }

    [Pure]
    internal static float RoundToMultipleOf(float value, float roundingValue)
    {
        if (roundingValue == 0)
            return value;
        return Round(value / roundingValue) * roundingValue;
    }

    [Pure]
    internal static float GetClosestPowerOfTen(float positiveNumber)
    {
        if (positiveNumber <= 0)
            return 1;
        return Pow(10, RoundToInt(Log10(positiveNumber)));
    }

    [Pure]
    internal static int GetNumberOfDecimalsForMinimumDifference(float minDifference)
    {
        return Clamp(-FloorToInt(Log10(Abs(minDifference))), 0, kMaxDecimals);
    }

    [Pure]
    internal static int GetNumberOfDecimalsForMinimumDifference(double minDifference)
    {
        return (int)Math.Max(0.0, -Math.Floor(Math.Log10(Math.Abs(minDifference))));
    }

    [Pure]
    internal static float RoundBasedOnMinimumDifference(float valueToRound, float minDifference)
    {
        if (minDifference == 0)
            return DiscardLeastSignificantDecimal(valueToRound);
        return (float)Math.Round(valueToRound, GetNumberOfDecimalsForMinimumDifference(minDifference),
            MidpointRounding.AwayFromZero);
    }

    [Pure]
    internal static double RoundBasedOnMinimumDifference(double valueToRound, double minDifference)
    {
        if (minDifference == 0)
            return DiscardLeastSignificantDecimal(valueToRound);
        return Math.Round(valueToRound, GetNumberOfDecimalsForMinimumDifference(minDifference),
            MidpointRounding.AwayFromZero);
    }

    [Pure]
    internal static float DiscardLeastSignificantDecimal(float v)
    {
        int decimals = Clamp((int)(5 - Log10(Abs(v))), 0, kMaxDecimals);
        return (float)Math.Round(v, decimals, MidpointRounding.AwayFromZero);
    }

    [Pure]
    internal static double DiscardLeastSignificantDecimal(double v)
    {
        int decimals = Math.Max(0, (int)(5 - Math.Log10(Math.Abs(v))));
        try
        {
            return Math.Round(v, decimals);
        }
        catch (ArgumentOutOfRangeException)
        {
            // This can happen for very small numbers.
            return 0;
        }
    }

    [Pure]
    public static int NextPowerOfTwo(int value)
    {
        value -= 1;
        value |= value >> 16;
        value |= value >> 8;
        value |= value >> 4;
        value |= value >> 2;
        value |= value >> 1;
        return value + 1;
    }

    [Pure]
    public static int ClosestPowerOfTwo(int value)
    {
        int nextPower = NextPowerOfTwo(value);
        int prevPower = nextPower >> 1;
        if (value - prevPower < nextPower - value)
            return prevPower;
        return nextPower;
    }

    [Pure]
    public static bool IsPowerOfTwo(int value)
    {
        return (value & (value - 1)) == 0;
    }

    /// <inheritdoc cref="Math.Cosh"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cosh(float f) => (float)Math.Cosh(f);

    /// <inheritdoc cref="Math.Sinh"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Sinh(float f) => (float)Math.Sinh(f);

    /// <inheritdoc cref="Math.Tanh"/>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Tanh(float f) => (float)Math.Tanh(f);

    /// <summary>
    /// Inverse hyperbolic cosine
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Acosh(float f) => (float)Math.Log(f + Math.Sqrt(f * f - 1));

    /// <summary>
    /// Inverse hyperbolic sine
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Asinh(float f) => (float)Math.Log(f + Math.Sqrt(f * f + 1));

    /// <summary>
    /// Inverse hyperbolic tangent
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Atanh(float f) => (float)(0.5 * Math.Log((1 + f) / (1 - f)));

    /// <summary>
    /// Calculates the cube root of a number
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Cbrt(float value) => (float)Math.Pow(value, 1.0 / 3.0);
}
