using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>Represents a line segment, similar to a line but with a defined start and end in 4D space</summary>
[Serializable]
public struct LineSegment4D : ILinear4D, IEquatable<LineSegment4D>
{
    /// <summary>The start point of the line segment</summary>
    public Vector4 Start { get; set; }

    /// <summary>The end point of the line segment</summary>
    public Vector4 End { get; set; }

    /// <summary>Creates a line segment with a defined start and end point</summary>
    /// <param name="start">The start point of the line segment</param>
    /// <param name="end">The end point of the line segment</param>
    public LineSegment4D(Vector4 start, Vector4 end) => (Start, End) = (start, end);

    /// <summary>Returns the displacement vector from start to end of this line. Equivalent to <c>end-start</c></summary>
    public Vector4 Displacement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => End - Start;
    }

    /// <summary>Returns the normalized direction of this line. Equivalent to <c>(end-start).normalized</c></summary>
    public Vector4 Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Displacement.Normalized();
    }

    [Pure]
    Vector4 ILinear4D.Origin => Start;

    [Pure]
    Vector4 ILinear4D.Direction => End - Start;

    /// <summary>Calculates the length of the line segment</summary>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get
        {
            float dx = End.X - Start.X;
            float dy = End.Y - Start.Y;
            float dz = End.Z - Start.Z;
            float dw = End.W - Start.W;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz + dw * dw);
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
            float dz = End.Z - Start.Z;
            float dw = End.W - Start.W;
            return dx * dx + dy * dy + dz * dz + dw * dw;
        }
    }

    /// <summary>Returns the perpendicular bisector. Note: the returned normal is not normalized to save performance. Use <c>Bisector.Normalized</c> if you want to make sure it is normalized</summary>
    public Line4D Bisector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => GetBisector(Start, End);
    }

    /// <summary>Returns the perpendicular bisector of the input line segment.
    /// </summary>
    /// <remarks>
    /// The returned line is not normalized to save performance.
    /// Use <c>GetBisector().Normalized</c> if you want to make sure it is normalized
    /// </remarks>
    /// <param name="startPoint">Starting point of the line segment</param>
    /// <param name="endPoint">Endpoint of the line segment</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Line4D GetBisector(Vector4 startPoint, Vector4 endPoint)
        => new((startPoint + endPoint) * 0.5f, (startPoint - endPoint).Perpendicular());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public bool IsValidTValue(float t) => t is >= 0 and <= 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public float ClampTValue(float t) => t < 0 ? 0 : t > 1 ? 1 : t;

    /// <summary>
    /// Indicates whether the current line segment is equal to another line segment.
    /// </summary>
    /// <param name="other">A line segment to compare with this line segment.</param>
    /// <returns>true if the current line segment is equal to the other line segment; otherwise, false.</returns>
    [Pure]
    public bool Equals(LineSegment4D other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    /// Determines whether the specified object is equal to the current line segment.
    /// </summary>
    /// <param name="obj">The object to compare with the current line segment.</param>
    /// <returns>true if the specified object is equal to the current line segment; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is LineSegment4D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line segment.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    /// Determines whether two specified instances of LineSegment4D are equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(LineSegment4D left, LineSegment4D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of LineSegment4D are not equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(LineSegment4D left, LineSegment4D right) => !left.Equals(right);
}
