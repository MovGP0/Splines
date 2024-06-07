using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>Represents a line segment, similar to a line but with a defined start and end</summary>
[Serializable]
public struct LineSegment3D : ILinear3D, IEquatable<LineSegment3D>
{
    /// <summary>The start point of the line segment</summary>
    public Vector3 Start
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The end point of the line segment</summary>
    public Vector3 End
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a line segment with a defined start and end point</summary>
    /// <param name="start">The start point of the line segment</param>
    /// <param name="end">The end point of the line segment</param>
    public LineSegment3D(Vector3 start, Vector3 end) => (Start, End) = (start, end);

    /// <summary>Returns the displacement vector from start to end of this line. Equivalent to <c>end-start</c></summary>
    public Vector3 Displacement
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => End - Start;
    }

    /// <summary>Returns the normalized direction of this line. Equivalent to <c>(end-start).normalized</c></summary>
    public Vector3 Direction
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
            float dz = End.Z - Start.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
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
            return dx * dx + dy * dy + dz * dz;
        }
    }

    /// <summary>Returns the perpendicular bisector. Note: the returned normal is not normalized to save performance. Use <c>Bisector.Normalized</c> if you want to make sure it is normalized</summary>
    public Plane3D Bisector
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => GetBisector(Start, End);
    }

    /// <summary>Returns the perpendicular bisector of the input line segment</summary>
    /// <param name="startPoint">Starting point of the line segment</param>
    /// <param name="endPoint">Endpoint of the line segment</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Plane3D GetBisector(Vector3 startPoint, Vector3 endPoint)
        => new((endPoint - startPoint).Normalized(), (endPoint + startPoint) / 2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear3D.IsValidTValue(float t) => t is >= 0 and <= 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear3D.ClampTValue(float t) => t < 0 ? 0 : t > 1 ? 1 : t;

    [Pure]
    Vector3 ILinear3D.Origin => Start;

    [Pure]
    Vector3 ILinear3D.Direction => End - Start;

    /// <summary>
    /// Indicates whether the current line segment is equal to another line segment.
    /// </summary>
    /// <param name="other">A line segment to compare with this line segment.</param>
    /// <returns>true if the current line segment is equal to the other line segment; otherwise, false.</returns>
    [Pure]
    public bool Equals(LineSegment3D other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    /// Determines whether the specified object is equal to the current line segment.
    /// </summary>
    /// <param name="obj">The object to compare with the current line segment.</param>
    /// <returns>true if the specified object is equal to the current line segment; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is LineSegment3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line segment.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    /// Determines whether two specified instances of LineSegment3D are equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(LineSegment3D left, LineSegment3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of LineSegment3D are not equal.
    /// </summary>
    /// <param name="left">The first line segment to compare.</param>
    /// <param name="right">The second line segment to compare.</param>
    /// <returns>true if the two line segments are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(LineSegment3D left, LineSegment3D right) => !left.Equals(right);
}
