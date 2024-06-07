using System.Runtime.CompilerServices;

namespace Splines.GeometricShapes;

public static class ExtILinear1D
{
    /// <summary>Gets a point along this line</summary>
    /// <param name="linear">The linear object to get a point along (Ray1D, Line1D or LineSegment1D)</param>
    /// <param name="t">The t-value along the line to get the point of. If the direction is normalized, t is equivalent to distance</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetPoint<T>(this T linear, float t) where T : ILinear1D => linear.Origin + linear.Direction * t;

    /// <summary>Returns the t-value of a point projected onto this line</summary>
    /// <param name="linear">The linear object to project onto (Ray1D, Line1D or LineSegment1D)</param>
    /// <param name="point">The point to use when projecting</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ProjectPointTValue<T>(this T linear, float point) where T : ILinear1D => linear.ClampTValue((point - linear.Origin) / linear.Direction);

    /// <summary>Projects a point onto this line</summary>
    /// <param name="linear">The linear object to project onto (Ray1D, Line1D or LineSegment1D)</param>
    /// <param name="point">The point to project</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ProjectPoint<T>(this T linear, float point) where T : ILinear1D => linear.GetPoint(linear.ProjectPointTValue(point));

    /// <summary>The shortest distance from this line to a point</summary>
    /// <param name="linear">The linear object to check distance from (Ray1D, Line1D or LineSegment1D)</param>
    /// <param name="point">The point to check the distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance<T>(this T linear, float point) where T : ILinear1D => Math.Abs(DistanceSqr(linear, point));

    /// <summary>The shortest squared distance from this line to a point</summary>
    /// <param name="linear">The linear object to check distance from (Ray1D, Line1D or LineSegment1D)</param>
    /// <param name="point">The point to check the distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSqr<T>(this T linear, float point) where T : ILinear1D
    {
        float projectedPoint = linear.ProjectPoint(point);
        float distance = point - projectedPoint;
        return distance * distance;
    }
}
