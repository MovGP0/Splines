using System.Numerics;
using Splines.Extensions;
using Splines.Unity;

namespace Splines.GeometricShapes;

/// <summary>A triangle in 2D space</summary>
[Serializable]
public partial struct Triangle2D
{
    /// <summary>The first vertex of the triangle</summary>
    public Vector2 A
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The second vertex of the triangle</summary>
    public Vector2 B
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The third vertex of the triangle</summary>
    public Vector2 C
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a 2D triangle</summary>
    /// <param name="a">The first vertex in the triangle</param>
    /// <param name="b">The second vertex in the triangle</param>
    /// <param name="c">The third vertex in the triangle</param>
    public Triangle2D(Vector2 a, Vector2 b, Vector2 c) => (A, B, C) = (a, b, c);

    /// <summary>Returns the internal angle of a given vertex</summary>
    /// <param name="index">The vertex to get the angle of. Valid values: 0, 1 or 2</param>
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

    /// <summary>The angle at vertex A</summary>
    [Pure]
    public float AngleA => Mathfs.AngleBetween(B - A, C - A);

    /// <summary>The angle at vertex B</summary>
    [Pure]
    public float AngleB => Mathfs.AngleBetween(C - B, A - B);

    /// <summary>The angle at vertex C</summary>
    [Pure]
    public float AngleC => Mathfs.AngleBetween(A - C, B - C);

    /// <summary>The angles of each vertex. The sum of these is always half a turn = tau/2 = pi</summary>
    [Pure]
    public (float angA, float angB, float angC) Angles
    {
        get
        {
            Vector2 abDir = (B - A).Normalized();
            Vector2 acDir = (C - A).Normalized();
            Vector2 bcDir = (C - B).Normalized();
            float angA = Mathf.Acos(Vector2.Dot(abDir, acDir).ClampNeg1to1());
            float angB = Mathf.Acos(Vector2.Dot(-abDir, bcDir).ClampNeg1to1());
            float angC = Mathfs.PI - angA - angB;
            return (angA, angB, angC);
        }
    }

    /// <summary>The smallest of the three angles</summary>
    [Pure]
    public float SmallestAngle
    {
        get
        {
            (float angA, float angB, float angC) = Angles;
            return Mathfs.Min(angA, angB, angC);
        }
    }

    /// <summary>The largest of the three angles</summary>
    [Pure]
    public float LargestAngle
    {
        get
        {
            (float angA, float angB, float angC) = Angles;
            return Mathfs.Max(angA, angB, angC);
        }
    }

    /// <summary>Returns the length of the side opposite the given vertex index</summary>
    /// <param name="index">The vertex index opposite the side to get the length of. Valid values: 0, 1 or 2</param>
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

    /// <summary>The length of the edge opposite to vertex A</summary>
    [Pure]
    public float EdgeA => Vector2.Distance(C, B);

    /// <summary>The length of the edge opposite to vertex B</summary>
    [Pure]
    public float EdgeB => Vector2.Distance(A, C);

    /// <summary>The length of the edge opposite to vertex C</summary>
    [Pure]
    public float EdgeC => Vector2.Distance(B, A);

    /// <summary>The perimeter of the triangle, equivalent to the sum of all edge lengths</summary>
    [Pure]
    public float Perimeter => EdgeA + EdgeB + EdgeC;

    /// <summary>Checks whether a point is inside this triangle</summary>
    /// <param name="point">The point to test if it's inside or not</param>
    /// <param name="aMargin">Optional floating point offset for margin testing on side A. Note: this flips direction depending on if the triangle is clockwise or counter-clockwise</param>
    /// <param name="bMargin">Optional floating point offset for margin testing on side B. Note: this flips direction depending on if the triangle is clockwise or counter-clockwise</param>
    /// <param name="cMargin">Optional floating point offset for margin testing on side C. Note: this flips direction depending on if the triangle is clockwise or counter-clockwise</param>
    [Pure]
    public bool Contains(Vector2 point, float aMargin = 0f, float bMargin = 0f, float cMargin = 0f)
    {
        float d0 = Mathfs.Determinant(B - A, point - A);
        float d1 = Mathfs.Determinant(C - B, point - B);
        float d2 = Mathfs.Determinant(A - C, point - C);
        bool b0 = d0 < cMargin;
        bool b1 = d1 < aMargin;
        bool b2 = d2 < bMargin;
        return b0 == b1 && b1 == b2; // on the same side of all halfspaces, this can only happen inside
    }

    /// <summary>The area of the triangle</summary>
    [Pure]
    public float Area => Mathf.Abs(SignedArea);

    // todo: verify clockwise vs ccw
    /// <summary>The signed area of the triangle. When the triangle is defined clockwise, the area will be negative</summary>
    [Pure]
    public float SignedArea => Mathfs.Determinant(B - A, C - A) / 2f;

    /// <summary>
    /// The inradius of this triangle, which is the radius of the largest circle that can fit inside of it.
    /// </summary>
    [Pure]
    public float Inradius
    {
        get
        {
            float s = Perimeter * 0.5f;
            return Mathf.Sqrt((s - EdgeA) * (s - EdgeB) * (s - EdgeC) / s);
        }
    }

    /// <summary>
    /// The incircle of this triangle, which is the largest circle that can fit inside of it. Its center is the incenter.
    /// </summary>
    [Pure]
    public Circle2D Incircle => new(Incenter, Inradius);

    /// <summary>The circumcircle of a triangle, which is the unique circle passing through all three vertices. Note that if the triangle is degenerate, no incircle exists</summary>
    [Pure]
    public Circle2D Circumcircle => Circle2D.FromThreePoints(A, B, C, out Circle2D circle) ? circle : default;

    /// <summary>
    /// The circumradius of this triangle, which is the radius of the smallest circle that can fit around it.
    /// </summary>
    [Pure]
    public float Circumradius => Circumcircle.Radius;

    /// <summary>Returns a control point position by index. Valid indices: 0, 1, 2 or 3</summary>
    public Vector2 this[int i]
    {
        [Pure]
        get
        {
            return i switch
            {
                0 => A,
                1 => B,
                2 => C,
                _ => throw new ArgumentOutOfRangeException(nameof(i),
                    $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know")
            };
        }
        set
        {
            switch (i)
            {
                case 0:
                    A = value;
                    break;
                case 1:
                    B = value;
                    break;
                case 2:
                    C = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(i),
                        $"Index has to be in the 0 to 2 range, and I think {i} is outside that range you know");
            }
        }
    }

    /// <summary>The centroid of the triangle, which is the point of intersection of its medians (the lines joining each vertex with the midpoint of the opposite side).
    /// Physically, this would also be its center of mass</summary>
    [Pure]
    public Vector2 Centroid => (A + B + C) / 3f;

    /// <summary>The incenter of a triangle, which is the point of intersection of the angular bisectors.
    /// This is also the center of the incircle, the largest circle that can fit in the triangle.</summary>
    [Pure]
    public Vector2 Incenter
    {
        get
        {
            float bc = Vector2.Distance(B, C);
            float ca = Vector2.Distance(C, A);
            float ab = Vector2.Distance(A, B);
            return (bc * A + ca * B + ab * C) / (bc + ca + ab);
        }
    }

    /// <summary>The intersection of the perpendicular bisectors of each edge of the triangle</summary>
    [Pure]
    public Vector2 Circumcenter
    {
        get
        {
            Line2D bsA = LineSegment2D.GetBisector(A, B);
            Line2D bsB = LineSegment2D.GetBisector(B, C);
            if (bsA.Intersect(bsB, out Vector2 intPt))
            {
                return intPt;
            }

            throw new ArithmeticException("Cannot get the circumcenter of a triangle without area");
        }
    }

    /// <summary>The intersection of the altitudes of the triangle</summary>
    [Pure]
    public Vector2 Orthocenter
    {
        get
        {
            Line2D bsA = new Line2D(A, (B - C).Rotate90CW());
            Line2D bsB = new Line2D(B, (C - A).Rotate90CW());
            if (bsA.Intersect(bsB, out Vector2 intPt))
            {
                return intPt;
            }

            throw new ArithmeticException("Cannot get the orthocenter of a triangle without area");
        }
    }
}
