namespace Splines.Curves;

public sealed class LagrangePolynomial1D : LagrangePolynomial<float>
{
    protected override float Zero => 0f;

    protected override float Multiply(float value, float scalar)
    {
        return value * scalar;
    }

    protected override float Add(float a, float b)
    {
        return a + b;
    }
}
