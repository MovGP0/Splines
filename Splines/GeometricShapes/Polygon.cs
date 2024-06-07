using System.Collections.Generic;
using System.Numerics;
using Splines.Extensions;
using Splines.Unity;
using static Splines.Mathfs;

namespace Splines.GeometricShapes;

/// <summary>Polygon with various math functions to test if a point is inside, calculate area, etc.</summary>
public sealed class Polygon : IEquatable<Polygon>
{
    /// <summary>The points in this polygon</summary>
    public IReadOnlyList<Vector2> Points { get; }

    /// <summary>Creates a new 2D polygon</summary>
    /// <param name="points">The points in the polygon</param>
    public Polygon(IReadOnlyList<Vector2> points) => Points = points;

    /// <summary>Get a point by index. Indices cannot be out of range, as they will wrap/cycle in the polygon</summary>
    /// <param name="i">The index of the point</param>
    [Pure]
    public Vector2 this[int i] => Points[i.Mod(Count)];

    /// <summary>The number of points in this polygon</summary>
    [Pure]
    public int Count => Points.Count;

    /// <summary>Returns whether this polygon is defined clockwise</summary>
    [Pure]
    public bool IsClockwise => SignedArea > 0;

    /// <summary>Returns the area of this polygon</summary>
    [Pure]
    public float Area => Mathf.Abs(SignedArea);

    /// <summary>Returns the signed area of this polygon</summary>
    [Pure]
    public float SignedArea
    {
        get
        {
            int count = Points.Count;
            float sum = 0f;
            for (int i = 0; i < count; i++)
            {
                Vector2 a = Points[i];
                Vector2 b = Points[(i + 1) % count];
                sum += (b.X - a.X) * (b.Y + a.Y);
            }

            return sum * 0.5f;
        }
    }

    /// <summary>Returns the length of the perimeter of the polygon</summary>
    [Pure]
    public float Perimeter
    {
        get
        {
            int count = Points.Count;
            float totalDist = 0f;
            for (int i = 0; i < count; i++)
            {
                Vector2 a = Points[i];
                Vector2 b = Points[(i + 1) % count];
                float dx = a.X - b.X;
                float dy = a.Y - b.Y;
                totalDist += Mathf.Sqrt(dx * dx + dy * dy); // unrolled for speed
            }

            return totalDist;
        }
    }

    /// <summary>Returns the axis-aligned bounding box of this polygon</summary>
    [Pure]
    public Rect Bounds
    {
        get
        {
            int count = Points.Count;
            Vector2 p = Points[0];
            float xMin = p.X, xMax = p.X, yMin = p.Y, yMax = p.Y;
            for (int i = 1; i < count; i++)
            {
                p = Points[i];
                xMin = Mathf.Min(xMin, p.X);
                xMax = Mathf.Max(xMax, p.X);
                yMin = Mathf.Min(yMin, p.Y);
                yMax = Mathf.Max(yMax, p.Y);
            }

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }
    }

    /// <summary>Returns whether a point is inside the polygon</summary>
    /// <param name="point">The point to test and see if it's inside</param>
    [Pure]
    public bool Contains(Vector2 point) => WindingNumber(point) != 0;

    /// <summary>Returns the winding number for this polygon, around a given point</summary>
    /// <param name="point">The point to check winding around</param>
    [Pure]
    public int WindingNumber(Vector2 point)
    {
        int winding = 0;

        int count = Points.Count;
        for (int i = 0; i < count; i++)
        {
            int iNext = (i + 1) % count;
            if (Points[i].Y <= point.Y)
            {
                if (Points[iNext].Y > point.Y && IsLeft(Points[i], Points[iNext], point) > 0)
                    winding--;
            }
            else
            {
                if (Points[iNext].Y <= point.Y && IsLeft(Points[i], Points[iNext], point) < 0)
                    winding++;
            }
        }

        return winding;

        [Pure]
        float IsLeft(Vector2 a, Vector2 b, Vector2 p) => SignWithZero(Determinant(a.To(p), a.To(b)));
    }

    /// <summary>Returns the resulting polygons when clipping this polygon by a line</summary>
    /// <param name="line">The line/plane to clip by. Points on its left side will be kept</param>
    /// <param name="clippedPolygons">The resulting array of clipped polygons (if any)</param>
    public ResultState Clip(Line2D line, out List<Polygon> clippedPolygons) => PolygonClipper.Clip(this, line, out clippedPolygons);

    /// <summary>Returns a new polygon created by offsetting the edges of this polygon by a given distance</summary>
    /// <param name="offset">The offset distance for the edges</param>
    [Pure]
    public Polygon GetMiterPolygon(float offset)
    {
        List<Vector2> miterPts = new List<Vector2>();

        for (int i = 0; i < Count; i++)
        {
            Line2D line = GetMiterLine(i);
            Line2D line2 = GetMiterLine(i + 1);
            if (line.Intersect(line2, out Vector2 pt))
            {
                miterPts.Add(pt);
            }
            else
            {
                Console.Error.WriteLine($"{line.Origin},{line.Direction}\n{line2.Origin},{line2.Direction}\n" +
                                        $"Points:{string.Join("\n", Points)}");
                throw new Exception("Line intersection failed");
            }
        }

        return new Polygon(miterPts);

        [Pure]
        Line2D GetMiterLine(int i)
        {
            Vector2 tangent = (this[i + 1] - this[i]).Normalized();
            Vector2 normal = tangent.Rotate90CCW();
            return new Line2D(this[i] + normal * offset, tangent);
        }
    }

    /// <summary>Checks if this polygon is equal to another polygon</summary>
    /// <param name="other">The polygon to compare with</param>
    /// <returns>True if the polygons are equal, otherwise false</returns>
    [Pure]
    public bool Equals(Polygon? other)
    {
        if (other is null)
        {
            return false;
        }

        if (Count != other.Count)
        {
            return false;
        }

        for (int i = 0; i < Count; i++)
        {
            if (this[i] != other[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>Checks if this polygon is equal to another object</summary>
    /// <param name="obj">The object to compare with</param>
    /// <returns>True if the object is a polygon and the polygons are equal, otherwise false</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Polygon polygon && Equals(polygon);

    /// <summary>Returns the hash code for this polygon</summary>
    /// <returns>The hash code for this polygon</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Points);
}
