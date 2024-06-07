using System.Runtime.CompilerServices;

namespace Splines.Extensions;

public static class IntExtensions
{
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(this int value)
        => value < 0 ? -value : value;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AtLeast(this int value, int min)
        => value < min ? min : value;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AtMost(this int value, int max)
        => value > max ? max : value;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Mod(this int value, int mod)
        => value < 0 ? value % mod + mod : value % mod;

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(this int value, int min, int max)
        => value < min ? min : value > max ? max : value;
}
