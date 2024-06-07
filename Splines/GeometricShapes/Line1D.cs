using System.Runtime.CompilerServices;

namespace Splines.GeometricShapes;

/// <summary>A structure representing an infinitely long 1D line</summary>
[Serializable]
public struct Line1D : ILinear1D, IEquatable<Line1D>
{
    /// <summary>The origin of this line</summary>
    public float Origin { get; set; }

    /// <summary>The direction of the line. Note: Line1D allows non-normalized direction values</summary>
    public float Direction { get; set; }

    /// <summary>Returns a normalized version of this line. Normalized lines ensure t-values correspond to distance</summary>
    [Pure]
    public Line1D Normalized => new(Origin, Direction / Math.Abs(Direction));

    /// <summary>Creates an infinitely long 1D line, given an origin and a direction</summary>
    /// <param name="origin">The origin of the line</param>
    /// <param name="dir">The direction of the line. It does not have to be normalized, but if it is, the t-value when sampling will correspond to distance along the line</param>
    public Line1D(float origin, float dir)
    {
        Origin = origin;
        Direction = dir;
    }

    /// <summary>The signed distance from this line to a point. Points to the left of this line are positive</summary>
    /// <param name="point">The point to check the signed distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public float SignedDistance(float point) => (point - Origin) / Math.Abs(Direction);

    /// <summary>Projects a point onto an infinite line, returning the t-value along the line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float ProjectPointToLineTValue(float lineOrigin, float lineDir, float point)
        => (point - lineOrigin) / lineDir;

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float ProjectPointToLine(float lineOrigin, float lineDir, float point)
        => lineOrigin + lineDir * ProjectPointToLineTValue(lineOrigin, lineDir, point);

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="line">Line to project onto</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float ProjectPointToLine(Line1D line, float point)
        => ProjectPointToLine(line.Origin, line.Direction, point);

    /// <summary>Returns the signed distance to a 1D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneSignedDistance(float planeOrigin, float planeNormal, float point)
        => (point - planeOrigin) * planeNormal;

    /// <summary>Returns the distance to a 1D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneDistance(float planeOrigin, float planeNormal, float point)
        => Math.Abs(PointToPlaneSignedDistance(planeOrigin, planeNormal, point));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear1D.IsValidTValue(float t) => true; // always valid

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear1D.ClampTValue(float t) => t; // no clamping needed

    /// <summary>
    /// Indicates whether the current line is equal to another line.
    /// </summary>
    /// <param name="other">A line to compare with this line.</param>
    /// <returns>true if the current line is equal to the other line; otherwise, false.</returns>
    [Pure]
    public bool Equals(Line1D other) => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    /// <summary>
    /// Determines whether the specified object is equal to the current line.
    /// </summary>
    /// <param name="obj">The object to compare with the current line.</param>
    /// <returns>true if the specified object is equal to the current line; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Line1D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, Direction);

    /// <summary>
    /// Determines whether two specified instances of Line1D are equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Line1D left, Line1D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Line1D are not equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Line1D left, Line1D right) => !left.Equals(right);
}
