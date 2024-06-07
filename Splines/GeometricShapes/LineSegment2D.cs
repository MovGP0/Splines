using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>Represents a line segment, similar to a line but with a defined start and end</summary>
[Serializable]
public struct LineSegment2D : ILinear2D, IEquatable<LineSegment2D>
{
    /// <summary>The start point of the line segment</summary>
    public Vector2 Start
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The end point of the line segment</summary>
    public Vector2 End
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a line segment with a defined start and end point</summary>
    /// <param name="start">The start point of the line segment</param>
    /// <param name="end">The end point of the line segment</param>
    public LineSegment2D(Vector2 start, Vector2 end) => (Start, End) = (start, end);

    /// <summary>Returns the displacement vector from start to end of this line. Equivalent to <c>end-start</c></summary>
    public Vector2 Displacement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => End - Start;
    }

    /// <summary>Returns the normalized direction of this line. Equivalent to <c>(end-start).normalized</c></summary>
    public Vector2 Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Displacement.Normalized();
    }

    /// <summary>Calculates the length of the line segment</summary>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get
        {
            float dx = End.X - Start.X;
            float dy = End.Y - Start.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }

    /// <summary>Calculates the length squared (faster than calculating the actual length)</summary>
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get
        {
            float dx = End.X - Start.X;
            float dy = End.Y - Start.Y;
            return dx * dx + dy * dy;
        }
    }

    /// <summary>Returns the perpendicular bisector. Note: the returned normal is not normalized to save performance. Use <c>Bisector.Normalized</c> if you want to make sure it is normalized</summary>
    public Line2D Bisector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => GetBisector(Start, End);
    }

    /// <summary>Returns the perpendicular bisector of the input line segment. Note: the returned line is not normalized to save performance. Use <c>GetBisector().Normalized</c> if you want to make sure it is normalized</summary>
    /// <param name="startPoint">Starting point of the line segment</param>
    /// <param name="endPoint">Endpoint of the line segment</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Line2D GetBisector(Vector2 startPoint, Vector2 endPoint)
        => new((startPoint + endPoint) * 0.5f, (startPoint - endPoint).Rotate90CCW());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear2D.IsValidTValue(float t) => t is >= 0 and <= 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear2D.ClampTValue(float t) => t < 0 ? 0 : t > 1 ? 1 : t;

    [Pure]
    Vector2 ILinear2D.Origin => Start;

    [Pure]
    Vector2 ILinear2D.Direction => End - Start;

    /// <summary>
    /// Indicates whether the current line segment is equal to another line segment.
    /// </summary>
    /// <param name="other">A line segment to compare with this line segment.</param>
    /// <returns>true if the current line segment is equal to the other line segment; otherwise, false.</returns>
    [Pure]
    public bool Equals(LineSegment2D other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    /// Determines whether the specified object is equal to the current line segment.
    /// </summary>
    /// <param name="obj">The object to compare with the current line segment.</param>
    /// <returns>true if the specified object is equal to the current line segment; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is LineSegment2D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line segment.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    /// Determines whether two specified instances of LineSegment2D are equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(LineSegment2D left, LineSegment2D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of LineSegment2D are not equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(LineSegment2D left, LineSegment2D right) => !left.Equals(right);
}
