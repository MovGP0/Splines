using System.Collections.Generic;
using System.Linq;
using Splines.Physics;

namespace Splines;

public static class OutlierTerminator1D
{
    [Pure]
    public static IEnumerable<float> EliminateOutliers(List<float> points, float deviationThresholdFactor = 0.3f)
    {
        if (points.Count < 3)
        {
            return points;
        }

        var infos = new OutlierPointInfo1D[points.Count];
        for (var i = 0; i < infos.Length; i++)
        {
            infos[i] = new OutlierPointInfo1D(points[i]);
        }

        CalculateAccelerations(infos);

        var accelerations = infos.Select(e => e.AccelerationMagnitude).Where(e => !float.IsNaN(e)).ToArray();
        var (mean, stdDev) = accelerations.MeanAndStandardDeviation();
        float threshold = mean + deviationThresholdFactor * stdDev;

        foreach (var info in infos)
        {
            if (float.IsNaN(info.AccelerationMagnitude)) continue;
            if (info.AccelerationMagnitude > threshold) info.IsOutlier = true;
        }

        return OutlierTerminator.GetPointsWithoutOutliers(infos);
    }

    private static void CalculateAccelerations(OutlierPointInfo1D[] infos)
    {
        for (int i = 1; i < infos.Length - 1; i++)
        {
            var prev = infos[i - 1].Point;
            var cur = infos[i].Point;
            var next = infos[i + 1].Point;

            infos[i].AccelerationMagnitude = AccelerationCalculator.CalculateAcceleration(prev, cur, next);
        }
    }
}
