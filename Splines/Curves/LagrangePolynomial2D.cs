using System.Numerics;

namespace Splines.Curves;

public sealed class LagrangePolynomial2D : LagrangePolynomial<Vector2>
{
    [Pure]
    protected override Vector2 Zero => Vector2.Zero;

    [Pure]
    protected override Vector2 Multiply(Vector2 value, float scalar) => value * scalar;

    [Pure]
    protected override Vector2 Add(Vector2 a, Vector2 b) => a + b;
}
