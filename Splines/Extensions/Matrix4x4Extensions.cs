using System.Numerics;
using Splines.Numerics;

namespace Splines.Extensions;

public static class Matrix4x4Extensions
{
    // Get a column of the matrix.
    public static Vector4 GetColumn(this Matrix4x4 m, int index)
    {
        switch (index)
        {
            case 0: return new Vector4(m.M11, m.M21, m.M31, m.M41);
            case 1: return new Vector4(m.M12, m.M22, m.M32, m.M42);
            case 2: return new Vector4(m.M13, m.M23, m.M33, m.M43);
            case 3: return new Vector4(m.M14, m.M24, m.M34, m.M44);
            default:
                throw new IndexOutOfRangeException("Invalid column index!");
        }
    }

    public static float AverageScale(this Matrix4x4 m)
    {
        return (
            (m.GetColumn(0).ToVector3()).Magnitude() +
            (m.GetColumn(1).ToVector3()).Magnitude() +
            (m.GetColumn(2).ToVector3()).Magnitude()
       ) / 3;
    }

    public static Matrix4x1 MultiplyColumnVector(this Matrix4x4 m, Matrix4x1 v) =>
        new Matrix4x1(
            m.M11 * v.M0 + m.M12 * v.M1 + m.M13 * v.M2 + m.M14 * v.M3,
            m.M21 * v.M0 + m.M22 * v.M1 + m.M23 * v.M2 + m.M24 * v.M3,
            m.M31 * v.M0 + m.M32 * v.M1 + m.M33 * v.M2 + m.M34 * v.M3,
            m.M41 * v.M0 + m.M42 * v.M1 + m.M43 * v.M2 + m.M44 * v.M3
       );

    public static Vector2Matrix4x1 MultiplyColumnVector(this Matrix4x4 m, Vector2Matrix4x1 v) => new(m.MultiplyColumnVector(v.X), m.MultiplyColumnVector(v.Y));
    public static Vector3Matrix4x1 MultiplyColumnVector(this Matrix4x4 m, Vector3Matrix4x1 v) => new(m.MultiplyColumnVector(v.X), m.MultiplyColumnVector(v.Y), m.MultiplyColumnVector(v.Z));
    public static Vector4Matrix4x1 MultiplyColumnVector(this Matrix4x4 m, Vector4Matrix4x1 v) => new(m.MultiplyColumnVector(v.X), m.MultiplyColumnVector(v.Y), m.MultiplyColumnVector(v.Z), m.MultiplyColumnVector(v.W));
/*
    /// <summary>Transforms a ray by this matrix</summary>
    /// <param name="mtx">The matrix to use</param>
    /// <param name="ray">The ray to transform</param>
    public static Ray MultiplyRay(this Matrix4x4 mtx, Ray ray) => new(mtx.MultiplyPoint3x4(ray.origin), mtx.MultiplyVector(ray.direction));
*/
}
