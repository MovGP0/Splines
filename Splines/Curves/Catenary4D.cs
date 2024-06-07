using System.Numerics;
using Splines.Enums;
using Splines.Extensions;
using Splines.GeometricShapes;

namespace Splines.Curves;

/// <summary>
/// A catenary curve passing through two points with a given arc length in 4D space.
/// </summary>
public partial struct Catenary4D
{
    // data
    private Vector4 _p1;
    private CatenaryToPoint3D _cat3D; // also stores arc length
    private Plane3DIn4D _space = default; // stores p0 and slack direction
    private Catenary4DEvaluability _evaluability;

    /// <summary>
    /// Gets or sets the length of the catenary curve.
    /// </summary>
    public float Length
    {
        [Pure]
        get => _cat3D.Length;
        set => _cat3D.Length = value; // does not change evaluability of this type, since space hasn't changed
    }

    /// <summary>
    /// Gets or sets the starting point of the catenary curve.
    /// </summary>
    public Vector4 P0
    {
        [Pure]
        get => _space.Origin;
        set
        {
            if (value != _space.Origin)
            {
                (_space.Origin, _evaluability) = (value, Catenary4DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Gets or sets the ending point of the catenary curve.
    /// </summary>
    public Vector4 P1
    {
        [Pure]
        get => _p1;
        set
        {
            if (value != _p1)
            {
                (_p1, _evaluability) = (value, Catenary4DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Gets or sets the slack direction of the catenary curve.
    /// </summary>
    public Vector4 SlackDirection
    {
        [Pure]
        get => -_space.AxisY;
        set
        {
            if (value != SlackDirection)
            {
                (_space.AxisY, _evaluability) = (-value, Catenary4DEvaluability.NotReady);
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
    public Catenary4D(Vector4 p0, Vector4 p1, float length, Vector4 slackDirection)
    {
        _cat3D = new CatenaryToPoint3D((p1 - p0).ToVector3(), length);
        _space.AxisX = default; // set on first evaluation by RotateAroundYToInclude
        (_space.Origin, _space.AxisY, this._p1) = (p0, -slackDirection, p1);
        _evaluability = Catenary4DEvaluability.NotReady;
    }

    /// <summary>
    /// Evaluates the position or derivative of the catenary curve at the given arc length.
    /// </summary>
    /// <param name="sEval">The arc length at which to evaluate.</param>
    /// <param name="n">The order of the derivative to evaluate.</param>
    /// <returns>The evaluated position or derivative at the given arc length.</returns>
    [Pure]
    public Vector4 Eval(float sEval, int n = 1)
    {
        ReadyForEvaluation();
        return n switch
        {
            0 => _space.TransformPoint(_cat3D.Eval(sEval, 0)),
            _ => _space.TransformVector(_cat3D.Eval(sEval, n))
        };
    }

    /// <summary>
    /// Ensures the space transformation is ready.
    /// </summary>
    private void ReadyForEvaluation()
    {
        if (_evaluability == Catenary4DEvaluability.Ready)
        {
            return;
        }

        // ready the embedded plane of the catenary and assign the 3D endpoint
        _space.RotateAroundYToInclude(P1, out Vector3 p1Local);
        _cat3D.P = p1Local;
        _evaluability = Catenary4DEvaluability.Ready;
    }
}
