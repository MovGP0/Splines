using System.Numerics;
using Splines.Extensions;
using Splines.GeometricShapes;
using Splines.Unity;

namespace Splines.Curves;

/// <summary>
/// A 2D arc with support for straight lines.
/// </summary>
[Serializable]
public partial struct Arc2D
{
    /// <summary>
    /// The starting point of the arc.
    /// </summary>
    public Transform2D Placement
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// The signed curvature of the arc, equal to 1/radius (0 = straight line, 1 = turning left, -1 = turning right).
    /// </summary>
    public float Curvature
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
    /// The radius of the circle traced by the arc. Returns infinity if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public float Radius => 1f / Mathf.Abs(Curvature);

    /// <summary>
    /// The center of the circle traced by the arc. Returns infinity if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public Vector2 CircleCenter => StartNormal / Curvature;

    /// <summary>
    /// The normal direction at the start of the arc.
    /// </summary>
    [Pure]
    public Vector2 StartNormal => Placement.AxisY;

    /// <summary>
    /// The tangent direction at the start of the arc.
    /// </summary>
    [Pure]
    public Vector2 StartTangent => Placement.AxisX;

    /// <summary>
    /// The normal direction at the end of the arc.
    /// </summary>
    [Pure]
    public Vector2 EndNormal => GetNormal(Length);

    /// <summary>
    /// The end point of the arc.
    /// </summary>
    [Pure]
    public Vector2 EndPoint => GetPosition(Length);

    /// <summary>
    /// The signed angular span covered across the arc. This returns 0 if this segment is linear, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public float AngularSpan => Length * Curvature;

    /// <summary>
    /// Whether this is a straight line rather than an arc, i.e., if curvature is 0.
    /// </summary>
    [Pure]
    public bool IsStraight => Mathfs.Approximately(Curvature, 0);

    /// <summary>
    /// Evaluates the position of this arc at the given arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate the position.</param>
    /// <returns>The position at the given arc length.</returns>
    [Pure]
    public Vector2 GetPosition(float s) => Eval(s, nThDerivative: 0);

    /// <summary>
    /// Evaluates the tangent direction of this arc at the given arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate the tangent direction.</param>
    /// <returns>The tangent direction at the given arc length.</returns>
    [Pure]
    public Vector2 GetTangent(float s) => s == 0 ? StartTangent : Eval(s, nThDerivative: 1); // no need to normalize, it's already arc-length parameterized

    /// <summary>
    /// Evaluates the normal direction of this arc at the given arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate the normal direction.</param>
    /// <returns>The normal direction at the given arc length.</returns>
    [Pure]
    public Vector2 GetNormal(float s) => Eval(s, nThDerivative: 1).Rotate90CCW(); // no need to normalize, it's already arc-length parameterized

    /// <summary>
    /// Evaluates the given derivative of this arc, by arc length <c>s</c>.
    /// </summary>
    /// <param name="s">The arc length at which to evaluate.</param>
    /// <param name="nThDerivative">The order of the derivative to evaluate.</param>
    /// <returns>The evaluated derivative at the given arc length.</returns>
    [Pure]
    public Vector2 Eval(float s, int nThDerivative = 0)
    {
        float ang = s * Curvature;
        float x, y;

        switch (nThDerivative)
        {
            case 0:
                x = s * Mathfs.Sinc(ang);
                y = s * Mathfs.Cosinc(ang);
                return Placement.TransformPoint(x, y);
            case 1:
                x = Mathf.Cos(ang);
                y = Mathf.Sin(ang);
                break;
            case 2:
                x = -Curvature * Mathf.Sin(ang);
                y = +Curvature * Mathf.Cos(ang);
                break;
            case 3:
                float k2 = Curvature * Curvature;
                x = -k2 * Mathf.Cos(ang);
                y = -k2 * Mathf.Sin(ang);
                break;
            case 4:
                float k3 = Curvature * Curvature * Curvature;
                x = +k3 * Mathf.Sin(ang);
                y = -k3 * Mathf.Cos(ang);
                break;
            case 5:
                float _k2 = Curvature * Curvature;
                float k4 = _k2 * _k2;
                x = k4 * Mathf.Cos(ang);
                y = k4 * Mathf.Sin(ang);
                break;
            default:
                // general form for n > 0
                float scale = Mathf.Pow(Curvature, nThDerivative - 1);
                int xSgn = nThDerivative / 2 % 2 == 0 ? 1 : -1;
                int ySgn = (nThDerivative - 1) / 2 % 2 == 0 ? 1 : -1;
                bool even = nThDerivative % 2 == 0;
                x = xSgn * scale * (even ? Mathf.Sin(ang) : Mathf.Cos(ang));
                y = ySgn * scale * (even ? Mathf.Cos(ang) : Mathf.Sin(ang));
                break;
        }

        // space transformation
        return Placement.TransformVector(x, y);
    }
}
