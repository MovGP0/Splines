using System.Collections.Generic;
using System.Numerics;
using Splines.Numerics;

namespace Splines.GeometricShapes;

/// <summary>
/// Represents a section of a polygon that is determined by a range of parameter values.
/// </summary>
public sealed class PolygonSection : IComparable<PolygonSection>, IEquatable<PolygonSection>
{
    /// <summary>
    /// Gets the range of parameter values that define this polygon section.
    /// </summary>
    public FloatRange tRange { get; }

    /// <summary>
    /// Gets the list of points that make up this polygon section.
    /// </summary>
    public List<Vector2> Points { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonSection"/> class with the specified parameter range and points.
    /// </summary>
    /// <param name="tRange">The range of parameter values that define this polygon section.</param>
    /// <param name="points">The points that make up this polygon section.</param>
    public PolygonSection(FloatRange tRange, List<Vector2> points)
        => (this.tRange, Points) = (tRange, points);

    /// <summary>
    /// Compares the current instance with another <see cref="PolygonSection"/> object based on the minimum value of their parameter ranges.
    /// </summary>
    /// <param name="other">The other <see cref="PolygonSection"/> to compare with this instance.</param>
    /// <returns>A value indicating the relative order of the objects being compared.
    /// The return value is less than zero if this instance is less than <paramref name="other"/>, zero if they are equal,
    /// and greater than zero if this instance is greater than <paramref name="other"/>.</returns>
    [Pure]
    public int CompareTo(PolygonSection? other) => other is null ? 1 : tRange.Min.CompareTo(other.tRange.Min);

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
    [Pure]
    public bool Equals(PolygonSection? other)
    {
        if (other == null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return tRange.Equals(other.tRange) && PointsEqual(Points, other.Points);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is PolygonSection polygonSection && Equals(polygonSection);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(tRange, HashCode.Combine(Points));

    /// <summary>
    /// Compares two lists of points for equality.
    /// </summary>
    /// <param name="points1">The first list of points.</param>
    /// <param name="points2">The second list of points.</param>
    /// <returns>true if the lists are equal; otherwise, false.</returns>
    [Pure]
    private static bool PointsEqual(List<Vector2>? points1, List<Vector2>? points2)
    {
        if (points1 == null || points2 == null)
            return false;

        if (points1.Count != points2.Count)
            return false;

        for (int i = 0; i < points1.Count; i++)
        {
            if (points1[i] != points2[i])
            {
                return false;
            }
        }

        return true;
    }
}
