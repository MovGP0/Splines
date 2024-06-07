using System.Numerics;
using Splines.Unity;

namespace Splines.Curves;

public sealed class GenericTrajectory3D
{
    private readonly Vector3[] _derivatives;

    public Vector3[] Derivatives => (Vector3[])_derivatives.Clone();

    public GenericTrajectory3D(params Vector3[] derivatives)
        => _derivatives = derivatives;

    [Pure]
    public Vector3 GetPosition(float time)
    {
        Vector3 pt = _derivatives[0];

        for (int i = 1; i < _derivatives.Length; i++)
        {
            float scale = Mathf.Pow(time, i) / Mathfs.Factorial((uint)i);
            pt += scale * _derivatives[i];
        }

        return pt;
    }
}
