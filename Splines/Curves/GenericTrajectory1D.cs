using Splines.Unity;

namespace Splines.Curves;

public sealed class GenericTrajectory1D
{
    private readonly float[] _derivatives;

    public float[] Derivatives => (float[])_derivatives.Clone();

    public GenericTrajectory1D(params float[] derivatives)
        => _derivatives = derivatives;

    [Pure]
    public float GetPosition(float time)
    {
        float pt = _derivatives[0];

        for (int i = 1; i < _derivatives.Length; i++)
        {
            float scale = Mathf.Pow(time, i) / Mathfs.Factorial((uint)i);
            pt += scale * _derivatives[i];
        }

        return pt;
    }
}
