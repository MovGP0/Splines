using System.Collections.Generic;
using System.Numerics;
using Splines.Splines.UniformSplineSegments;

namespace Splines.Interpolation;

public static class SplineInterpolation2D
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
    /// List&lt;Vector2&gt; points = new List&lt;Vector2&gt; { new Vector2(0, 0), new Vector2(1, 2), new Vector2(2, 3), new Vector2(4, 5) };
    /// IEnumerable&lt;Vector2&gt; interpolatedPoints = SplineInterpolation.Interpolate(points, 10);
    /// </code>
    /// </example>
    [Pure]
    public static IEnumerable<Vector2> Interpolate(List<Vector2> points, int numInterpolatedPoints)
    {
        if (numInterpolatedPoints < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numInterpolatedPoints));
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 p0 = i > 0 ? points[i - 1] : points[i];
            Vector2 p1 = points[i];
            Vector2 p2 = points[i + 1];
            Vector2 p3 = i < points.Count - 2 ? points[i + 2] : points[i + 1];

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
    /// Vector2 p0 = new(0, 0);
    /// Vector2 p1 = new(1, 2);
    /// Vector2 p2 = new(2, 3);
    /// Vector2 p3 = new(4, 5);
    /// float time = 0.5f; // 50%
    /// Vector2 point = SplineInterpolation.CatmullRom(p0, p1, p2, p3, time);
    /// </code>
    /// </example>
    [Pure]
    public static Vector2 CatmullRom(
        Vector2 p0,
        Vector2 p1,
        Vector2 p2,
        Vector2 p3,
        float time)
    {
        // create the spline segment
        var catRomSegment = new CatRomCubic2D(p0, p1, p2, p3);

        // get the polynomial curve
        var curve = catRomSegment.Curve;

        // evaluate the curve at the given location
        return curve.Eval(time);
    }
}
