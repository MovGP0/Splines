using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.UtilityTypes;

namespace Splines.GeometricShapes;

/// <summary>A 2D rectangular box</summary>
[Serializable]
public partial struct Box2D
{
    /// <summary>The number of vertices in a rectangle</summary>
    public const int VertexCount = 4;

    /// <summary>The center of this rectangle</summary>
    public Vector2 Center
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The extents of this rectangle (distance from the center to the edge) per axis</summary>
    public Vector2 Extents
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Projects a point onto the boundary of this box. Points inside will be pushed out to the boundary</summary>
    /// <param name="point">The point to project onto the box boundary</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public Vector2 ClosestPointOnBoundary(Vector2 point)
    {
        float px = point.X - Center.X;
        float py = point.Y - Center.Y;
        float ax = Mathfs.Abs(px);
        float ay = Mathfs.Abs(py);
        float dx = ax - Extents.X;
        float dy = ay - Extents.Y;
        bool caseX = dy <= dx;
        bool caseY = caseX == false;
        return new Vector2(
            Center.X + Mathfs.Sign(px) * (caseX ? Extents.X : ax.AtMost(Extents.X)),
            Center.Y + Mathfs.Sign(py) * (caseY ? Extents.Y : ay.AtMost(Extents.Y))
        );
    }

    /// <summary>Returns whether an infinite line intersects this box</summary>
    /// <param name="line">The line to see if it intersects</param>
    [Pure]
    public bool Intersects(Line2D line) => IntersectionTest.LineRectOverlap(Extents, line.Origin - Center, line.Direction);

    /// <summary>Returns the intersection points of this rectangle and a line</summary>
    /// <param name="line">The line to get intersection points of</param>
    [Pure]
    public ResultsMax2<Vector2> Intersect(Line2D line) => IntersectionTest.LinearRectPoints(Center, Extents, line.Origin, line.Direction);

    /// <summary>Returns the point inside the box, closest to another point.
    /// Points already inside will return the same location</summary>
    /// <param name="point">The point to get the closest point to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public Vector2 ClosestPointInside(Vector2 point) =>
        new(
            point.X.Clamp(Center.X - Extents.X, Center.X + Extents.X),
            point.Y.Clamp(Center.Y - Extents.Y, Center.Y + Extents.Y)
        );

    /// <summary>Returns the corner of this box closest to the given point</summary>
    /// <param name="point">The point to get the closest corner to</param>
    [Pure]
    public Vector2 ClosestCorner(Vector2 point) =>
        new(
            Center.X + Mathfs.Sign(point.X - Center.X) * Extents.X,
            Center.Y + Mathfs.Sign(point.Y - Center.Y) * Extents.Y
        );

    /// <summary>Extends the boundary of this box to encapsulate a point</summary>
    /// <param name="point">The point to encapsulate</param>
    public void Encapsulate(Vector2 point)
    {
        float minX = Mathfs.Min(Center.X - Extents.X, point.X);
        float minY = Mathfs.Min(Center.Y - Extents.Y, point.Y);
        float maxX = Mathfs.Max(Center.X + Extents.X, point.X);
        float maxY = Mathfs.Max(Center.Y + Extents.Y, point.Y);
        Center = new Vector2((maxX + minX) / 2, (maxY + minY) / 2);
        Extents = new Vector2((maxX - minX) / 2, (maxY - minY) / 2);
    }

    /// <summary>Returns whether a point is inside this box</summary>
    /// <param name="point">The point to test if it's inside</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public bool Contains(Vector2 point) => Mathfs.Abs(point.X - Center.X) - Extents.X <= 0 &&
                                           Mathfs.Abs(point.Y - Center.Y) - Extents.Y <= 0;

    /// <summary>The total perimeter length of this rectangle</summary>
    public float Perimeter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => 4 * (Extents.X + Extents.Y);
    }

    /// <summary>The area of this rectangle</summary>
    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.X * Extents.Y * 4;
    }

    /// <summary>Returns a vertex of this box by index</summary>
    /// <param name="index">The index of the vertex to retrieve</param>
    [Pure]
    public Vector2 GetVertex(int index)
    {
        switch (index)
        {
            case 0: return new Vector2(Center.X - Extents.X, Center.Y - Extents.Y);
            case 1: return new Vector2(Center.X - Extents.X, Center.Y + Extents.Y);
            case 2: return new Vector2(Center.X + Extents.X, Center.Y - Extents.Y);
            case 3: return new Vector2(Center.X + Extents.X, Center.Y + Extents.Y);
            default:
                throw new ArgumentOutOfRangeException(nameof(index),
                    $"Invalid index: {index}. Valid vertex indices range from 0 to {VertexCount - 1}");
        }
    }

    /// <summary>The minimum coordinates inside the box, per axis</summary>
    public Vector2 Min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => new(Center.X - Extents.X, Center.Y - Extents.Y);
    }

    /// <summary>The maximum coordinates inside the box, per axis</summary>
    public Vector2 Max
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => new(Center.X + Extents.X, Center.Y + Extents.Y);
    }

    /// <summary>The size of this box</summary>
    public Vector2 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents * 2;
    }

    /// <summary>The width of this rectangle</summary>
    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.X * 2;
    }

    /// <summary>The height of this rectangle</summary>
    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.Y * 2;
    }
}
