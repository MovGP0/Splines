using System.Numerics;
using System.Runtime.CompilerServices;
using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>A 3D rectangular box, also known as a cuboid</summary>
[Serializable]
public partial struct Box3D
{
    /// <summary>The number of vertices in a cuboid</summary>
    public const int VertexCount = 8;

    /// <summary>The center of this cuboid</summary>
    public Vector3 Center
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The extents of this cuboid (distance from the center to the edge) per axis</summary>
    public Vector3 Extents
    {
        [Pure]
        get;
        set;
    }

    /// <inheritdoc cref="Box2D.ClosestPointOnBoundary"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public Vector3 ClosestPointOnBoundary(Vector3 point)
    {
        float px = point.X - Center.X;
        float py = point.Y - Center.Y;
        float pz = point.Z - Center.Z;
        float ax = Mathfs.Abs(px);
        float ay = Mathfs.Abs(py);
        float az = Mathfs.Abs(pz);
        float dx = ax - Extents.X;
        float dy = ay - Extents.Y;
        float dz = az - Extents.Z;
        bool caseX = dz <= dx && dy <= dx;
        bool caseY = caseX == false && dx <= dy && dz <= dy;
        bool caseZ = caseX == false && caseY == false;
        return new Vector3(
            Center.X + Mathfs.Sign(px) * (caseX ? Extents.X : ax.AtMost(Extents.X)),
            Center.Y + Mathfs.Sign(py) * (caseY ? Extents.Y : ay.AtMost(Extents.Y)),
            Center.Z + Mathfs.Sign(pz) * (caseZ ? Extents.Z : az.AtMost(Extents.Z))
       );
    }

    /// <inheritdoc cref="Box2D.ClosestPointInside"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public Vector3 ClosestPointInside(Vector3 point) =>
        new(
            point.X.Clamp(Center.X - Extents.X, Center.X + Extents.X),
            point.Y.Clamp(Center.Y - Extents.Y, Center.Y + Extents.Y),
            point.Z.Clamp(Center.Z - Extents.Z, Center.Z + Extents.Z)
       );

    /// <inheritdoc cref="Box2D.ClosestCorner"/>
    [Pure]
    public Vector3 ClosestCorner(Vector3 point) =>
        new(
            Center.X + Mathfs.Sign(point.X - Center.X) * Extents.X,
            Center.Y + Mathfs.Sign(point.Y - Center.Y) * Extents.Y,
            Center.Z + Mathfs.Sign(point.Z - Center.Z) * Extents.Z
       );

    /// <inheritdoc cref="Box2D.Encapsulate"/>
    public void Encapsulate(Vector3 point)
    {
        float minX = Mathfs.Min(Center.X - Extents.X, point.X);
        float minY = Mathfs.Min(Center.Y - Extents.Y, point.Y);
        float minZ = Mathfs.Min(Center.Z - Extents.Z, point.Z);
        float maxX = Mathfs.Max(Center.X + Extents.X, point.X);
        float maxY = Mathfs.Max(Center.Y + Extents.Y, point.Y);
        float maxZ = Mathfs.Max(Center.Z + Extents.Z, point.Z);
        Center = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, (maxZ + minZ) / 2);
        Extents = new Vector3((maxX - minX) / 2, (maxY - minY) / 2, (maxZ - minZ) / 2);
    }

    /// <inheritdoc cref="Box2D.Contains"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Pure]
    public bool Contains(Vector3 point) => Mathfs.Abs(point.X - Center.X) - Extents.X <= 0 &&
                                           Mathfs.Abs(point.Y - Center.Y) - Extents.Y <= 0 &&
                                           Mathfs.Abs(point.Z - Center.Z) - Extents.Z <= 0;

    /// <summary>The total surface area of this cuboid</summary>
    public float SurfaceArea
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => 4 * (Extents.Y * (Extents.X + Extents.Z) + Extents.Z * Extents.X);
    }

    /// <summary>The volume of this cuboid</summary>
    public float Volume
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.X * Extents.Y * Extents.Z * 8;
    }

    /// <inheritdoc cref="Box2D.GetVertex"/>
    [Pure]
    public Vector3 GetVertex(int index)
    {
        return index switch
        {
            0 => new Vector3(Center.X - Extents.X, Center.Y - Extents.Y, Center.Z - Extents.Z),
            1 => new Vector3(Center.X - Extents.X, Center.Y - Extents.Y, Center.Z + Extents.Z),
            2 => new Vector3(Center.X - Extents.X, Center.Y + Extents.Y, Center.Z - Extents.Z),
            3 => new Vector3(Center.X - Extents.X, Center.Y + Extents.Y, Center.Z + Extents.Z),
            4 => new Vector3(Center.X + Extents.X, Center.Y - Extents.Y, Center.Z - Extents.Z),
            5 => new Vector3(Center.X + Extents.X, Center.Y - Extents.Y, Center.Z + Extents.Z),
            6 => new Vector3(Center.X + Extents.X, Center.Y + Extents.Y, Center.Z - Extents.Z),
            7 => new Vector3(Center.X + Extents.X, Center.Y + Extents.Y, Center.Z + Extents.Z),
            _ => throw new ArgumentOutOfRangeException(nameof(index),
                $"Invalid index: {index}. Valid vertex indices range from 0 to {VertexCount - 1}")
        };
    }

    /// <inheritdoc cref="Box2D.Min"/>
    public Vector3 Min
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => new(Center.X - Extents.X, Center.Y - Extents.Y, Center.Z - Extents.Z);
    }

    /// <inheritdoc cref="Box2D.Max"/>
    public Vector3 Max
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => new(Center.X + Extents.X, Center.Y + Extents.Y, Center.Z + Extents.Z);
    }

    /// <inheritdoc cref="Box2D.Size"/>
    public Vector3 Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents * 2;
    }

    /// <inheritdoc cref="Box2D.Width"/>
    public float Width
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.X * 2;
    }

    /// <inheritdoc cref="Box2D.Height"/>
    public float Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.Y * 2;
    }

    /// <summary>The depth of this box</summary>
    public float Depth
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        get => Extents.Z * 2;
    }
}
