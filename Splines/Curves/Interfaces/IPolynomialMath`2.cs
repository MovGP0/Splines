namespace Splines.Curves;

public interface IPolynomialMath<out P, in V>
{
    public P NaN { get; }
    public P FitCubicFrom0(
        float x1,
        float x2,
        float x3,
        V y0,
        V y1,
        V y2,
        V y3);
}
