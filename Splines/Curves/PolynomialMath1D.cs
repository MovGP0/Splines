namespace Splines.Curves;

public struct PolynomialMath1D : IPolynomialMath<Polynomial1D, float>
{
    public Polynomial1D NaN => Polynomial1D.NaN;

    /// <inheritdoc cref="Polynomial1D.FitCubicFrom0(float,float,float,float,float,float,float)"/>
    public Polynomial1D FitCubicFrom0(
        float x1,
        float x2,
        float x3,
        float y0,
        float y1,
        float y2,
        float y3)
    {
        return Polynomial1D.FitCubicFrom0(
            x1,
            x2,
            x3,
            y0,
            y1,
            y2,
            y3);
    }
}
