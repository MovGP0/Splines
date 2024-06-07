using System.Numerics;

namespace Splines.Curves;

public struct PolynomialMath2D : IPolynomialMath<Polynomial2D, Vector2>
{
    public Polynomial2D NaN => Polynomial2D.NaN;

    /// <inheritdoc cref="Polynomial2D.FitCubicFrom0(float,float,float,Vector2,Vector2,Vector2,Vector2)"/>
    public Polynomial2D FitCubicFrom0(
        float x1,
        float x2,
        float x3,
        Vector2 y0,
        Vector2 y1,
        Vector2 y2,
        Vector2 y3)
    {
        return Polynomial2D.FitCubicFrom0(
            x1,
            x2,
            x3,
            y0,
            y1,
            y2,
            y3);
    }
}
