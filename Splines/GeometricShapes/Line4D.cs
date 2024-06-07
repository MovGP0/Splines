using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>A structure representing an infinitely long 4D line</summary>
[Serializable]
public struct Line4D : ILinear4D, IEquatable<Line4D>
{
    /// <summary>The origin of this line</summary>
    public Vector4 Origin { get; set; }

    /// <summary>The direction of the line. Note: Line4D allows non-normalized direction vectors</summary>
    public Vector4 Direction { get; set; }

    /// <summary>Returns a normalized version of this line. Normalized lines ensure t-values correspond to distance</summary>
    [Pure]
    public Line4D Normalized => new Line4D(Origin, Direction.Normalized());

    /// <summary>Creates an infinitely long 4D line, given an origin and a direction</summary>
    /// <param name="origin">The origin of the line</param>
    /// <param name="dir">The direction of the line. It does not have to be normalized, but if it is, the t-value when sampling will correspond to distance along the ray</param>
    public Line4D(Vector4 origin, Vector4 dir)
    {
        Origin = origin;
        Direction = dir;
    }

    /// <summary>The signed distance from this line to a point. Points to the left of this line are positive</summary>
    /// <param name="point">The point to check the signed distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public float SignedDistance(Vector4 point) => Vector4.Dot(Direction.Normalized(), point - Origin);

    /// <summary>Projects a point onto an infinite line, returning the t-value along the line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float ProjectPointToLineTValue(Vector4 lineOrigin, Vector4 lineDir, Vector4 point)
    {
        return Vector4.Dot(lineDir, point - lineOrigin) / Vector4.Dot(lineDir, lineDir);
    }

    /// <summary>Gets the t-values of the closest point between two infinite lines, returning the two t-values along the line</summary>
    /// <param name="aOrigin">Line A origin</param>
    /// <param name="aDir">Line A direction (does not have to be normalized)</param>
    /// <param name="bOrigin">Line B origin</param>
    /// <param name="bDir">Line B direction (does not have to be normalized)</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static (float tA, float tB) ClosestPointBetweenLinesTValues(Vector4 aOrigin, Vector4 aDir, Vector4 bOrigin, Vector4 bDir)
    {
        Vector4 a = aOrigin;
        Vector4 b = aDir;
        Vector4 c = bOrigin;
        Vector4 d = bDir;
        Vector4 e = a - c;
        float be = Vector4.Dot(b, e);
        float de = Vector4.Dot(d, e);
        float bd = Vector4.Dot(b, d);
        float b2 = Vector4.Dot(b, b);
        float d2 = Vector4.Dot(d, d);
        float A = -b2 * d2 + bd * bd;

        float s = (-b2 * de + be * bd) / A;
        float t = (d2 * be - de * bd) / A;

        return (t, s);
    }

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Vector4 ProjectPointToLine(Vector4 lineOrigin, Vector4 lineDir, Vector4 point)
    {
        return lineOrigin + lineDir * ProjectPointToLineTValue(lineOrigin, lineDir, point);
    }

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="line">Line to project onto</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Vector4 ProjectPointToLine(Line4D line, Vector4 point) => ProjectPointToLine(line.Origin, line.Direction, point);

    /// <summary>Returns the signed distance to a 4D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneSignedDistance(Vector4 planeOrigin, Vector4 planeNormal, Vector4 point)
    {
        return Vector4.Dot(point - planeOrigin, planeNormal);
    }

    /// <summary>Returns the distance to a 4D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneDistance(Vector4 planeOrigin, Vector4 planeNormal, Vector4 point) => Math.Abs(PointToPlaneSignedDistance(planeOrigin, planeNormal, point));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear4D.IsValidTValue(float t) => true; // always valid

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear4D.ClampTValue(float t) => t; // no clamping needed

    /// <summary>
    /// Indicates whether the current line is equal to another line.
    /// </summary>
    /// <param name="other">A line to compare with this line.</param>
    /// <returns>true if the current line is equal to the other line; otherwise, false.</returns>
    [Pure]
    public bool Equals(Line4D other) => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    /// <summary>
    /// Determines whether the specified object is equal to the current line.
    /// </summary>
    /// <param name="obj">The object to compare with the current line.</param>
    /// <returns>true if the specified object is equal to the current line; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Line4D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, Direction);

    /// <summary>
    /// Determines whether two specified instances of Line4D are equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Line4D left, Line4D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Line4D are not equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Line4D left, Line4D right) => !left.Equals(right);
}
