using System.Collections.Generic;
using System.Numerics;

namespace Splines.Interpolation;

/// <summary>
/// Provides methods for interpolating movements between points.
/// </summary>
public static class TrajectoryInterpolation2D
{
    /// <summary>
    /// Interpolates the given list of points, returning the specified number of interpolated points between each pair of original points.
    /// </summary>
    /// <param name="points">The list of points to interpolate between.</param>
    /// <param name="numInterpolatedPoints">The number of interpolated points to generate between each pair of original points. Must be greater than 0.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Vector2"/> representing the interpolated points.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="numInterpolatedPoints"/> is less than 1.</exception>
    [Pure]
    public static IEnumerable<Vector2> Interpolate(List<Vector2> points, int numInterpolatedPoints)
    {
        if (numInterpolatedPoints < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numInterpolatedPoints));
        }

        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector2 position0 = points[i - 1];
            Vector2 position1 = points[i];
            Vector2 position2 = points[i + 1];

            Vector2 velocityStart = position1 - position0;
            Vector2 velocityEnd = position2 - position1;
            Vector2 acceleration = velocityEnd - velocityStart;
            Vector2 jerk = acceleration - (velocityEnd - velocityStart);

            for (int k = 0; k < numInterpolatedPoints; k++)
            {
                float time = k / (float)numInterpolatedPoints;
                float timeSquared = time * time;
                float timeCubed = timeSquared * time;

                // scale values
                var velocityStartScaled = velocityStart * time;
                var accelerationScaled = acceleration * 0.5f * timeSquared;
                var jerkScaled = jerk * (1 / 6f) * timeCubed;

                Vector2 interpolatedPosition = position1 + velocityStartScaled + accelerationScaled + jerkScaled;
                yield return interpolatedPosition;
            }
        }

        var lastPoint = points[points.Count - 1];
        yield return lastPoint;
    }
}
