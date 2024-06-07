using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.UtilityTypes;

namespace Splines.GeometricShapes;

/// <summary>A 2D circle with a centerpoint and a radius</summary>
[Serializable]
public partial struct Circle2D
{
    /// <summary>The center of the circle</summary>
    public Vector2 Center
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The radius of the circle</summary>
    public float Radius
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a 2D Circle</summary>
    /// <param name="center">The center of the circle</param>
    /// <param name="radius">The radius of the circle</param>
    public Circle2D(Vector2 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    /// <summary>Calculates the area of a circle, given its radius</summary>
    /// <param name="r">The radius</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float RadiusToArea(float r) => r * r * (0.5f * Mathfs.TAU);

    /// <summary>Calculates the radius of a circle, given its area</summary>
    /// <param name="area">The area</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float AreaToRadius(float area) => Mathfs.Sqrt(2 * area / Mathfs.TAU);

    /// <summary>Calculates the circumference of a circle, given its area</summary>
    /// <param name="area">The area</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float AreaToCircumference(float area) => Mathfs.Sqrt(2 * area / Mathfs.TAU) * Mathfs.TAU;

    /// <summary>Calculates the area of a circle, given its circumference</summary>
    /// <param name="c">The circumference</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float CircumferenceToArea(float c) => c * c / (2 * Mathfs.TAU);

    /// <summary>Calculates the circumference of a circle, given its radius</summary>
    /// <param name="r">The radius</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float RadiusToCircumference(float r) => r * Mathfs.TAU;

    /// <summary>Calculates the radius of a circle, given its circumference</summary>
    /// <param name="c">The circumference</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float CircumferenceToRadius(float c) => c / Mathfs.TAU;

    /// <summary>Returns the osculating circle of a point in a curve. Osculating circles are defined everywhere except on inflection points, where curvature is 0</summary>
    /// <param name="point">The point of the curve</param>
    /// <param name="velocity">The first derivative of the point in the curve</param>
    /// <param name="acceleration">The second derivative of the point in the curve</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Circle2D GetOsculatingCircle(Vector2 point, Vector2 velocity, Vector2 acceleration)
    {
        float curvature = velocity.GetCurvature(acceleration);
        Vector2 tangent = velocity.Normalized();
        Vector2 normal = tangent.Rotate90CCW();
        float signedRadius = 1f / curvature;
        return new Circle2D(point + normal * signedRadius, Mathfs.Abs(signedRadius));
    }

    /// <summary>
    /// Scales the circle by a given value. The center and radius of the circle are both multiplied by the specified value.
    /// </summary>
    /// <param name="circle">The circle to be scaled.</param>
    /// <param name="value">The value to scale the circle by.</param>
    /// <returns>A new circle with the center and radius scaled by the given value.</returns>
    public static Circle2D operator *(Circle2D circle, float value) => new(circle.Center * value, circle.Radius * value);

    /// <summary>
    /// Scales the circle by a given value. The center and radius of the circle are both multiplied by the specified value.
    /// </summary>
    /// <param name="value">The value to scale the circle by.</param>
    /// <param name="circle">The circle to be scaled.</param>
    /// <returns>A new circle with the center and radius scaled by the given value.</returns>
    public static Circle2D operator *(float value, Circle2D circle) => new(circle.Center * value, circle.Radius * value);

    /// <summary>Returns whether this circle intersects a linear object</summary>
    /// <param name="linear">The linear object to test intersection against (Ray2D, Line2D or LineSegment2D)</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public bool Intersects<T>(T linear) where T : ILinear2D =>
        IntersectionTest.LinearCircleIntersects(linear, this);

    /// <summary>Returns whether this circle intersects another circle</summary>
    /// <param name="circle">The circle to test intersection against</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public bool Intersects(Circle2D circle) =>
        IntersectionTest.CirclesOverlap(Center, Radius, circle.Center, circle.Radius);

    /// <summary>Returns the intersections this circle has with a linear object (if any)</summary>
    /// <param name="linear">The linear object to test intersection against (Ray2D, Line2D or LineSegment2D)</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public ResultsMax2<Vector2> Intersect<T>(T linear) where T : ILinear2D =>
        IntersectionTest.LinearCircleIntersectionPoints(linear, this);

    /// <summary>Returns the intersections this circle has with another circle (if any)</summary>
    /// <param name="circle">The circle to test intersection against</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public ResultsMax2<Vector2> Intersect(Circle2D circle) =>
        IntersectionTest.CirclesIntersectionPoints(Center, Radius, circle.Center, circle.Radius);

    /// <summary>Returns a circle passing through the start with a given tangent direction, and the end point, if possible.
    /// Note: if the tangent points directly toward the second point, no valid circle exists</summary>
    /// <param name="startPt">The first point on the circle</param>
    /// <param name="startTangent">The tangent direction of the circle at the first point</param>
    /// <param name="endPt">The second point on the circle</param>
    /// <param name="circle">The circle passing through the start with a given tangent direction, and the end point, if possible</param>
    public static bool FromPointTangentPoint(Vector2 startPt, Vector2 startTangent, Vector2 endPt, out Circle2D circle)
    {
        Line2D lineA = new Line2D(startPt, startTangent.Rotate90CW());
        Line2D lineB = LineSegment2D.GetBisector(startPt, endPt);
        if (lineA.Intersect(lineB, out Vector2 pt))
        {
            circle = new Circle2D(pt, Vector2.Distance(pt, startPt));
            return true;
        }

        circle = default;
        return false;
    }

    /// <summary>Returns a circle passing through all three points. Note: if the three points are collinear, no valid circle exists</summary>
    /// <param name="a">The first point on the circle</param>
    /// <param name="b">The second point on the circle</param>
    /// <param name="c">The third point on the circle</param>
    /// <param name="circle">The circle passing through all three points</param>
    [Pure]
    public static bool FromThreePoints(Vector2 a, Vector2 b, Vector2 c, out Circle2D circle)
    {
        Line2D lineA = LineSegment2D.GetBisector(a, b);
        Line2D lineB = LineSegment2D.GetBisector(b, c);
        circle = default;
        if (lineA.Intersect(lineB, out var center))
        {
            circle.Center = center;
            circle.Radius = Vector2.Distance(center, a);
            return true;
        }

        return false;
    }

    /// <summary>Returns the smallest possible circle passing through two points</summary>
    /// <param name="a">The first point in the circle</param>
    /// <param name="b">The second point in the circle</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Circle2D FromTwoPoints(Vector2 a, Vector2 b) =>
        new((a + b) / 2f, Vector2.Distance(a, b) / 2f);

    /// <summary>Returns whether the point is inside the circle</summary>
    /// <param name="point">The point to check if it's inside or outside</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public bool Contains(Vector2 point)
    {
        float dx = point.X - Center.X;
        float dy = point.Y - Center.Y;
        return dx * dx + dy * dy <= Radius * Radius;
    }

    /// <summary>Projects a point to the nearest point on the circle. Points inside the circle will be pushed out to the boundary</summary>
    /// <param name="point">The point to project</param>
    [Pure]
    public Vector2 ProjectPoint(Vector2 point)
    {
        Vector2 v = point - Center;
        float mag = v.Magnitude();
        return Center + v * (Radius / mag);
    }

    /// <summary>Get or set the circumference of this circle</summary>
    public float Circumference
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => RadiusToCircumference(Radius);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Radius = CircumferenceToRadius(value);
    }

    /// <summary>Get or set the area of this circle</summary>
    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => RadiusToArea(Radius);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Radius = AreaToRadius(value);
    }
}
