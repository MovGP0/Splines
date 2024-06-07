using System.Numerics;
using Splines.Numerics;

namespace Splines.GeometricAlgebra;

public static class SpinorFactory
{
    /// <summary>
    /// Creates a 3D Pauli spinor
    /// </summary>
    /// <param name="x">The X component</param>
    /// <param name="y">The Y component</param>
    /// <param name="z">The Z component</param>
    /// <returns>The Pauli spinor</returns>
    public static ComplexMatrix2x2 CreatePauliSpinor(float x, float y, float z)
    {
        var i = Complex.ImaginaryOne;
        return new ComplexMatrix2x2(z, x - i * y, x + i * y, -z);
    }

    /// <summary>
    /// Creates a 4D Weyl spinor
    /// </summary>
    /// <param name="x">The X component</param>
    /// <param name="y">The Y component</param>
    /// <param name="z">The Z component</param>
    /// <param name="w">w = ct = Time multiplied by the speed of light</param>
    /// <returns>The Weyl spinor</returns>
    public static ComplexMatrix2x2 CreateWeylSpinor(float x, float y, float z, float w)
    {
        var i = Complex.ImaginaryOne;
        return new ComplexMatrix2x2(w + z, x - i * y, x + i * y, w - z);
    }
}
