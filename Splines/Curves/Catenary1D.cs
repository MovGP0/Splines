using System.Numerics;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>Catenary math utility functions</summary>
public static class Catenary1D
{
    /// <summary>Returns the y coordinate of a catenary at the given x value</summary>
    /// <param name="x">The x coordinate to evaluate at</param>
    /// <param name="a">The a-parameter of the catenary</param>
    public static float Eval(float x, float a) => a * Mathfs.Cosh(x / a);

    /// <summary>Evaluates the arc length from the apex of the catenary, to the given x coordinate.
    /// Note that this is negative when x is less than 0</summary>
    /// <param name="x">The x coordinate to get the length to</param>
    /// <param name="a">The a-parameter of the catenary</param>
    public static float EvalArcLength(float x, float a) => a * Mathfs.Sinh(x / a);

    /// <summary>Evaluates the x coordinate at the given arc length relative to the apex of the catenary.
    /// Note that the input arc length can be negative, to get the negative x coordinates</summary>
    /// <param name="s">The arc length to get the x coordinate of</param>
    /// <param name="a">The a-parameter of the catenary</param>
    public static float EvalXByArcLength(float s, float a) => a * Mathfs.Asinh(s / a);

    /// <summary>Evaluates the n:th 2D derivative at the given arc length relative to the apex of the catenary.
    /// Note that the input arc length can be negative, to get the tangents on the negative x side</summary>
    /// <param name="s">The arc length coordinate to get the tangent of</param>
    /// <param name="a">The a-parameter of the catenary</param>
    public static Vector2 EvalDerivateByArcLength(float s, float a, int n = 1)
    {
        if (n == 0)
        { // position
            float x = EvalXByArcLength(s, a);
            float y = Eval(x, a);
            return new Vector2(x, y);
        }

        float xNum;
        float yNum;
        float aSq = a * a;
        float sSq = s * s;

        switch (n)
        {
            case 1: // velocity
                xNum = a;
                yNum = s;
                break;

            case 2: // acceleration
                xNum = -a * s;
                yNum = aSq;
                break;

            case 3: // jerk/jolt
                xNum = a * (-aSq + 2 * sSq);
                yNum = 3 * aSq * s;
                break;

            case 4: // 4th derivative
                xNum = 3 * s * a * (-3 * aSq + 2 * sSq);
                yNum = 3 * aSq * (-aSq + 4 * sSq);
                break;
            default:
                throw new NotImplementedException($"Derivative ({n}) of Catenaries are not implemented");
        }

        float den = Mathf.Pow(aSq + sSq, (n * 2 - 1) / 2f);
        return new Vector2(xNum / den, yNum / den);
    }
}
