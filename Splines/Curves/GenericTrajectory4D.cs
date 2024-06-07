using System.Numerics;
using Splines.Unity;

namespace Splines.Curves;

public sealed class GenericTrajectory4D
{
    private readonly Vector4[] _derivatives;

    public Vector4[] Derivatives => (Vector4[])_derivatives.Clone();

    public GenericTrajectory4D(params Vector4[] derivatives)
        => _derivatives = derivatives;

    [Pure]
    public Vector4 GetPosition(float time)
    {
        Vector4 pt = _derivatives[0];

        for (int i = 1; i < _derivatives.Length; i++)
        {
            float scale = Mathf.Pow(time, i) / Mathfs.Factorial((uint)i);
            pt += scale * _derivatives[i];
        }

        return pt;
    }
}
