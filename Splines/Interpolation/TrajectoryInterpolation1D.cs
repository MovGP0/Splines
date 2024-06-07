using System.Collections.Generic;

namespace Splines.Interpolation;

public static class TrajectoryInterpolation1D
{
    /// <summary>
    /// Interpolates the given list of points, returning the specified number of interpolated points between each pair of original points.
    /// </summary>
    /// <param name="points">The list of points to interpolate between.</param>
    /// <param name="numInterpolatedPoints">The number of interpolated points to generate between each pair of original points. Must be greater than 0.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="float"/> representing the interpolated points.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="numInterpolatedPoints"/> is less than 1.</exception>
    [Pure]
    public static IEnumerable<float> Interpolate(List<float> points, int numInterpolatedPoints)
    {
        if (numInterpolatedPoints < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(numInterpolatedPoints));
        }

        for (int i = 1; i < points.Count - 1; i++)
        {
            float position0 = points[i - 1];
            float position1 = points[i];
            float position2 = points[i + 1];

            float velocityStart = position1 - position0;
            float velocityEnd = position2 - position1;
            float acceleration = velocityEnd - velocityStart;
            float jerk = acceleration - (velocityEnd - velocityStart);

            for (int k = 0; k < numInterpolatedPoints; k++)
            {
                float time = k / (float)numInterpolatedPoints;
                float timeSquared = time * time;
                float timeCubed = timeSquared * time;

                // scale values
                var velocityStartScaled = velocityStart * time;
                var accelerationScaled = acceleration * 0.5f * timeSquared;
                var jerkScaled = jerk * (1 / 6f) * timeCubed;

                float interpolatedPosition = position1 + velocityStartScaled + accelerationScaled + jerkScaled;
                yield return interpolatedPosition;
            }
        }

        var lastPoint = points[points.Count - 1];
        yield return lastPoint;
    }
}
