namespace Splines.GeometricAlgebra;

public partial struct Rotor4
{
    public static Rotor4 operator +(Rotor4 a, Rotor4 b)
    {
        return new Rotor4(
            a.R + b.R,
            a.B + b.B,
            a.T + b.T
        );
    }

    public static Rotor4 operator -(Rotor4 a, Rotor4 b)
    {
        return new Rotor4(
            a.R - b.R,
            a.B - b.B,
            a.T - b.T
        );
    }

    public static Rotor4 operator *(Rotor4 a, float scalar)
    {
        return new Rotor4(
            a.R * scalar,
            a.B * scalar,
            a.T * scalar
        );
    }

    public static Rotor4 operator *(float scalar, Rotor4 a)
    {
        return a * scalar;
    }

    public static Rotor4 operator -(Rotor4 a)
    {
        return new Rotor4(
            -a.R,
            -a.B,
            -a.T
        );
    }
}
