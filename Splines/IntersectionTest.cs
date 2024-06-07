using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.GeometricShapes;
using Splines.Unity;
using Splines.UtilityTypes;
using static Splines.Mathfs;

namespace Splines;

/// <summary>Core intersection test functions.
/// Note: these are pretty esoteric, generally it's easier to use the instance methods in each shape,
/// such as <c>myLine.Intersect(otherThing)</c></summary>
public static class IntersectionTest
{
    // internal
    const float PARALLEL_DETERMINANT_THRESHOLD = 0.00001f;

    /// <summary>Returns whether these infinite lines intersect, and if they do, also returns the t-value for each infinite line</summary>
    /// <param name="aOrigin">First line origin</param>
    /// <param name="aDir">First line direction (does not have to be normalized)</param>
    /// <param name="bOrigin">Second line origin</param>
    /// <param name="bDir">Second line direction (does not have to be normalized)</param>
    /// <param name="tA">The t-value along the first line, where the intersection happened</param>
    /// <param name="tB">The t-value along the second line, where the intersection happened</param>
    public static bool LinearTValues(
        Vector2 aOrigin,
        Vector2 aDir,
        Vector2 bOrigin,
        Vector2 bDir,
        out float tA,
        out float tB)
    {
        float d = Determinant(aDir, bDir);
        if (Abs(d) < PARALLEL_DETERMINANT_THRESHOLD)
        {
            tA = tB = default;
            return false;
        }

        Vector2 aToB = bOrigin - aOrigin;
        tA = Determinant(aToB, bDir) / d;
        tB = Determinant(aToB, aDir) / d;
        return true;
    }

    // based on https://stackoverflow.com/questions/1073336/circle-line-segment-collision-detection-algorithm
    /// <summary>Returns the intersections between an infinite line and a circle in the form of t-values along the line where the intersections lie. Or none, if there are none</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="circleOrigin">Center or the circle</param>
    /// <param name="radius">Radius of the circle</param>
    public static ResultsMax2<float> LinearCircleTValues(Vector2 lineOrigin, Vector2 lineDir, Vector2 circleOrigin, float radius) {
        Vector2 circleToLineOrigin = lineOrigin - circleOrigin;
        float a = Vector2.Dot(lineDir, lineDir); // ray len sq
        float b = 2 * Vector2.Dot(circleToLineOrigin, lineDir);
        float c = Vector2.Dot(circleToLineOrigin, circleToLineOrigin) - radius.Square();
        float discriminant = b * b - 4 * a * c;
        if (discriminant > 0)
        {
            discriminant = Sqrt(discriminant);
            if (discriminant < 0.00001f) // line is tangent to the circle, one intersection
                return new ResultsMax2<float>(-b / (2 * a));
            float tA = (-b + discriminant) / (2 * a);
            float tB = (-b - discriminant) / (2 * a); // line has two intersections
            return new ResultsMax2<float>(tA, tB);
        }

        return default; // line doesn't hit it at all
    }

    /// <summary>Returns whether two circles intersect, and the two intersection points (if they exist)</summary>
    /// <param name="aPos">The position of the first circle</param>
    /// <param name="aRadius">The radius of the first circle</param>
    /// <param name="bPos">The position of the second circle</param>
    /// <param name="bRadius">The radius of the second circle</param>
    public static ResultsMax2<Vector2> CirclesIntersectionPoints(Vector2 aPos, float aRadius, Vector2 bPos, float bRadius) {
        float distSq = DistanceSquared(aPos, bPos);
        float dist = Sqrt(distSq);
        bool differentPosition = dist > 0.00001f;
        float maxRad = Max(aRadius, bRadius);
        float minRad = Min(aRadius, bRadius);
        bool ringsTouching = Mathf.Abs(dist - maxRad) < minRad;

        if (ringsTouching && differentPosition) {
            float aRadSq = aRadius * aRadius;
            float bRadSq = bRadius * bRadius;
            float lateralOffset = (distSq - bRadSq + aRadSq) / (2 * dist);
            float normalOffset = (0.5f / dist) * Sqrt(4 * distSq * aRadSq - (distSq - bRadSq + aRadSq).Square());
            Vector2 tangent = (bPos - aPos) / dist;
            Vector2 normal = tangent.Rotate90CCW();
            Vector2 chordCenter = aPos + tangent * lateralOffset;
            if (normalOffset < 0.00001f)
                return new ResultsMax2<Vector2>(chordCenter); // double intersection at one point
            return new ResultsMax2<Vector2>(// two intersections
                chordCenter + normal * normalOffset,
                chordCenter - normal * normalOffset
           );
        }

        return default; // no intersections
    }

    /// <summary>Returns whether two circles overlap</summary>
    /// <param name="aPos">The position of the first circle</param>
    /// <param name="aRadius">The radius of the first circle</param>
    /// <param name="bPos">The position of the second circle</param>
    /// <param name="bRadius">The radius of the second circle</param>
    public static bool CirclesOverlap(Vector2 aPos, float aRadius, Vector2 bPos, float bRadius) {
        float dist = Vector2.Distance(aPos, bPos);
        float maxRad = Max(aRadius, bRadius);
        float minRad = Min(aRadius, bRadius);
        return Mathf.Abs(dist - maxRad) < minRad;
    }

    /// <summary>Returns whether a line passes through a box centered at (0,0)</summary>
    /// <param name="extents">Box extents/"radius" per axis</param>
    /// <param name="pt">A point in the line</param>
    /// <param name="dir">The direction of the line</param>
    public static bool LineRectOverlap(Vector2 extents, Vector2 pt, Vector2 dir) {
        Vector2 corner = new Vector2(extents.X, extents.Y * -Sign(dir.X * dir.Y));
        return SignAsInt(Determinant(dir, corner - pt)) != SignAsInt(Determinant(dir, -corner - pt));
    }

    /// <summary>Returns the intersection points of a line passing through a box</summary>
    /// <param name="center">Center of the box</param>
    /// <param name="extents">Box extents/"radius" per axis</param>
    /// <param name="pt">A point in the line</param>
    /// <param name="dir">The direction of the line</param>
    public static ResultsMax2<Vector2> LinearRectPoints(Vector2 center, Vector2 extents, Vector2 pt, Vector2 dir) {
        const float FLAT_THRESH = 0.000001f;

        // place the line relative to the box
        pt.X -= center.X;
        pt.Y -= center.Y;

        // Vertical line
        if (dir.X.Abs() < FLAT_THRESH)
        {
            if (pt.X.Abs() <= extents.X) // inside - two intersections
            {
                return new ResultsMax2<Vector2>(
                    new Vector2(center.X + pt.X, center.Y - extents.Y),
                    new Vector2(center.X + pt.X, center.Y + extents.Y));
            }

            return default; // outside the box
        }

        // Horizontal line
        if (dir.Y.Abs() < FLAT_THRESH)
        {
            if (pt.Y.Abs() <= extents.Y) // inside - two intersections
            {
                return new ResultsMax2<Vector2>(
                    new Vector2(center.X - extents.X, center.Y + pt.Y),
                    new Vector2(center.X + extents.X, center.Y + pt.Y));
            }

            return default; // outside the box
        }

        // slope intercept form y = ax+b
        float a = dir.Y / dir.X;
        float b = pt.Y - pt.X * a;

        // y coords on vertical lines
        float xpy = a * extents.X + b; // x = extents.X
        float xny = -a * extents.X + b; // x = -extents.X
        // x coords on horizontal lines
        float ypx = (extents.Y - b) / a; // y = extents.Y
        float ynx = (-extents.Y - b) / a; // y = -extents.Y

        // validity checks
        bool xp = Abs(xpy) <= extents.Y;
        bool xn = Abs(xny) <= extents.Y;
        bool yp = Abs(ypx) <= extents.X;
        bool yn = Abs(ynx) <= extents.X;

        if ((xp || xn || yp || yn) == false)
            return default; // no intersections

        float ax, ay, bx, by;
        if (a > 0) { // positive slope means we group results in (x,y) and (-x,-y)
            ax = xp ? extents.X : ypx;
            ay = xp ? xpy : extents.Y;
            bx = xn ? -extents.X : ynx;
            by = xn ? xny : -extents.Y;
        } else { // negative slope means we group results in (x,-y) and (-x,y)
            ax = xp ? extents.X : ynx;
            ay = xp ? xpy : -extents.Y;
            bx = xn ? -extents.X : ypx;
            by = xn ? xny : extents.Y;
        }

        // if the points are very close, this means we hit a corner and we should return only one point
        if (Abs(ax - bx) + Abs(ay - by) < 0.000001f)
            return new ResultsMax2<Vector2>(new Vector2(center.X + ax, center.Y + ay));

        // else, two points
        return new ResultsMax2<Vector2>(
            new Vector2(center.X + ax, center.Y + ay),
            new Vector2(center.X + bx, center.Y + by));
    }

    /// <summary>Returns whether two discs overlap. Unlike circles, discs overlap even if one is smaller and is completely inside the other</summary>
    /// <param name="aPos">The position of the first disc</param>
    /// <param name="aRadius">The radius of the first disc</param>
    /// <param name="bPos">The position of the second disc</param>
    /// <param name="bRadius">The radius of the second disc</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DiscsOverlap(Vector2 aPos, float aRadius, Vector2 bPos, float bRadius) {
        return DistanceSquared(aPos, bPos) <= (aRadius + bRadius).Square();
    }

    #region Linear-Linear

    /// <summary>Returns the t-value of each linear 2D type (Ray2D, Line2D or LineSegment2D) where they intersect, if at all</summary>
    /// <param name="a">The first Ray, Line or LineSegment</param>
    /// <param name="b">The second Ray, Line or LineSegment</param>
    /// <param name="tA">The t-value of the intersection of the first linear type</param>
    /// <param name="tB">The t-value of the intersection of the second linear type</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LinearTValues<T, U>(T a, U b, out float tA, out float tB) where T : ILinear2D where U : ILinear2D {
        return IntersectionTest.LinearTValues(a.Origin, a.Direction, b.Origin, b.Direction, out tA, out tB) && a.IsValidTValue(tA) && b.IsValidTValue(tB);
    }

    /// <summary>Returns whether two linear 2D types (Ray2D, Line2D or LineSegment2D) intersect</summary>
    /// <param name="a">The first Ray, Line or LineSegment</param>
    /// <param name="b">The second Ray, Line or LineSegment</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Linear<T, U>(T a, U b) where T : ILinear2D where U : ILinear2D => LinearTValues(a, b, out _, out _);

    /// <summary>Returns whether two linear 2D types (Ray2D, Line2D or LineSegment2D) intersect, and the point where they did, if they did</summary>
    /// <param name="a">The first Ray, Line or LineSegment</param>
    /// <param name="b">The second Ray, Line or LineSegment</param>
    /// <param name="intersectionPoint">The point at which they intersect</param>
    public static bool LinearIntersectionPoint<T, U>(T a, U b, out Vector2 intersectionPoint) where T : ILinear2D where U : ILinear2D {
        bool intersects = LinearTValues(a, b, out float tA, out _);
        intersectionPoint = intersects ? a.GetPoint(tA) : default;
        return intersects;
    }

    #endregion

    #region Linear-Circle

    /// <summary>Returns the t-values of a linear 2D type (Ray2D, Line2D or LineSegment2D) where it would intersect a 2D circle, if at all</summary>
    /// <param name="linear">The Ray, Line or LineSegment to test intersection with</param>
    /// <param name="circle">The circle to test intersection with</param>
    public static ResultsMax2<float> LinearCircleTValues<T>(T linear, Circle2D circle) where T : ILinear2D {
        ResultsMax2<float> values = IntersectionTest.LinearCircleTValues(linear.Origin, linear.Direction, circle.Center, circle.Radius);
        if (values.count == 0) return default;
        bool aValid = linear.IsValidTValue(values.a);
        bool bValid = linear.IsValidTValue(values.b);
        if (aValid && bValid) return values;
        if (aValid) return new ResultsMax2<float>(values.a);
        if (bValid) return new ResultsMax2<float>(values.b);
        return default;
    }

    /// <summary>Returns whether a linear 2D type (Ray2D, Line2D or LineSegment2D) intersects with a 2D circle</summary>
    /// <param name="linear">The Ray, Line or LineSegment to test intersection with</param>
    /// <param name="circle">The circle to test intersection with</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LinearCircleIntersects<T>(T linear, Circle2D circle) where T : ILinear2D => LinearCircleTValues(linear, circle).count > 0;

    /// <summary>Returns the intersection points between a linear 2D type (Ray2D, Line2D or LineSegment2D) and a 2D circle, if any</summary>
    /// <param name="linear">The Ray, Line or LineSegment to test intersection with</param>
    /// <param name="circle">The circle to test intersection with</param>
    public static ResultsMax2<Vector2> LinearCircleIntersectionPoints<T>(T linear, Circle2D circle) where T : ILinear2D {
        ResultsMax2<float> tVals = LinearCircleTValues(linear, circle);
        if (tVals.count == 1) return new ResultsMax2<Vector2>(linear.GetPoint(tVals.a));
        if (tVals.count == 2) return new ResultsMax2<Vector2>(linear.GetPoint(tVals.a), linear.GetPoint(tVals.b));
        return default;
    }

    #endregion

    #region Circle-Circle

    /// <summary>Returns whether two discs overlap. Unlike circles, discs overlap even if one is smaller and is completely inside the other</summary>
    /// <param name="a">The circle describing the first disc</param>
    /// <param name="b">The circle describing the second disc</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool DiscsOverlap(Circle2D a, Circle2D b) => IntersectionTest.DiscsOverlap(a.Center, a.Radius, b.Center, b.Radius);

    #endregion
}
