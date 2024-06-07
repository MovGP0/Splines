namespace Splines.Unity;

internal struct MathfInternal
{
    public static readonly float FloatMinNormal = 1.17549435E-38f;
    public static readonly float FloatMinDenormal = float.Epsilon;
    public static readonly bool IsFlushToZeroEnabled = FloatMinDenormal == 0;
}
