using System.Numerics;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>An oriented 2D plane embedded in 3D space</summary>
[Serializable]
public partial struct Plane2DIn3D
{
    public static readonly Plane2DIn3D XY = new(default, Vector3Helper.Right, Vector3Helper.Up);
    public static readonly Plane2DIn3D YZ = new(default, Vector3Helper.Up, Vector3Helper.Forward);
    public static readonly Plane2DIn3D ZX = new(default, Vector3Helper.Forward, Vector3Helper.Right);

    public Vector3 Origin
    {
        [Pure]
        get;
        set;
    }

    public Vector3 AxisX
    {
        [Pure]
        get;
        set;
    }

    public Vector3 AxisY
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates an oriented 2D plane embedded in 3D space</summary>
    /// <param name="origin">The origin of the plane</param>
    /// <param name="axisX">The x-axis direction of the plane</param>
    /// <param name="axisY">The y-axis direction of the plane</param>
    public Plane2DIn3D(Vector3 origin, Vector3 axisX, Vector3 axisY)
        => (this.Origin, this.AxisX, this.AxisY) = (origin, axisX.Normalized(), axisY.Normalized());

    /// <summary>Rotates this plane around the Y axis, setting the X axis,
    /// so that the given point <c>p</c> is in the plane where x > 0</summary>
    /// <param name="p">The point to include in the plane</param>
    /// <param name="pLocal">The included point in the 2D local space</param>
    public void RotateAroundYToInclude(Vector3 p, out Vector2 pLocal)
    {
        Vector3 pRel = p - Origin;
        float yProj = Vector3.Dot(AxisY, pRel);
        AxisX = (pRel - AxisY * yProj).Normalized();
        float xProj = Vector3.Dot(AxisX, pRel);
        pLocal = new Vector2(xProj, yProj);
    }

    /// <summary>Transforms a local 2D point to a 3D world space point</summary>
    /// <param name="pt">The local space point to transform</param>
    [Pure]
    public Vector3 TransformPoint(Vector2 pt)
    {
        return new( // unrolled for performance
            Origin.X + AxisX.X * pt.X + AxisY.X * pt.Y,
            Origin.Y + AxisX.Y * pt.X + AxisY.Y * pt.Y,
            Origin.Z + AxisX.Z * pt.X + AxisY.Z * pt.Y
        );
    }

    /// <summary>Transforms a local 2D vector to a 3D world space vector, not taking position into account</summary>
    /// <param name="vec">The local space vector to transform</param>
    [Pure]
    public Vector3 TransformVector(Vector2 vec)
    {
        return new( // unrolled for performance
            AxisX.X * vec.X + AxisY.X * vec.Y,
            AxisX.Y * vec.X + AxisY.Y * vec.Y,
            AxisX.Z * vec.X + AxisY.Z * vec.Y
        );
    }

    /// <summary>Transforms a 3D world space point to a local 2D point</summary>
    /// <param name="pt">World space point</param>
    [Pure]
    public Vector2 InverseTransformPoint(Vector3 pt)
    {
        float rx = pt.X - Origin.X;
        float ry = pt.Y - Origin.Y;
        float rz = pt.Z - Origin.Z;
        return new(
            AxisX.X * rx + AxisX.Y * ry + AxisX.Z * rz,
            AxisY.X * rx + AxisY.Y * ry + AxisY.Z * rz
        );
    }

    /// <summary>Transforms a 3D world space vector to a local 2D vector</summary>
    /// <param name="vec">World space vector</param>
    [Pure]
    public Vector2 InverseTransformVector(Vector3 vec)
    {
        return new(
            AxisX.X * vec.X + AxisX.Y * vec.Y + AxisX.Z * vec.Z,
            AxisY.X * vec.X + AxisY.Y * vec.Y + AxisY.Z * vec.Z
        );
    }
}
