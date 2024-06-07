using System.Numerics;

namespace Splines.Curves;

public struct PolynomialMath3D : IPolynomialMath<Polynomial3D, Vector3>
{
    public Polynomial3D NaN => Polynomial3D.NaN;

    /// <inheritdoc cref="Polynomial3D.FitCubicFrom0(float,float,float,Vector3,Vector3,Vector3,Vector3)"/>
    public Polynomial3D FitCubicFrom0(
        float x1,
        float x2,
        float x3,
        Vector3 y0,
        Vector3 y1,
        Vector3 y2,
        Vector3 y3)
    {
        return Polynomial3D.FitCubicFrom0(
            x1,
            x2,
            x3,
            y0,
            y1,
            y2,
            y3);
    }
}
