using System.Numerics;
using Splines.Extensions;
using Splines.Unity;
using static Splines.Mathfs;

namespace Splines.GeometricShapes;

/// <summary>
/// A triangle in 3D space.
/// </summary>
[Serializable]
public partial struct Triangle3D
{
    /// <inheritdoc cref="Triangle2D.A"/>
    public Vector3 A
    {
        [Pure]
        get;
        set;
    }

    /// <inheritdoc cref="Triangle2D.B"/>
    public Vector3 B
    {
        [Pure]
        get;
        set;
    }

    /// <inheritdoc cref="Triangle2D.C"/>
    public Vector3 C
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Creates a 3D triangle.
    /// </summary>
    /// <param name="a">The first vertex in the triangle.</param>
    /// <param name="b">The second vertex in the triangle.</param>
    /// <param name="c">The third vertex in the triangle.</param>
    public Triangle3D(Vector2 a, Vector2 b, Vector2 c) =>
        (A, B, C) = (a.ToVector3(), b.ToVector3(), c.ToVector3());

    /// <inheritdoc cref="Triangle2D.GetAngle(int)"/>
    [Pure]
    public float GetAngle(int index)
    {
        return index switch
        {
            0 => AngleA,
            1 => AngleB,
            2 => AngleC,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Triangle indices have to be either 0, 1 or 2")
        };
    }

    /// <inheritdoc cref="Triangle2D.AngleA"/>
    [Pure]
    public float AngleA => AngleBetween(B - A, C - A);

    /// <inheritdoc cref="Triangle2D.AngleB"/>
    [Pure]
    public float AngleB => AngleBetween(C - B, A - B);

    /// <inheritdoc cref="Triangle2D.AngleC"/>
    [Pure]
    public float AngleC => AngleBetween(A - C, B - C);

    /// <inheritdoc cref="Triangle2D.Angles"/>
    [Pure]
    public (float angA, float angB, float angC) Angles
    {
        get
        {
            Vector2 abDir = (B - A).Normalized().ToVector2();
            Vector2 acDir = (C - A).Normalized().ToVector2();
            Vector2 bcDir = (C - B).Normalized().ToVector2();
            float angA = Mathf.Acos(Vector2.Dot(abDir, acDir).ClampNeg1to1());
            float angB = Mathf.Acos(Vector2.Dot(-abDir, bcDir).ClampNeg1to1());
            float angC = Mathf.PI - angA - angB;
            return (angA, angB, angC);
        }
    }

    /// <inheritdoc cref="Triangle2D.SmallestAngle"/>
    [Pure]
    public float SmallestAngle
    {
        get
        {
            (float angA, float angB, float angC) = Angles;
            return Min(angA, angB, angC);
        }
    }

    /// <inheritdoc cref="Triangle2D.LargestAngle"/>
    [Pure]
    public float LargestAngle
    {
        get
        {
            (float angA, float angB, float angC) = Angles;
            return Max(angA, angB, angC);
        }
    }

    /// <inheritdoc cref="Triangle2D.Area"/>
    [Pure]
    public float Area => Vector3.Cross(B - A, C - A).Magnitude() / 2f;

    /// <inheritdoc cref="Triangle2D.GetEdge(int)"/>
    [Pure]
    public float GetEdge(int index)
    {
        return index switch
        {
            0 => EdgeA,
            1 => EdgeB,
            2 => EdgeC,
            _ => throw new ArgumentOutOfRangeException(nameof(index), "Triangle indices have to be either 0, 1 or 2")
        };
    }

    /// <inheritdoc cref="Triangle2D.EdgeA"/>
    [Pure]
    public float EdgeA => Vector3.Distance(C, B);

    /// <inheritdoc cref="Triangle2D.EdgeB"/>
    [Pure]
    public float EdgeB => Vector3.Distance(A, C);

    /// <inheritdoc cref="Triangle2D.EdgeC"/>
    [Pure]
    public float EdgeC => Vector3.Distance(B, A);

    /// <inheritdoc cref="Triangle2D.Perimeter"/>
    [Pure]
    public float Perimeter => EdgeA + EdgeB + EdgeC;

    /// <inheritdoc cref="Triangle2D.Centroid"/>
    [Pure]
    public Vector3 Centroid => (A + B + C) / 3f;

    /// <summary>
    /// The normalized vector that is perpendicular to the triangle.
    /// </summary>
    [Pure]
    public Vector3 Normal
    {
        get
        {
            Vector3 ab = B - A;
            Vector3 ac = C - A;
            return Vector3.Cross(ab, ac).Normalized();
        }
    }

    /// <inheritdoc cref="Triangle2D.Incenter"/>
    [Pure]
    public Vector3 Incenter
    {
        get
        {
            float bc = Vector3.Distance(B, C);
            float ca = Vector3.Distance(C, A);
            float ab = Vector3.Distance(A, B);
            return (bc * A + ca * B + ab * C) / (bc + ca + ab);
        }
    }

    [Pure]
    public float Inradius => 2 * Area / Perimeter;

    [Pure]
    public Circle3D Incircle => new(Incenter, Normal, Inradius);

    [Pure]
    public Vector3 Circumcenter
    {
        get
        {
            Vector3 ac = C - A;
            Vector3 ab = B - A;
            Vector3 abPerp = Vector3.Cross(Vector3.Cross(ac, ab), ab);
            Vector3 acPerp = Vector3.Cross(Vector3.Cross(ab, ac), ac);

            float d = 2 * Vector3.Dot(ab, acPerp);
            float lenAb2 = ab.LengthSquared();
            float lenAc2 = ac.LengthSquared();

            return A + (lenAc2 * abPerp - lenAb2 * acPerp) / d;
        }
    }

    [Pure]
    public float Circumradius => EdgeA * EdgeB * EdgeC / (4 * Area);

    [Pure]
    public Circle3D Circumcircle => new(Circumcenter, Normal, Circumradius);

    [Pure]
    public Vector3 Orthocenter
    {
        get
        {
            // Calculate vectors for altitudes
            Vector3 AB = B - A;
            Vector3 AC = C - A;
            Vector3 BC = C - B;

            // Normal vector of the plane
            Vector3 n = Vector3.Cross(AB, AC);

            // Altitude from A
            Vector3 altitudeA = Vector3.Cross(n, BC);
            // Altitude from B
            Vector3 altitudeB = Vector3.Cross(n, -AC);

            // Lines from A and B
            Vector3 lineA = A + altitudeA;
            Vector3 lineB = B + altitudeB;

            // Find intersection of these lines
            if (TryGetLineIntersection(A, lineA, B, lineB, out Vector3 intersection))
            {
                return intersection;
            }

            throw new ArithmeticException("Cannot get the orthocenter of a triangle without area");
        }
    }

    /// <summary>
    /// Finds the intersection point of two lines in 3D space.
    /// </summary>
    [Pure]
    private static bool TryGetLineIntersection(
        Vector3 p1,
        Vector3 p2,
        Vector3 p3,
        Vector3 p4,
        out Vector3 intersection)
    {
        Vector3 p13 = p1 - p3;
        Vector3 p43 = p4 - p3;
        Vector3 p21 = p2 - p1;

        if (p43.LengthSquared() < float.Epsilon || p21.LengthSquared() < float.Epsilon)
        {
            intersection = Vector3.Zero;
            return false;
        }

        float d1343 = p13.X * p43.X + p13.Y * p43.Y + p13.Z * p43.Z;
        float d4321 = p43.X * p21.X + p43.Y * p21.Y + p43.Z * p21.Z;
        float d1321 = p13.X * p21.X + p13.Y * p21.Y + p13.Z * p21.Z;
        float d4343 = p43.X * p43.X + p43.Y * p43.Y + p43.Z * p43.Z;
        float d2121 = p21.X * p21.X + p21.Y * p21.Y + p21.Z * p21.Z;

        float denominator = d2121 * d4343 - d4321 * d4321;
        if (Math.Abs(denominator) < float.Epsilon)
        {
            intersection = Vector3.Zero;
            return false;
        }

        float numerator = d1343 * d4321 - d1321 * d4343;

        float mua = numerator / denominator;
        intersection = p1 + mua * p21;

        return true;
    }

    /// <summary>
    /// Returns a control point position by index. Valid indices: 0, 1, 2 or 3.
    /// </summary>
    /// <param name="i">The index of the control point.</param>
    /// <returns>The position of the control point.</returns>
    public Vector2 this[int i]
    {
        [Pure]
        get
        {
            return i switch
            {
                0 => A.ToVector2(),
                1 => B.ToVector2(),
                2 => C.ToVector2(),
                _ => throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know")
            };
        }
        set
        {
            switch (i)
            {
                case 0:
                    A = value.ToVector3();
                    break;
                case 1:
                    B = value.ToVector3();
                    break;
                case 2:
                    C = value.ToVector3();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i), $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know");
            }
        }
    }
}
