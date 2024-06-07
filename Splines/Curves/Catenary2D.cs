using System.Numerics;
using Splines.Enums;
using Splines.Extensions;
using Splines.GeometricShapes;

namespace Splines.Curves;

/// <summary>
/// A catenary curve passing through two points with a given arc length.
/// </summary>
public partial struct Catenary2D
{
    // data
    private Vector2 _p1;
    private CatenaryToPoint2D _catenary; // stores arc length
    private Transform2D _space; // stores p0 and slack direction
    private Catenary2DEvaluability _evaluability;

    /// <summary>
    /// Gets or sets the length of the catenary curve.
    /// </summary>
    public float Length
    {
        [Pure]
        get => _catenary.Length;
        set => _catenary.Length = value; // does not change evaluability of this type, since space hasn't changed
    }

    /// <summary>
    /// Gets or sets the starting point of the catenary curve.
    /// </summary>
    public Vector2 P0
    {
        [Pure]
        get => _space.Origin;
        set
        {
            if (value != _space.Origin)
            {
                (_space.Origin, _evaluability) = (value, Catenary2DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Gets or sets the ending point of the catenary curve.
    /// </summary>
    public Vector2 P1
    {
        [Pure]
        get => _p1;
        set
        {
            if (value != _p1)
            {
                (_p1, _evaluability) = (value, Catenary2DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Gets or sets the slack direction of the catenary curve.
    /// </summary>
    public Vector2 SlackDirection
    {
        [Pure]
        get => -_space.AxisY;
        set
        {
            if (value != SlackDirection)
            {
                (_space.AxisY, _evaluability) = (-value, Catenary2DEvaluability.NotReady);
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Catenary2D"/> struct with the specified parameters.
    /// </summary>
    /// <param name="p0">The starting point of the catenary curve.</param>
    /// <param name="p1">The ending point of the catenary curve.</param>
    /// <param name="length">The length of the catenary curve.</param>
    /// <param name="slackDirection">The slack direction of the catenary curve.</param>
    public Catenary2D(Vector2 p0, Vector2 p1, float length, Vector2 slackDirection)
    {
        _space = default;
        _catenary = new CatenaryToPoint2D(p1 - p0, length);
        (_space.Origin, _p1) = (p0, p1);
        _evaluability = Catenary2DEvaluability.NotReady;
        SlackDirection = slackDirection;
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
            0 => _space.TransformPoint(_catenary.Eval(sEval, 0)).ToVector3(),
            _ => _space.TransformVector(_catenary.Eval(sEval, n)).ToVector3()
        };
    }

    /// <summary>
    /// Ensures the space transformation is ready.
    /// </summary>
    private void ReadyForEvaluation()
    {
        if (_evaluability == Catenary2DEvaluability.Ready)
        {
            return;
        }

        _catenary.P = _space.InverseTransformPoint(_p1);
        _evaluability = Catenary2DEvaluability.Ready;
    }
}
