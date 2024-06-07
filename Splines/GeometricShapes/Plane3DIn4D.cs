using System.Numerics;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>A 3D plane embedded in 4D space</summary>
[Serializable]
public partial struct Plane3DIn4D
{
    public Vector4 Origin
    {
        [Pure]
        get;
        set;
    }

    public Vector4 AxisX
    {
        [Pure]
        get;
        set;
    }

    public Vector4 AxisY
    {
        [Pure]
        get;
        set;
    }

    public Vector4 AxisZ
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a 3D plane embedded in 4D space</summary>
    /// <param name="origin">The origin of the plane</param>
    /// <param name="axisX">The x-axis direction of the plane</param>
    /// <param name="axisY">The y-axis direction of the plane</param>
    /// <param name="axisZ">The z-axis direction of the plane</param>
    public Plane3DIn4D(Vector4 origin, Vector4 axisX, Vector4 axisY, Vector4 axisZ)
        => (Origin, AxisX, AxisY, AxisZ) = (origin, axisX.Normalized(), axisY.Normalized(), axisZ.Normalized());

    /// <summary>Rotates this plane around the Y axis, setting the X axis,
    /// so that the given point <c>p</c> is in the plane where x > 0</summary>
    /// <param name="p">The point to include in the plane</param>
    /// <param name="pLocal">The included point in the 3D local space</param>
    public void RotateAroundYToInclude(Vector4 p, out Vector3 pLocal)
    {
        Vector4 pRel = p - Origin;
        float yProj = Vector4.Dot(AxisY, pRel);
        AxisX = (pRel - AxisY * yProj).Normalized();
        float xProj = Vector4.Dot(AxisX, pRel);
        float zProj = Vector4.Dot(AxisZ, pRel);
        pLocal = new Vector3(xProj, yProj, zProj);
    }

    /// <summary>Transforms a local 3D point to a 4D world space point</summary>
    /// <param name="pt">The local space point to transform</param>
    [Pure]
    public Vector4 TransformPoint(Vector3 pt)
    {
        return new Vector4( // unrolled for performance
            Origin.X + AxisX.X * pt.X + AxisY.X * pt.Y + AxisZ.X * pt.Z,
            Origin.Y + AxisX.Y * pt.X + AxisY.Y * pt.Y + AxisZ.Y * pt.Z,
            Origin.Z + AxisX.Z * pt.X + AxisY.Z * pt.Y + AxisZ.Z * pt.Z,
            Origin.W + AxisX.W * pt.X + AxisY.W * pt.Y + AxisZ.W * pt.Z
        );
    }

    /// <summary>Transforms a local 3D vector to a 4D world space vector, not taking position into account</summary>
    /// <param name="vec">The local space vector to transform</param>
    [Pure]
    public Vector4 TransformVector(Vector3 vec)
    {
        return new Vector4( // unrolled for performance
            AxisX.X * vec.X + AxisY.X * vec.Y + AxisZ.X * vec.Z,
            AxisX.Y * vec.X + AxisY.Y * vec.Y + AxisZ.Y * vec.Z,
            AxisX.Z * vec.X + AxisY.Z * vec.Y + AxisZ.Z * vec.Z,
            AxisX.W * vec.X + AxisY.W * vec.Y + AxisZ.W * vec.Z
        );
    }

    /// <summary>Transforms a 4D world space point to a local 3D point</summary>
    /// <param name="pt">World space point</param>
    [Pure]
    public Vector3 InverseTransformPoint(Vector4 pt)
    {
        float rx = pt.X - Origin.X;
        float ry = pt.Y - Origin.Y;
        float rz = pt.Z - Origin.Z;
        float rw = pt.W - Origin.W;
        return new Vector3(
            AxisX.X * rx + AxisX.Y * ry + AxisX.Z * rz + AxisX.W * rw,
            AxisY.X * rx + AxisY.Y * ry + AxisY.Z * rz + AxisY.W * rw,
            AxisZ.X * rx + AxisZ.Y * ry + AxisZ.Z * rz + AxisZ.W * rw
        );
    }

    /// <summary>Transforms a 4D world space vector to a local 3D vector</summary>
    /// <param name="vec">World space vector</param>
    [Pure]
    public Vector3 InverseTransformVector(Vector4 vec)
    {
        return new Vector3(
            AxisX.X * vec.X + AxisX.Y * vec.Y + AxisX.Z * vec.Z + AxisX.W * vec.W,
            AxisY.X * vec.X + AxisY.Y * vec.Y + AxisY.Z * vec.Z + AxisY.W * vec.W,
            AxisZ.X * vec.X + AxisZ.Y * vec.Y + AxisZ.Z * vec.Z + AxisZ.W * vec.W
        );
    }
}
