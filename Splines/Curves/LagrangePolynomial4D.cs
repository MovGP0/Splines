using System.Numerics;

namespace Splines.Curves;

public sealed class LagrangePolynomial4D : LagrangePolynomial<Vector4>
{
    [Pure]
    protected override Vector4 Zero => Vector4.Zero;

    [Pure]
    protected override Vector4 Multiply(Vector4 value, float scalar) => value * scalar;

    [Pure]
    protected override Vector4 Add(Vector4 a, Vector4 b) => a + b;
}
