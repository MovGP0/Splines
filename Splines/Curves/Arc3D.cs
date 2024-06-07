using System.Numerics;
using Splines.Extensions;
using Splines.GeometricShapes;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>
/// A 3D arc with support for straight lines.
/// </summary>
[Serializable]
public partial struct Arc3D
{
    /// <summary>
    /// The starting point of the arc.
    /// </summary>
    public Transform3D Placement
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The signed curvature of the arc in the X-Y plane, equal to 1/radius (0 = straight line, 1 = turning left, -1 = turning right).
    /// </summary>
    public float CurvatureXY
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The signed curvature of the arc in the Z direction, equal to 1/radius (0 = no curvature in Z direction).
    /// </summary>
    public float CurvatureZ
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The length of the arc.
    /// </summary>
    public float Length
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The radius of the circle traced by the arc in the X-Y plane. Returns infinity if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public float RadiusXY => 1f / Mathf.Abs(CurvatureXY);

    /// <summary>
    /// The center of the circle traced by the arc in the X-Y plane. Returns infinity if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public Vector3 CircleCenterXY => StartNormal / CurvatureXY;

    /// <summary>
    /// The normal direction at the start of the arc.
    /// </summary>
    [Pure]
    public Vector3 StartNormal => Placement.AxisZ;

    /// <summary>
    /// The tangent direction at the start of the arc.
    /// </summary>
    [Pure]
    public Vector3 StartTangent => Placement.AxisX;

    /// <summary>
    /// The normal direction at the end of the arc.
    /// </summary>
    [Pure]
    public Vector3 EndNormal => GetNormal(Length);

    /// <summary>
    /// The end point of the arc.
    /// </summary>
    [Pure]
    public Vector3 EndPoint => GetPosition(Length);

    /// <summary>
    /// The signed angular span covered across the arc in the X-Y plane. This returns 0 if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public float AngularSpanXY => Length * CurvatureXY;

    /// <summary>
    /// The signed angular span covered across the arc in the Z direction. This returns 0 if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public float AngularSpanZ => Length * CurvatureZ;

    /// <summary>
    /// Whether this is a straight line rather than an arc, i.e., if curvature is 0 in both planes.
    /// </summary>
    [Pure]
    public bool IsStraight => Mathf.Approximately(CurvatureXY, 0) && Mathf.Approximately(CurvatureZ, 0);

    /// <summary>
    /// Evaluates the position of this arc at the given arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate the position.</param>
    /// <returns>The position at the given arc length.</returns>
    [Pure]
    public Vector3 GetPosition(float s) => Eval(s, nThDerivative: 0);

    /// <summary>
    /// Evaluates the tangent direction of this arc at the given arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate the tangent direction.</param>
    /// <returns>The tangent direction at the given arc length.</returns>
    [Pure]
    public Vector3 GetTangent(float s) =>
        s == 0
            ? StartTangent
            : Eval(s, nThDerivative: 1); // no need to normalize, it's already arc-length parameterized

    /// <summary>
    /// Evaluates the normal direction of this arc at the given arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate the normal direction.</param>
    /// <returns>The normal direction at the given arc length.</returns>
    [Pure]
    public Vector3 GetNormal(float s) =>
        Eval(s, nThDerivative: 1).Rotate90CCW(); // no need to normalize, it's already arc-length parameterized

    /// <summary>
    /// Evaluates the given derivative of this arc, by arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate.</param>
    /// <param name="nThDerivative">The order of the derivative to evaluate.</param>
    /// <returns>The evaluated derivative at the given arc length.</returns>
    [Pure]
    public Vector3 Eval(float s, int nThDerivative = 0)
    {
        float angXY = s * CurvatureXY;
        float angZ = s * CurvatureZ;
        float x, y, z;

        switch (nThDerivative)
        {
            case 0:
                x = s * Mathf.Sin(angXY);
                y = s * Mathf.Cos(angXY);
                z = s * Mathf.Sin(angZ);
                return Placement.TransformPoint(new Vector3(x, y, z));
            case 1:
                x = Mathf.Cos(angXY);
                y = Mathf.Sin(angXY);
                z = Mathf.Cos(angZ);
                break;
            case 2:
                x = -CurvatureXY * Mathf.Sin(angXY);
                y = +CurvatureXY * Mathf.Cos(angXY);
                z = -CurvatureZ * Mathf.Sin(angZ);
                break;
            case 3:
                float k2XY = CurvatureXY * CurvatureXY;
                x = -k2XY * Mathf.Cos(angXY);
                y = -k2XY * Mathf.Sin(angXY);
                z = 0; // Higher derivatives in Z are not needed in this example
                break;
            case 4:
                float k3XY = CurvatureXY * CurvatureXY * CurvatureXY;
                x = +k3XY * Mathf.Sin(angXY);
                y = -k3XY * Mathf.Cos(angXY);
                z = 0; // Higher derivatives in Z are not needed in this example
                break;
            case 5:
                float _k2XY = CurvatureXY * CurvatureXY;
                float k4XY = _k2XY * _k2XY;
                x = k4XY * Mathf.Cos(angXY);
                y = k4XY * Mathf.Sin(angXY);
                z = 0; // Higher derivatives in Z are not needed in this example
                break;
            default:
                // general form for n > 0
                float scaleXY = Mathf.Pow(CurvatureXY, nThDerivative - 1);
                int xSgn = nThDerivative / 2 % 2 == 0 ? 1 : -1;
                int ySgn = (nThDerivative - 1) / 2 % 2 == 0 ? 1 : -1;
                bool even = nThDerivative % 2 == 0;
                x = xSgn * scaleXY * (even ? Mathf.Sin(angXY) : Mathf.Cos(angXY));
                y = ySgn * scaleXY * (even ? Mathf.Cos(angXY) : Mathf.Sin(angXY));
                z = 0; // Higher derivatives in Z are not needed in this example
                break;
        }

        // space transformation
        return Placement.TransformVector(new Vector3(x, y, z));
    }
}
