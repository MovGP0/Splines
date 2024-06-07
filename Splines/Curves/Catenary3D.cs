using System.Numerics;
using Splines.Enums;
using Splines.Extensions;
using Splines.GeometricShapes;

namespace Splines.Curves;

/// <summary>
/// A catenary curve passing through two points with a given arc length.
/// </summary>
public partial struct Catenary3D
{
    // data
    private Vector3 _p1;
    private CatenaryToPoint2D _cat2D; // also stores arc length
    private Plane2DIn3D _space = default; // stores p0 and slack direction
    private Catenary3DEvaluability _evaluability;

    /// <summary>
    /// Gets or sets the length of the catenary curve.
    /// </summary>
    public float Length
    {
        [Pure]
        get => _cat2D.Length;
        set => _cat2D.Length = value; // does not change evaluability of this type, since space hasn't changed
    }

    /// <summary>
    /// Gets or sets the starting point of the catenary curve.
    /// </summary>
    public Vector3 P0
    {
        [Pure]
        get => _space.Origin;
        set
        {
            if (value != _space.Origin)
            {
                (_space.Origin, _evaluability) = (value, Catenary3DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Gets or sets the ending point of the catenary curve.
    /// </summary>
    public Vector3 P1
    {
        [Pure]
        get => _p1;
        set
        {
            if (value != _p1)
            {
                (_p1, _evaluability) = (value, Catenary3DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Gets or sets the slack direction of the catenary curve.
    /// </summary>
    public Vector3 SlackDirection
    {
        [Pure]
        get => -_space.AxisY;
        set
        {
            if (value != SlackDirection)
            {
                (_space.AxisY, _evaluability) = (-value, Catenary3DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Creates a catenary curve between two points, given an arc length <c>s</c> and a slack direction.
    /// </summary>
    /// <param name="p0">The start of the curve.</param>
    /// <param name="p1">The end of the curve.</param>
    /// <param name="length">The length of the curve. Note: has to be equal or longer than the distance between the points.</param>
    /// <param name="slackDirection">The direction of "gravity" for the arc.</param>
    public Catenary3D(Vector3 p0, Vector3 p1, float length, Vector3 slackDirection)
    {
        _cat2D = new CatenaryToPoint2D((p1 - p0).ToVector2(), length);
        _space.AxisX = default; // set on first evaluation by RotateAroundYToInclude
        (_space.Origin, _space.AxisY, this._p1) = (p0, -slackDirection, p1);
        _evaluability = Catenary3DEvaluability.NotReady;
    }

    /// <summary>
    /// Evaluates the position or derivative of the catenary curve at the given arc length.
    /// </summary>
    /// <param name="sEval">The arc length at which to evaluate.</param>
    /// <param name="n">The order of the derivative to evaluate.</param>
    /// <returns>The evaluated position or derivative at the given arc length.</returns>
    [Pure]
    public Vector3 Eval(float sEval, int n = 1)
    {
        ReadyForEvaluation();
        return n switch
        {
            0 => _space.TransformPoint(_cat2D.Eval(sEval, 0)),
            _ => _space.TransformVector(_cat2D.Eval(sEval, n))
        };
    }

    /// <summary>
    /// Ensures the space transformation is ready.
    /// </summary>
    private void ReadyForEvaluation()
    {
        if (_evaluability == Catenary3DEvaluability.Ready)
        {
            return;
        }

        // ready the embedded plane of the catenary and assign the 2D endpoint
        _space.RotateAroundYToInclude(P1, out Vector2 p1Local);
        _cat2D.P = p1Local;
        _evaluability = Catenary3DEvaluability.Ready;
    }
}
