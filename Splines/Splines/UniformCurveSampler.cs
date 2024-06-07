using System.Collections.Generic;
using System.Numerics;
using Splines.Curves;
using Splines.Extensions;
using Splines.Numerics;
using static Splines.Mathfs;

namespace Splines.Splines;

/// <summary>
/// A helper class to let you sample a curve by uniform parameter values, t-values, or by distance.
/// </summary>
public sealed class UniformCurveSampler
{
    /// <summary>The number of distance samples used when calculating the cumulative distance table.</summary>
    public int Resolution { get; }

    private readonly float[] _cumulativeDistances;

    /// <summary>
    /// Cumulative distance samples, where the first element is 0 and the last element is the total length of the curve.
    /// </summary>
    public IReadOnlyCollection<float> CumulativeDistances => Array.AsReadOnly(_cumulativeDistances);

    /// <summary>
    /// Returns the approximate length of this curve. This property doesn't have to recalculate length since it's already stored in the cumulative distances array.
    /// </summary>
    public float CurveIntervalLength => _cumulativeDistances[Resolution - 1];

    /// <summary>The t-value range in which we've calculated the uniform sampler for.</summary>
    [Pure]
    public FloatRange ParamInterval { get; private set; }

    /// <summary>
    /// Creates a sampler that can be used to sample a curve by distance or by uniform t-values.
    /// You'll need to call <c>Recalculate(curve)</c> to recalculate if the curve changes shape after this point.
    /// Recommended resolution for animation: [8-16]
    /// Recommended resolution for even point spacing: [16-50]
    /// </summary>
    /// <param name="curve">The curve to use when sampling.</param>
    /// <param name="interval">The interval you want to uniformly sample within.</param>
    /// <param name="resolution">
    /// The accuracy of this sampler. Higher values are more accurate but are more costly to calculate for every new curve shape.
    /// </param>
    public UniformCurveSampler(Polynomial2D curve, FloatRange interval, int resolution = 12)
    {
        Resolution = resolution;
        _cumulativeDistances = new float[resolution];
        Recalculate(curve, interval);
    }

    /// <inheritdoc cref="UniformCurveSampler(Polynomial2D, FloatRange, int)"/>
    public UniformCurveSampler(Polynomial2D curve, int resolution = 12) : this(curve, FloatRange.Unit, resolution)
    {
    }

    /// <inheritdoc cref="UniformCurveSampler(Polynomial2D, FloatRange, int)"/>
    public UniformCurveSampler(Polynomial3D curve, FloatRange interval, int resolution = 12)
    {
        Resolution = resolution;
        _cumulativeDistances = new float[resolution];
        Recalculate(curve, interval);
    }

    /// <inheritdoc cref="UniformCurveSampler(Polynomial3D, FloatRange, int)"/>
    public UniformCurveSampler(Polynomial3D curve, int resolution = 12) : this(curve, FloatRange.Unit, resolution)
    {
    }

    /// <summary>
    /// Recalculates the internal lookup table so that the curve can be sampled by distance or by uniform t-values.
    /// Only call this before sampling a different curve, or if the curve has changed shape since the last time it was calculated.
    /// </summary>
    /// <param name="curve">The curve to use when sampling.</param>
    /// <param name="interval">The interval you want to uniformly sample within.</param>
    public void Recalculate(Polynomial2D curve, FloatRange interval)
    {
        ParamInterval = interval;
        float cumulativeLength = 0;
        Vector2 prevPt = curve.Eval(ParamInterval.Start);
        _cumulativeDistances[0] = 0;
        for (int i = 1; i < Resolution; i++)
        {
            Vector2 pt = curve.Eval(ParamInterval.Lerp(i / (Resolution - 1f)));
            cumulativeLength += Vector2.Distance(prevPt, pt);
            _cumulativeDistances[i] = cumulativeLength;
            prevPt = pt;
        }
    }

    /// <inheritdoc cref="Recalculate(Polynomial2D, FloatRange)"/>
    public void Recalculate(Polynomial2D curve) => Recalculate(curve, FloatRange.Unit);

    /// <inheritdoc cref="Recalculate(Polynomial2D, FloatRange)"/>
    public void Recalculate(Polynomial3D curve, FloatRange interval)
    {
        ParamInterval = interval;
        float cumulativeLength = 0;
        Vector3 prevPt = curve.Eval(ParamInterval.Start);
        _cumulativeDistances[0] = 0;
        for (int i = 1; i < Resolution; i++)
        {
            Vector3 pt = curve.Eval(ParamInterval.Lerp(i / (Resolution - 1f)));
            cumulativeLength += Vector3.Distance(prevPt, pt);
            _cumulativeDistances[i] = cumulativeLength;
            prevPt = pt;
        }
    }

    /// <inheritdoc cref="Recalculate(Polynomial3D, FloatRange)"/>
    public void Recalculate(Polynomial3D curve) => Recalculate(curve, FloatRange.Unit);

    /// <summary>Converts a t-value along the segment to a raw parameter value. Useful to uniformly sample a curve.</summary>
    /// <param name="t">A value from 0 to 1 representing uniform position along the curve interval.</param>
    [Pure]
    public float UniformTToParam(float t) => DistanceToParam(t * CurveIntervalLength);

    /// <summary>Converts a uniform parameter value to a raw parameter value. Useful to uniformly sample a curve.</summary>
    /// <param name="u">The input parameter for uniform position along the curve.</param>
    [Pure]
    public float UniformParamToParam(float u) => DistanceToParam(ParamInterval.InverseLerp(u) * CurveIntervalLength);

    /// <summary>
    /// Converts a distance value (relative to the start of the interval) to a parameter value. Useful to sample a curve by distance.
    /// </summary>
    /// <param name="distance">
    /// The distance along the curve segment parameter interval at which you'd like to get the parameter value for.
    /// </param>
    [Pure]
    public float DistanceToParam(float distance)
    {
        if (distance > 0 && distance < CurveIntervalLength)
        {
            for (int i = 0; i < Resolution - 1; i++)
            {
                float distPrev = _cumulativeDistances[i];
                float distNext = _cumulativeDistances[i + 1];
                if (distance.Within(distPrev, distNext))
                {
                    float tPrev = i / (Resolution - 1f);
                    float tNext = (i + 1) / (Resolution - 1f);
                    float tLocal = Remap(distance, distPrev, distNext, tPrev, tNext);
                    return ParamInterval.Lerp(tLocal);
                }
            }
        }

        return ParamInterval.Lerp(distance / CurveIntervalLength);
    }

    /// <summary>
    /// Converts a t-value along the segment to a raw parameter value. Useful to uniformly sample a curve.
    /// </summary>
    /// <param name="t">A value from 0 to 1 representing uniform position along the curve interval.</param>
    [Pure]
    public float GetParamAtSegmentTValue(float t) => ParamInterval.InverseLerp(t);
}
