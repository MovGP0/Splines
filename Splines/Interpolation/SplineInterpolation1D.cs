using System.Collections.Generic;
using Splines.Splines.UniformSplineSegments;

namespace Splines.Interpolation;

public static class SplineInterpolation1D
{
    /// <summary>
    /// Interpolates a list of points using Catmull-Rom spline and returns the interpolated points.
    /// </summary>
    /// <param name="points">The list of points to interpolate.</param>
    /// <param name="numInterpolatedPoints">The number of interpolated points to generate between each pair of points.</param>
    /// <returns>An enumerable collection of interpolated points.</returns>
    /// <remarks>
    /// This method uses Catmull-Rom spline interpolation to generate smooth curves through the provided points.
    /// </remarks>
    /// <example>
    /// <code>
    /// List&lt;float&gt; points = new List&lt;float&gt; { 0, 1, 2, 4 };
    /// IEnumerable&lt;float&gt; interpolatedPoints = SplineInterpolation1D.Interpolate(points, 10);
    /// </code>
    /// </example>
    [Pure]
    public static IEnumerable<float> Interpolate(List<float> points, int numInterpolatedPoints)
    {
        if (numInterpolatedPoints < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numInterpolatedPoints));
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            float p0 = i > 0 ? points[i - 1] : points[i];
            float p1 = points[i];
            float p2 = points[i + 1];
            float p3 = i < points.Count - 2 ? points[i + 2] : points[i + 1];

            for (int j = 0; j < numInterpolatedPoints; j++)
            {
                float t = j / (float)numInterpolatedPoints;
                yield return CatmullRom(p0, p1, p2, p3, t);
            }
        }

        var lastPoint = points[points.Count - 1];
        yield return lastPoint;
    }

    /// <summary>
    /// Computes a point on a Catmull-Rom spline.
    /// </summary>
    /// <param name="p0">The first control point.</param>
    /// <param name="p1">The second control point.</param>
    /// <param name="p2">The third control point.</param>
    /// <param name="p3">The fourth control point.</param>
    /// <param name="time">The interpolation parameter, typically between 0 and 1.</param>
    /// <returns>The interpolated point on the spline.</returns>
    /// <remarks>
    /// This method uses the Catmull-Rom spline formula to calculate an interpolated point.
    /// </remarks>
    /// <example>
    /// <code>
    /// float p0 = 0;
    /// float p1 = 1;
    /// float p2 = 2;
    /// float p3 = 4;
    /// float time = 0.5f; // 50%
    /// float point = SplineInterpolation1D.CatmullRom(p0, p1, p2, p3, time);
    /// </code>
    /// </example>
    [Pure]
    public static float CatmullRom(
        float p0,
        float p1,
        float p2,
        float p3,
        float time)
    {
        // create the spline segment
        var catRomSegment = new CatRomCubic1D(p0, p1, p2, p3);

        // get the polynomial curve
        var curve = catRomSegment.Curve;

        // evaluate the curve at the given location
        return curve.Eval(time);
    }
}
