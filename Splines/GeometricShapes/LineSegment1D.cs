using System.Runtime.CompilerServices;

namespace Splines.GeometricShapes;

/// <summary>Represents a line segment, similar to a line but with a defined start and end in 1D space</summary>
[Serializable]
public struct LineSegment1D : ILinear1D, IEquatable<LineSegment1D>
{
    /// <summary>The start point of the line segment</summary>
    public float Start { get; set; }

    /// <summary>The end point of the line segment</summary>
    public float End { get; set; }

    /// <summary>Creates a line segment with a defined start and end point</summary>
    /// <param name="start">The start point of the line segment</param>
    /// <param name="end">The end point of the line segment</param>
    public LineSegment1D(float start, float end) => (Start, End) = (start, end);

    /// <summary>Returns the displacement from start to end of this line. Equivalent to <c>end-start</c></summary>
    public float Displacement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => End - Start;
    }

    /// <summary>Returns the normalized direction of this line. Equivalent to <c>(end-start)/|end-start|</c></summary>
    public float Direction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Math.Sign(Displacement);
    }

    /// <summary>Calculates the length of the line segment</summary>
    public float Length
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Math.Abs(End - Start);
    }

    /// <summary>Calculates the length squared (faster than calculating the actual length)</summary>
    public float LengthSquared
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get
        {
            float displacement = End - Start;
            return displacement * displacement;
        }
    }

    float ILinear1D.Origin => Start;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear1D.IsValidTValue(float t) => t is >= 0 and <= 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear1D.ClampTValue(float t) => t < 0 ? 0 : t > 1 ? 1 : t;

    /// <summary>
    /// Indicates whether the current line segment is equal to another line segment.
    /// </summary>
    /// <param name="other">A line segment to compare with this line segment.</param>
    /// <returns>true if the current line segment is equal to the other line segment; otherwise, false.</returns>
    [Pure]
    public bool Equals(LineSegment1D other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    /// Determines whether the specified object is equal to the current line segment.
    /// </summary>
    /// <param name="obj">The object to compare with the current line segment.</param>
    /// <returns>true if the specified object is equal to the current line segment; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is LineSegment1D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line segment.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    /// Determines whether two specified instances of LineSegment1D are equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(LineSegment1D left, LineSegment1D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of LineSegment1D are not equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(LineSegment1D left, LineSegment1D right) => !left.Equals(right);
}
