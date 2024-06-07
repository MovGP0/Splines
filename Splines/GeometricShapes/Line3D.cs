using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using static Splines.Mathfs;

namespace Splines.GeometricShapes;

/// <summary>A structure representing an infinitely long 3D line</summary>
[Serializable]
public struct Line3D : ILinear3D, IEquatable<Line3D>
{
    /// <summary>The origin of this line</summary>
    public Vector3 Origin
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The direction of the ray. Note: Line3D allows non-normalized direction vectors</summary>
    public Vector3 Direction
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Returns a normalized version of this line. Normalized lines ensure t-values correspond to distance</summary>
    public Line3D Normalized => new(Origin, Direction.Normalized());

    /// <summary>Creates an infinitely long 3D line, given an origin and a direction</summary>
    /// <param name="origin">The origin of the line</param>
    /// <param name="dir">The direction of the line. It does not have to be normalized, but if it is, the t-value when sampling will correspond to distance along the ray</param>
    public Line3D(Vector3 origin, Vector3 dir) => (this.Origin, this.Direction) = (origin, dir);

    /// <summary>The signed distance from this line to a point. Points to the left of this line are positive</summary>
    /// <param name="point">The point to check the signed distance to</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public float SignedDistance(Vector3 point) => Determinant(Direction.Normalized(), point - Origin);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    bool ILinear3D.IsValidTValue(float t) => true; // always valid

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    float ILinear3D.ClampTValue(float t) => t; // no clamping needed

    [Pure]
    Vector3 ILinear3D.Origin => Origin;

    [Pure]
    Vector3 ILinear3D.Direction => Direction;

    /// <summary>Projects a point onto an infinite line, returning the t-value along the line</summary>
    /// <param name="lineOrigin">Line origin</param>
    /// <param name="lineDir">Line direction (does not have to be normalized)</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float ProjectPointToLineTValue(Vector3 lineOrigin, Vector3 lineDir, Vector3 point)
    {
        return Vector3.Dot(lineDir, point - lineOrigin) / Vector3.Dot(lineDir, lineDir);
    }

    /// <summary>Gets the t-values of the closest point between two infinite lines, returning the two t-values along the line</summary>
    /// <param name="aOrigin">Line A origin</param>
    /// <param name="aDir">Line A direction (does not have to be normalized)</param>
    /// <param name="bOrigin">Line B origin</param>
    /// <param name="bDir">Line B direction (does not have to be normalized)</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static (float tA, float tB) ClosestPointBetweenLinesTValues(Vector3 aOrigin, Vector3 aDir, Vector3 bOrigin, Vector3 bDir)
    {
        Vector3 a = aOrigin;
        Vector3 b = aDir;
        Vector3 c = bOrigin;
        Vector3 d = bDir;
        Vector3 e = a - c;
        float be = Vector3.Dot(b, e);
        float de = Vector3.Dot(d, e);
        float bd = Vector3.Dot(b, d);
        float b2 = Vector3.Dot(b, b);
        float d2 = Vector3.Dot(d, d);
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
    public static Vector3 ProjectPointToLine(Vector3 lineOrigin, Vector3 lineDir, Vector3 point)
    {
        return lineOrigin + lineDir * ProjectPointToLineTValue(lineOrigin, lineDir, point);
    }

    /// <summary>Projects a point onto an infinite line</summary>
    /// <param name="line">Line to project onto</param>
    /// <param name="point">The point to project onto the line</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Vector3 ProjectPointToLine(Line3D line, Vector3 point) => ProjectPointToLine(line.Origin, line.Direction, point);

    /// <summary>Returns the signed distance to a 3D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneSignedDistance(Vector3 planeOrigin, Vector3 planeNormal, Vector3 point)
    {
        return Vector3.Dot(point - planeOrigin, planeNormal);
    }

    /// <summary>Returns the distance to a 3D plane</summary>
    /// <param name="planeOrigin">Plane origin</param>
    /// <param name="planeNormal">Plane normal (has to be normalized for a true distance)</param>
    /// <param name="point">The point to use when checking distance to the plane</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static float PointToPlaneDistance(Vector3 planeOrigin, Vector3 planeNormal, Vector3 point) => Mathfs.Abs(PointToPlaneSignedDistance(planeOrigin, planeNormal, point));

    /// <summary>
    /// Indicates whether the current line is equal to another line.
    /// </summary>
    /// <param name="other">A line to compare with this line.</param>
    /// <returns>true if the current line is equal to the other line; otherwise, false.</returns>
    [Pure]
    public bool Equals(Line3D other) => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    /// <summary>
    /// Determines whether the specified object is equal to the current line.
    /// </summary>
    /// <param name="obj">The object to compare with the current line.</param>
    /// <returns>true if the specified object is equal to the current line; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Line3D other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current line.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Origin, Direction);

    /// <summary>
    /// Determines whether two specified instances of Line3D are equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Line3D left, Line3D right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Line3D are not equal.
    /// </summary>
    /// <param name="left">The first line to compare.</param>
    /// <param name="right">The second line to compare.</param>
    /// <returns>true if the two lines are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Line3D left, Line3D right) => !left.Equals(right);
}
