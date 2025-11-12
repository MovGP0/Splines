using System.Numerics;
using Splines.Numerics;

namespace Splines.GeometricAlgebra;

public static class SpinorFactory
{
    /// <summary>
    /// Creates a 3D Pauli spinor.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    /// <returns>The Pauli spinor.</returns>
    public static ComplexMatrix2x2 CreatePauliSpinor(float x, float y, float z)
    {
        var i = Complex.ImaginaryOne;
        return new ComplexMatrix2x2(z, x - i * y, x + i * y, -z);
    }

    /// <summary>
    /// Creates a 4D Weyl spinor.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">w = ct = Time multiplied by the speed of light.</param>
    /// <returns>The Weyl spinor.</returns>
    public static ComplexMatrix2x2 CreateWeylSpinor(float x, float y, float z, float w)
    {
        var i = Complex.ImaginaryOne;
        return new ComplexMatrix2x2(w + z, x - i * y, x + i * y, w - z);
    }

    /// <summary>
    /// Creates a Jones vector for the provided amplitudes and phases (radians).
    /// </summary>
    /// <param name="horizontalAmplitude">Amplitude of the horizontal component.</param>
    /// <param name="verticalAmplitude">Amplitude of the vertical component.</param>
    /// <param name="horizontalPhase">Phase of the horizontal component. (default = 0)</param>
    /// <param name="verticalPhase">Phase of the vertical component. (default = 0)</param>
    /// <returns>The Jones vector.</returns>
    public static ComplexVector2 CreateJonesVector(
        double horizontalAmplitude,
        double verticalAmplitude,
        double horizontalPhase,
        double verticalPhase)
    {
        var horizontal = Complex.FromPolarCoordinates(horizontalAmplitude, horizontalPhase);
        var vertical = Complex.FromPolarCoordinates(verticalAmplitude, verticalPhase);
        return new ComplexVector2(horizontal, vertical);
    }

    /// <summary>
    /// Creates the normalized horizontal polarization Jones vector (H).
    /// </summary>
    /// <returns>The horizontal polarization vector.</returns>
    public static ComplexVector2 CreateHorizontalPolarizationVector() => new(Complex.One, Complex.Zero);

    /// <summary>
    /// Creates the normalized vertical polarization Jones vector (V).
    /// </summary>
    /// <returns>The vertical polarization vector.</returns>
    public static ComplexVector2 CreateVerticalPolarizationVector() => new(Complex.Zero, Complex.One);

    /// <summary>
    /// Creates the normalized diagonal polarization Jones vector (D).
    /// </summary>
    /// <returns>The diagonal polarization vector.</returns>
    public static ComplexVector2 CreateDiagonalPolarizationVector()
    {
        var amplitude = 1.0 / Math.Sqrt(2.0);
        return new ComplexVector2(new Complex(amplitude, 0.0), new Complex(amplitude, 0.0));
    }

    /// <summary>
    /// Creates the normalized anti-diagonal polarization Jones vector (A).
    /// </summary>
    /// <returns>The anti-diagonal polarization vector.</returns>
    public static ComplexVector2 CreateAntidiagonalPolarizationVector()
    {
        var amplitude = 1.0 / Math.Sqrt(2.0);
        return new ComplexVector2(new Complex(amplitude, 0.0), new Complex(-amplitude, 0.0));
    }

    /// <summary>
    /// Creates the normalized right-circular polarization Jones vector (R).
    /// </summary>
    /// <returns>The right-circular polarization vector.</returns>
    public static ComplexVector2 CreateRightCircularPolarizationVector()
    {
        var amplitude = 1.0 / Math.Sqrt(2.0);
        return new ComplexVector2(new Complex(amplitude, 0.0), -Complex.ImaginaryOne * amplitude);
    }

    /// <summary>
    /// Creates the normalized left-circular polarization Jones vector (L).
    /// </summary>
    /// <returns>The left-circular polarization vector.</returns>
    public static ComplexVector2 CreateLeftCircularPolarizationVector()
    {
        var amplitude = 1.0 / Math.Sqrt(2.0);
        return new ComplexVector2(new Complex(amplitude, 0.0), Complex.ImaginaryOne * amplitude);
    }

    /// <summary>
    /// Creates the Jones matrix for a horizontal linear polarizer.
    /// </summary>
    /// <returns>The horizontal linear polarizer matrix.</returns>
    public static ComplexMatrix2x2 CreateHorizontalPolarizer() => CreateLinearPolarizer(0.0);

    /// <summary>
    /// Creates the Jones matrix for a vertical linear polarizer.
    /// </summary>
    /// <returns>The vertical linear polarizer matrix.</returns>
    public static ComplexMatrix2x2 CreateVerticalPolarizer() => CreateLinearPolarizer(Math.PI / 2.0);

    /// <summary>
    /// Creates the Jones matrix for a linear polarizer rotated by an arbitrary angle [rad].
    /// </summary>
    /// <param name="angle">The rotation angle [rad].</param>
    /// <returns>The rotated linear polarizer matrix.</returns>
    public static ComplexMatrix2x2 CreateLinearPolarizer(double angle)
    {
        var cos = Math.Cos(angle);
        var sin = Math.Sin(angle);
        var cosSquared = cos * cos;
        var sinSquared = sin * sin;
        var cross = cos * sin;

        return new ComplexMatrix2x2(
            new Complex(cosSquared, 0.0),
            new Complex(cross, 0.0),
            new Complex(cross, 0.0),
            new Complex(sinSquared, 0.0));
    }

    /// <summary>
    /// Creates the Jones matrix for a waveplate aligned to the horizontal axis.
    /// </summary>
    /// <param name="phaseDelay">Phase delay between axes [rad].</param>
    /// <returns>The waveplate matrix.</returns>
    public static ComplexMatrix2x2 CreateWaveplate(double phaseDelay)
    {
        var phase = Complex.FromPolarCoordinates(1.0, phaseDelay);
        return new ComplexMatrix2x2(Complex.One, Complex.Zero, Complex.Zero, phase);
    }

    /// <summary>
    /// Creates the Jones matrix for a quarter-wave plate aligned to the horizontal axis.
    /// </summary>
    /// <returns>The quarter-wave plate matrix.</returns>
    public static ComplexMatrix2x2 CreateQuarterWaveplate()
        => CreateWaveplate(Math.PI / 2.0);

    /// <summary>
    /// Creates the Jones matrix for a waveplate rotated by an arbitrary angle (radians).
    /// </summary>
    /// <param name="phaseDelay">Phase delay between axes in radians.</param>
    /// <param name="rotationAngle">Rotation angle of the waveplate in radians.</param>
    /// <returns>The rotated waveplate matrix.</returns>
    public static ComplexMatrix2x2 CreateWaveplate(double phaseDelay, double rotationAngle)
    {
        var phase = Complex.FromPolarCoordinates(1.0, phaseDelay);
        var cos = Math.Cos(rotationAngle);
        var sin = Math.Sin(rotationAngle);
        var cosSquared = cos * cos;
        var sinSquared = sin * sin;
        var cross = cos * sin;

        var cosSquaredComplex = new Complex(cosSquared, 0.0);
        var sinSquaredComplex = new Complex(sinSquared, 0.0);
        var crossComplex = new Complex(cross, 0.0);

        var m00 = cosSquaredComplex + phase * sinSquaredComplex;
        var m11 = sinSquaredComplex + phase * cosSquaredComplex;
        var m01 = (phase - Complex.One) * crossComplex;

        return new ComplexMatrix2x2(m00, m01, m01, m11);
    }

    /// <summary>
    /// Creates the Jones matrix for a quarter-wave plate rotated by an arbitrary angle (radians).
    /// </summary>
    /// <param name="rotationAngle">Rotation angle of the waveplate in radians.</param>
    /// <returns>The rotated quarter-wave plate matrix.</returns>
    public static ComplexMatrix2x2 CreateQuarterWaveplate(double rotationAngle)
        => CreateWaveplate(Math.PI / 2.0, rotationAngle);
}
