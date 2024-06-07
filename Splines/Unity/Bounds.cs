using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Splines.Unity;

/// <summary>
/// Represents an axis-aligned bounding box defined by a center and extents.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct Bounds : IEquatable<Bounds>, IFormattable
{
    private Vector3 m_Center;
    private Vector3 m_Extents;

    /// <summary>
    /// Initializes a new instance of the <see cref="Bounds"/> struct with a given center and total size.
    /// The extents will be half the given size.
    /// </summary>
    /// <param name="center">The center of the bounding box.</param>
    /// <param name="size">The total size of the bounding box.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Bounds(Vector3 center, Vector3 size)
    {
        m_Center = center;
        m_Extents = size * 0.5F;
    }

    /// <summary>
    /// Gets the hash code for this bounds.
    /// </summary>
    /// <returns>A hash code for the current bounds.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(Center, Extents);

    /// <summary>
    /// Determines whether the specified object is equal to the current bounds.
    /// </summary>
    /// <param name="other">The object to compare with the current bounds.</param>
    /// <returns>true if the specified object is equal to the current bounds; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? other) => other is Bounds bounds && Equals(bounds);

    /// <summary>
    /// Determines whether the specified bounds are equal to the current bounds.
    /// </summary>
    /// <param name="other">The bounds to compare with the current bounds.</param>
    /// <returns>true if the specified bounds are equal to the current bounds; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Bounds other) => Center.Equals(other.Center) && Extents.Equals(other.Extents);

    /// <summary>
    /// Gets or sets the center of the bounding box.
    /// </summary>
    public Vector3 Center
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_Center;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => m_Center = value;
    }

    /// <summary>
    /// Gets or sets the total size of the bounding box. This is always twice as large as the extents.
    /// </summary>
    public Vector3 Size
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_Extents * 2.0F;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => m_Extents = value * 0.5F;
    }

    /// <summary>
    /// Gets or sets the extents of the bounding box. This is always half of the size.
    /// </summary>
    public Vector3 Extents
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => m_Extents;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => m_Extents = value;
    }

    /// <summary>
    /// Gets or sets the minimal point of the bounding box. This is always equal to center - extents.
    /// </summary>
    public Vector3 Min
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Center - Extents;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => SetMinMax(value, Max);
    }

    /// <summary>
    /// Gets or sets the maximal point of the bounding box. This is always equal to center + extents.
    /// </summary>
    public Vector3 Max
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Center + Extents;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => SetMinMax(Min, value);
    }

    /// <summary>
    /// Determines whether two bounds instances are equal.
    /// </summary>
    /// <param name="lhs">The first bounds to compare.</param>
    /// <param name="rhs">The second bounds to compare.</param>
    /// <returns>true if the bounds are equal; otherwise, false.
    /// false in the presence of NaN values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator==(Bounds lhs, Bounds rhs) => lhs.Center == rhs.Center && lhs.Extents == rhs.Extents;

    /// <summary>
    /// Determines whether two bounds instances are not equal.
    /// </summary>
    /// <param name="lhs">The first bounds to compare.</param>
    /// <param name="rhs">The second bounds to compare.</param>
    /// <returns>true if the bounds are not equal; otherwise, false.
    /// true in the presence of NaN values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator!=(Bounds lhs, Bounds rhs) => !(lhs == rhs);

    /// <summary>
    /// Sets the bounds to the min and max values of the box.
    /// </summary>
    /// <param name="min">The minimal point of the bounding box.</param>
    /// <param name="max">The maximal point of the bounding box.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetMinMax(Vector3 min, Vector3 max)
    {
        Extents = (max - min) * 0.5F;
        Center = min + Extents;
    }

    /// <summary>
    /// Grows the bounds to include the specified point.
    /// </summary>
    /// <param name="point">The point to include.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Encapsulate(Vector3 point)
    {
        SetMinMax(Vector3.Min(Min, point), Vector3.Max(Max, point));
    }

    /// <summary>
    /// Grows the bounds to include the specified bounds.
    /// </summary>
    /// <param name="bounds">The bounds to include.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Encapsulate(Bounds bounds)
    {
        Encapsulate(bounds.Center - bounds.Extents);
        Encapsulate(bounds.Center + bounds.Extents);
    }

    /// <summary>
    /// Expands the bounds by increasing its size by the specified amount along each side.
    /// </summary>
    /// <param name="amount">The amount to expand the bounds.</param>
    public void Expand(float amount)
    {
        amount *= 0.5f;
        Extents += new Vector3(amount, amount, amount);
    }

    /// <summary>
    /// Expands the bounds by increasing its size by the specified amount along each side.
    /// </summary>
    /// <param name="amount">The amount to expand the bounds.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Expand(Vector3 amount)
    {
        Extents += amount * 0.5f;
    }

    /// <summary>
    /// Determines whether another bounding box intersects with this bounding box.
    /// </summary>
    /// <param name="bounds">The bounding box to check for intersection.</param>
    /// <returns>true if the bounding boxes intersect; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Bounds bounds)
    {
        return (Min.X <= bounds.Max.X) && (Max.X >= bounds.Min.X) &&
               (Min.Y <= bounds.Max.Y) && (Max.Y >= bounds.Min.Y) &&
               (Min.Z <= bounds.Max.Z) && (Max.Z >= bounds.Min.Z);
    }

    /// <summary>
    /// Returns a string that represents the current bounds.
    /// </summary>
    /// <returns>A string that represents the current bounds.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => ToString("F2", CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a formatted string that represents the current bounds.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <returns>A formatted string that represents the current bounds.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a formatted string that represents the current bounds.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <returns>A formatted string that represents the current bounds.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format))
            format = "F2";

        formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;

        return $"Center: {m_Center.ToString(format, formatProvider)}, Extents: {m_Extents.ToString(format, formatProvider)}";
    }
}
