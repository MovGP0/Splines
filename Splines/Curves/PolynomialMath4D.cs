using System.Numerics;

namespace Splines.Curves;

public struct PolynomialMath4D : IPolynomialMath<Polynomial4D, Vector4> {
    public Polynomial4D NaN => Polynomial4D.NaN;

    /// <inheritdoc cref="Polynomial4D.FitCubicFrom0(float,float,float,Vector4,Vector4,Vector4,Vector4)"/>
    public Polynomial4D FitCubicFrom0(
        float x1,
        float x2,
        float x3,
        Vector4 y0,
        Vector4 y1,
        Vector4 y2,
        Vector4 y3)
    {
        return Polynomial4D.FitCubicFrom0(
            x1,
            x2,
            x3,
            y0,
            y1,
            y2,
            y3);
    }
}
