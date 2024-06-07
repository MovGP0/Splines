using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.Unity;
using static Splines.Mathfs;

namespace Splines.GeometricShapes;

/// <summary>A structure representing an infinitely long 2D line</summary>
[Serializable]
public struct Line2D : ILinear2D, IEquatable<Line2D>
{
    /// <summary>The origin of this line</summary>
    public Vector2 Origin
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The direction of the ray. Note: Line2D allows non-normalized direction vectors</summary>
    public Vector2 Direction
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Returns a normalized version of this line. Normalized lines ensure t-values correspond to distance</summary>
    [Pure]
    public Line2D Normalized => new(Origin, Direction.Normalized());

    [Pure]
    public float Angle() => Mathf.Atan2(Direction.Y, Direction.X);

    /// <summary>Creates an infinitely long 2D line, given an origin and a direction</summary>
    /// <param name="origin">The origin of the line</param>
    /// <param name="dir">The direction of the line. It does not have to be normalized, but if it is, the t-value when sampling will correspond to distance along the ray</param>
    public Line2D(Vector2 origin, Vector2 dir) => (Origin, Direction) = (origin, dir);

    /// <summary>The signed distance from this line to a point. Points to the left of this line are positive</summary>
    /// <param name="point">The point to check the signed distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public float SignedDistance(Vector2 point) => Determinant(Direction.Normalized(), point - Origin);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear2D.IsValidTValue(float t) => true; // always valid

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear2D.ClampTValue(float t) => t; // no clamping needed

    [Pure]
    Vector2 ILinear2D.Origin => Origin;

    [Pure]
    Vector2 ILinear2D.Direction => Direction;

    /// <summary>Projects a point onto an infinite line, returning the t-value along the line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float ProjectPointToLineTValue(Vector2 lineOrigin, Vector2 lineDir, Vector2 point)
        => Vector2.Dot(lineDir, point - lineOrigin) / Vector2.Dot(lineDir, lineDir);

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Vector2 ProjectPointToLine(Vector2 lineOrigin, Vector2 lineDir, Vector2 point)
        => lineOrigin + lineDir * ProjectPointToLineTValue(lineOrigin, lineDir, point);

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="line">Line to project onto</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Vector2 ProjectPointToLine(Line2D line, Vector2 point)
        => ProjectPointToLine(line.Origin, line.Direction, point);

    /// <summary>Returns the signed distance to a 2D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneSignedDistance(Vector2 planeOrigin, Vector2 planeNormal, Vector2 point)
        => Vector2.Dot(point - planeOrigin, planeNormal);

    /// <summary>Returns the distance to a 2D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneDistance(Vector2 planeOrigin, Vector2 planeNormal, Vector2 point)
        => Abs(PointToPlaneSignedDistance(planeOrigin, planeNormal, point));

    /// <summary>
    /// Indicates whether the current line is equal to another line.
    /// </summary>
    /// <param name="other">A line to compare with this line.</param>
    /// <returns>true if the current line is equal to the other line; otherwise, false.</returns>
    [Pure]
    public bool Equals(Line2D other) => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    /// <summary>
    /// Determines whether the specified object is equal to the current line.
    /// </summary>
    /// <param name="obj">The object to compare with the current line.</param>
    /// <returns>true if the specified object is equal to the current line; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Line2D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, Direction);

    /// <summary>
    /// Determines whether two specified instances of Line2D are equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Line2D left, Line2D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Line2D are not equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Line2D left, Line2D right) => !left.Equals(right);
}
