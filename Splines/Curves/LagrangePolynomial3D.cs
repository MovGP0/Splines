using System.Numerics;

namespace Splines.Curves;

public sealed class LagrangePolynomial3D : LagrangePolynomial<Vector3>
{
    [Pure]
    protected override Vector3 Zero => Vector3.Zero;

    [Pure]
    protected override Vector3 Multiply(Vector3 value, float scalar) => value * scalar;

    [Pure]
    protected override Vector3 Add(Vector3 a, Vector3 b) => a + b;
}
