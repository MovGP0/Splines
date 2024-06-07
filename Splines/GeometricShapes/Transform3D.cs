using System.Diagnostics;
using System.Numerics;

namespace Splines.GeometricShapes;

/// <summary>An orthonormal affine 3D transformation</summary>
[Serializable]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public partial struct Transform3D
{
    private string DebuggerDisplay
    {
        get
        {
            return $"Origin: ({origin_x:0.###}, {origin_y:0.###}, {origin_z:0.###}), " +
                   $"X-Axis: ({axisX_x:0.###}, {axisX_y:0.###}, {axisX_z:0.###}), " +
                   $"Y-Axis: ({axisY_x:0.###}, {axisY_y:0.###}, {axisY_z:0.###}), " +
                   $"Z-Axis: ({AxisZ_x:0.###}, {AxisZ_y:0.###}, {AxisZ_z:0.###})";
        }
    }

    public float origin_x, origin_y, origin_z;
    public float axisX_x, axisX_y, axisX_z;
    public float axisY_x, axisY_y, axisY_z;

    public Vector3 Origin
    {
        get => new(origin_x, origin_y, origin_z);
        set => (origin_x, origin_y, origin_z) = (value.X, value.Y, value.Z);
    }

    public Vector3 AxisX
    {
        get => new(axisX_x, axisX_y, axisX_z);
        set => (axisX_x, axisX_y, axisX_z) = (value.X, value.Y, value.Z);
    }

    public Vector3 AxisY
    {
        get => new(axisY_x, axisY_y, axisY_z);
        set => (axisY_x, axisY_y, axisY_z) = (value.X, value.Y, value.Z);
    }

    [Pure]
    public Vector3 AxisZ => Vector3.Cross(AxisX, AxisY);

    [Pure]
    public float AxisZ_x => AxisZ.X;

    [Pure]
    public float AxisZ_y => AxisZ.Y;

    [Pure]
    public float AxisZ_z => AxisZ.Z;

    /// <summary>Creates an orthonormal affine 3D transformation</summary>
    public Transform3D(Vector3 origin, Vector3 axisX, Vector3 axisY)
    {
        origin_x = origin.X;
        origin_y = origin.Y;
        origin_z = origin.Z;
        axisX_x = axisX.X;
        axisX_y = axisX.Y;
        axisX_z = axisX.Z;
        axisY_x = axisY.X;
        axisY_y = axisY.Y;
        axisY_z = axisY.Z;
    }

    /// <summary>Transforms a local point to a world space point</summary>
    /// <param name="pt">The local space point to transform</param>
    [Pure]
    public Vector3 TransformPoint(Vector3 pt)
    {
        return new(
            origin_x + axisX_x * pt.X + axisY_x * pt.Y + AxisZ_x * pt.Z,
            origin_y + axisX_y * pt.X + axisY_y * pt.Y + AxisZ_y * pt.Z,
            origin_z + axisX_z * pt.X + axisY_z * pt.Y + AxisZ_z * pt.Z
       );
    }

    /// <inheritdoc cref="TransformPoint(Vector3)"/>
    [Pure]
    public Vector3 TransformPoint(float x, float y, float z)
    {
        return new(
            origin_x + axisX_x * x + axisY_x * y + AxisZ_x * z,
            origin_y + axisX_y * x + axisY_y * y + AxisZ_y * z,
            origin_z + axisX_z * x + axisY_z * y + AxisZ_z * z
       );
    }

    /// <summary>Transforms a local vector to a world space vector, not taking position into account</summary>
    /// <param name="vec">The local space vector to transform</param>
    public Vector3 TransformVector(Vector3 vec)
    {
        return new(
            axisX_x * vec.X + axisY_x * vec.Y + AxisZ_x * vec.Z,
            axisX_y * vec.X + axisY_y * vec.Y + AxisZ_y * vec.Z,
            axisX_z * vec.X + axisY_z * vec.Y + AxisZ_z * vec.Z
       );
    }

    /// <inheritdoc cref="TransformVector(Vector3)"/>
    public Vector3 TransformVector(float x, float y, float z)
    {
        return new(
            axisX_x * x + axisY_x * y + AxisZ_x * z,
            axisX_y * x + axisY_y * y + AxisZ_y * z,
            axisX_z * x + axisY_z * y + AxisZ_z * z
       );
    }

    /// <summary>Transform a world space point to a local point</summary>
    /// <param name="pt">World space point</param>
    [Pure]
    public Vector3 InverseTransformPoint(Vector3 pt)
    {
        float rx = pt.X - origin_x;
        float ry = pt.Y - origin_y;
        float rz = pt.Z - origin_z;
        return new(
            axisX_x * rx + axisX_y * ry + axisX_z * rz,
            axisY_x * rx + axisY_y * ry + axisY_z * rz,
            AxisZ_x * rx + AxisZ_y * ry + AxisZ_z * rz
       );
    }

    /// <summary>Transform a world space vector to a local vector</summary>
    /// <param name="vec">World space vector</param>
    [Pure]
    public Vector3 InverseTransformVector(Vector3 vec)
    {
        return new(
            axisX_x * vec.X + axisX_y * vec.Y + axisX_z * vec.Z,
            axisY_x * vec.X + axisY_y * vec.Y + axisY_z * vec.Z,
            AxisZ_x * vec.X + AxisZ_y * vec.Y + AxisZ_z * vec.Z
       );
    }

    [Pure]
    public static Transform3D operator +(Transform3D a, Vector3 translation)
    {
        return new Transform3D(
            new Vector3(a.origin_x + translation.X, a.origin_y + translation.Y, a.origin_z + translation.Z),
            new Vector3(a.axisX_x, a.axisX_y, a.axisX_z),
            new Vector3(a.axisY_x, a.axisY_y, a.axisY_z)
        );
    }

    [Pure]
    public static Transform3D operator -(Transform3D a, Vector3 translation)
    {
        return new Transform3D(
            new Vector3(a.origin_x - translation.X, a.origin_y - translation.Y, a.origin_z - translation.Z),
            new Vector3(a.axisX_x, a.axisX_y, a.axisX_z),
            new Vector3(a.axisY_x, a.axisY_y, a.axisY_z)
        );
    }

    [Pure]
    public static Transform3D operator *(Transform3D a, Transform3D b)
    {
        return new Transform3D(
            a.TransformPoint(b.origin_x, b.origin_y, b.origin_z),
            a.TransformVector(b.axisX_x, b.axisX_y, b.axisX_z),
            a.TransformVector(b.axisY_x, b.axisY_y, b.axisY_z)
        );
    }
}
