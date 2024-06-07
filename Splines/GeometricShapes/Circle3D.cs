using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;
using Splines.GeometricAlgebra;
using Splines.Unity;
using static Splines.Mathfs;

namespace Splines.GeometricShapes;

/// <summary>A 3D circle with a center point, radius, and a normal/axis</summary>
[Serializable]
public partial struct Circle3D
{
    /// <inheritdoc cref="Circle2D.GetOsculatingCircle(Vector2, Vector2, Vector2)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static Circle3D GetOsculatingCircle(Vector3 point, Vector3 velocity, Vector3 acceleration)
    {
        Bivector3 curvatureBivector = Vector3Extensions.GetCurvature(velocity, acceleration);
        (Vector3 axis, float curvature) = curvatureBivector.GetNormalAndArea();
        Vector3 normal = Vector3.Cross(velocity, Vector3.Cross(acceleration, velocity)).Normalized();
        float signedRadius = 1f / curvature;
        return new Circle3D(point + normal * signedRadius, axis, Abs(signedRadius));
    }

    /// <inheritdoc cref="Circle2D.FromPointTangentPoint"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static bool FromPointTangentPoint(Vector3 startPt, Vector3 startTangent, Vector3 endPt, out Circle3D circle)
    {
        Vector3 delta = endPt - startPt;
        (Vector3 xAxis, float d) = delta.GetDirectionAndMagnitude();
        if (Vector3.Dot(xAxis, startTangent).Abs() < 0.9999f)
        {
            float h = d / 2;
            float ang = AngleBetween(xAxis, startTangent);
            float fh = h * Mathf.Tan(ang + TAU / 4);
            float x2D = h;
            float y2D = fh;
            float r = Mathf.Sqrt(h * h + fh * fh);
            Vector3 normal = Vector3.Cross(xAxis, startTangent).Normalized();
            Vector3 yAxis = Vector3.Cross(normal, xAxis);
            Vector3 center = startPt + xAxis * x2D + yAxis * y2D;
            circle = new Circle3D(center, normal, r);
            return true;
        }

        circle = default;
        return false;
    }

    /// <inheritdoc cref="Circle2D.FromThreePoints"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public static bool FromThreePoints(Vector3 a, Vector3 b, Vector3 c, out Circle3D circle)
    {
        Vector3 bRel = b - a;
        (Vector3 xAxis, float bx2D) = bRel.GetDirectionAndMagnitude();
        Vector3 cRel = c - a;
        Vector3 normal = Vector3.Cross(bRel, cRel).Normalized();
        Vector3 yAxis = Vector3.Cross(normal, xAxis);

        Vector2 b2D = new Vector2(bx2D, 0);
        Vector2 c2D = new Vector2(Vector3.Dot(xAxis, cRel), Vector3.Dot(yAxis, cRel));

        if (Circle2D.FromThreePoints(default, b2D, c2D, out Circle2D circle2D))
        {
            Vector3 origin = xAxis * circle2D.Center.X + yAxis * circle2D.Center.Y;
            circle = new Circle3D(a + origin, normal, circle2D.Radius);
            return true;
        }

        circle = default;
        return false;
    }

    /// <inheritdoc cref="Circle2D.ProjectPoint"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public Vector3 ProjectPoint(Vector3 point)
    {
        Vector3 v = point - Center;
        Vector3 flattened = v - Vector3.Dot(v, Normal) * Normal;
        float mag = flattened.Magnitude();
        return Center + flattened * (Radius / mag);
    }

    /// <inheritdoc cref="Circle2D.Circumference"/>
    public float Circumference
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Circle2D.RadiusToCircumference(Radius);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Radius = Circle2D.CircumferenceToRadius(value);
    }

    /// <inheritdoc cref="Circle2D.Area"/>
    public float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Circle2D.RadiusToArea(Radius);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => Radius = Circle2D.AreaToRadius(value);
    }

    /// <inheritdoc cref="Circle2D.Center"/>
    public Vector3 Center
    {
        [Pure]
        get;
        set;
    }

    /// <inheritdoc cref="Circle2D.Radius"/>
    public float Radius
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The normal/axis of the circle</summary>
    public Vector3 Normal
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a 3D Circle</summary>
    /// <param name="center">The center of the circle</param>
    /// <param name="normal">The normal/axis of the circle</param>
    /// <param name="radius">The radius of the circle</param>
    public Circle3D(Vector3 center, Vector3 normal, float radius)
    {
        Center = center;
        Normal = normal;
        Radius = radius;
    }
}
