using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.GeometricShapes;

public static class ExtILinear4D
{
    /// <summary>Gets a point along this line</summary>
    /// <param name="linear">The linear object to get a point along (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="t">The t-value along the ray to get the point of. If the ray direction is normalized, t is equivalent to distance</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 GetPoint<T>(this T linear, float t) where T : ILinear4D => linear.Origin + linear.Direction * t;

    /// <summary>Returns the t-value of a point projected onto this line</summary>
    /// <param name="linear">The linear object to project onto (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="point">The point to use when projecting</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ProjectPointTValue<T>(this T linear, Vector4 point) where T : ILinear4D => linear.ClampTValue(Line4D.ProjectPointToLineTValue(linear.Origin, linear.Direction, point));

    /// <summary>The t-values at the shortest squared distance between two linear objects</summary>
    /// <param name="linear">The linear object to check distance from (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="other">The other linear object to check the distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (float, float) LinearTValues<A, B>(this A linear, B other) where A : ILinear4D where B : ILinear4D
    {
        (float tA, float tB) = Line4D.ClosestPointBetweenLinesTValues(linear.Origin, linear.Direction, other.Origin, other.Direction);
        tA = linear.ClampTValue(tA);
        tB = other.ClampTValue(tB);
        return (tA, tB);
    }

    /// <summary>Projects a point onto this line</summary>
    /// <param name="linear">The linear object to project onto (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="point">The point to project</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 ProjectPoint<T>(this T linear, Vector4 point) where T : ILinear4D => linear.GetPoint(linear.ProjectPointTValue(point));

    /// <summary>The shortest distance from this line to a point</summary>
    /// <param name="linear">The linear object to check distance from (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="point">The point to check the distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance<T>(this T linear, Vector4 point) where T : ILinear4D => Mathf.Sqrt(DistanceSqr(linear, point));

    /// <summary>The shortest squared distance from this line to a point</summary>
    /// <param name="linear">The linear object to check distance from (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="point">The point to check the distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSqr<T>(this T linear, Vector4 point) where T : ILinear4D => (point - linear.ProjectPoint(point)).SqrMagnitude();

    /// <summary>The shortest squared distance from this line to another line</summary>
    /// <param name="linear">The linear object to check distance from (Ray4D, Line4D or LineSegment4D)</param>
    /// <param name="other">The other linear object to check the distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float DistanceSqr<A, B>(this A linear, B other) where A : ILinear4D where B : ILinear4D
    {
        (float tA, float tB) = LinearTValues(linear, other);
        return (linear.GetPoint(tA) - other.GetPoint(tB)).SqrMagnitude();
    }
}
