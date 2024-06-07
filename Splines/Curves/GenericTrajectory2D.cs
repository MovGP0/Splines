using System.Numerics;
using Splines.Unity;

namespace Splines.Curves;

public sealed class GenericTrajectory2D
{
    private readonly Vector2[] _derivatives;

    public Vector2[] Derivatives => (Vector2[])_derivatives.Clone();

    public GenericTrajectory2D(params Vector2[] derivatives)
        => _derivatives = derivatives;

    [Pure]
    public Vector2 GetPosition(float time)
    {
        Vector2 pt = _derivatives[0];

        for (int i = 1; i < _derivatives.Length; i++)
        {
            float scale = Mathf.Pow(time, i) / Mathfs.Factorial((uint)i);
            pt += scale * _derivatives[i];
        }

        return pt;
    }
}
