using System.Numerics;
using Splines.Extensions;
using Splines.Splines.MultiSegmentSplines;

namespace Splines.Splines.Smoothing;

public static class NURBS4DExtensions
{
    /// <summary>
    /// Smooths the NURBS curve by averaging the control points with their neighbors.
    /// </summary>
    /// <param name="nurbs">The NURBS curve to smooth.</param>
    /// <param name="iterations">The number of smoothing iterations to perform.</param>
    /// <returns>A new NURBS4D object representing the smoothed curve.</returns>
    [Pure]
    public static NURBS4D Smooth(this NURBS4D nurbs, int iterations = 1)
    {
        if (iterations < 1)
        {
            throw new ArgumentException("Number of iterations must be at least 1.", nameof(iterations));
        }

        Vector4[] smoothedPoints = (Vector4[])nurbs.Points.Clone();

        for (int iter = 0; iter < iterations; iter++)
        {
            Vector4[] newPoints = new Vector4[nurbs.PointCount];

            // Copy the first and last control points to maintain the curve endpoints
            newPoints[0] = smoothedPoints[0];
            newPoints[nurbs.PointCount - 1] = smoothedPoints[nurbs.PointCount - 1];

            // Average the control points with their neighbors
            for (int i = 1; i < nurbs.PointCount - 1; i++)
            {
                newPoints[i] = 0.5f * smoothedPoints[i] + 0.25f * (smoothedPoints[i - 1] + smoothedPoints[i + 1]);
            }

            smoothedPoints = newPoints;
        }

        // Clone the knots and weights arrays
        var clonedKnots = nurbs.Knots.Copy();
        var clonedWeights = nurbs.Weights.CopyNullable();

        return new NURBS4D(smoothedPoints, clonedKnots, clonedWeights, nurbs.Degree);
    }
}
