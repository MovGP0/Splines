using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Splines;

internal static class OutlierTerminator
{
    /// <summary>
    /// Remove outliers, but keep the first and last points.
    /// </summary>
    /// <param name="infos">The list of points with outlier information.</param>
    /// <returns>The list of points without outliers.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IEnumerable<T> GetPointsWithoutOutliers<T>(OutlierPointInfo<T>[] infos) where T : struct
    {
        yield return infos[0].Point; // first point

        for (int i = 1; i < infos.Length - 1; i++)
        {
            if (!infos[i].IsOutlier)
            {
                yield return infos[i].Point;
            }

            // TODO: calculate interpolated points
        }

        yield return infos[infos.Length - 1].Point; // last point
    }
}
