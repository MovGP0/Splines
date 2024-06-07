using System.Collections.Generic;

namespace Splines.Curves;

public abstract class LagrangePolynomial<T>
{
    public List<T> Points = new();
    public List<float>? Knots = null;
    public bool Uniform => Knots == null;

    public (float, float) InternalKnotRange => Knots == null ? (0, Points.Count - 1) : (Knots[0], Knots[Knots.Count - 1]);

    [Pure]
    protected abstract T Zero { get; }

    [Pure]
    protected abstract T Multiply(T value, float scalar);

    [Pure]
    protected abstract T Add(T a, T b);

    [Pure]
    public T Eval(float u)
    {
        float l(int j)
        {
            float prod = 1;
            for (int i = 0; i < Points.Count; i++)
            {
                if (i == j)
                {
                    continue;
                }

                if (Uniform)
                {
                    prod *= (u - i) / (j - i);
                }
                else
                {
                    prod *= Mathfs.InverseLerp(Knots![i], Knots[j], u);
                }
            }

            return prod;
        }

        T sum = Zero;
        for (int j = 0; j < Points.Count; j++)
        {
            sum = Add(sum, Multiply(Points[j], l(j)));
        }

        return sum;
    }
}
