using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Splines.Unity;

/// <summary>
/// Represents a 2D vector with integer components.
/// </summary>
public struct Vector2Int : IEquatable<Vector2Int>, IFormattable
{
    /// <summary>
    /// Gets or sets the x-component of the vector.
    /// </summary>
    public int X
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    /// <summary>
    /// Gets or sets the y-component of the vector.
    /// </summary>
    public int Y
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2Int"/> struct with the specified x and y components.
    /// </summary>
    /// <param name="x">The x-component of the vector.</param>
    /// <param name="y">The y-component of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Sets the x and y components of the vector.
    /// </summary>
    /// <param name="x">The new x-component of the vector.</param>
    /// <param name="y">The new y-component of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int x, int y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Gets or sets the x or y component of the vector based on the specified index.
    /// </summary>
    /// <param name="index">The index of the component (0 for x, 1 for y).</param>
    /// <returns>The component of the vector at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is not 0 or 1.</exception>
    public int this[int index]
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                _ => throw new IndexOutOfRangeException($"Invalid Vector2Int index addressed: {index}!")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                default:
                    throw new IndexOutOfRangeException($"Invalid Vector2Int index addressed: {index}!");
            }
        }
    }

    /// <summary>
    /// Returns the length of this vector (RO).
    /// </summary>
    public float Magnitude
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Mathf.Sqrt(X * X + Y * Y);
    }

    /// <summary>
    /// Gets the squared length of the vector.
    /// </summary>
    public int SqrMagnitude
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y;
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The distance between the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector2Int a, Vector2Int b)
    {
        float diff_x = a.X - b.X;
        float diff_y = a.Y - b.Y;
        return (float)Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
    }

    /// <summary>
    /// Returns a vector made from the smallest components of two vectors.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>A vector made from the smallest components of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs) => new(Mathf.Min(lhs.X, rhs.X), Mathf.Min(lhs.Y, rhs.Y));

    /// <summary>
    /// Returns a vector made from the largest components of two vectors.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>A vector made from the largest components of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs) => new(Mathf.Max(lhs.X, rhs.X), Mathf.Max(lhs.Y, rhs.Y));

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise multiplication of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int Scale(Vector2Int a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);

    /// <summary>
    /// Multiplies every component of this vector by the same component of the specified scale vector.
    /// </summary>
    /// <param name="scale">The scale vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Scale(Vector2Int scale)
    {
        X *= scale.X;
        Y *= scale.Y;
    }

    /// <summary>
    /// Clamps the vector components within the specified minimum and maximum vectors.
    /// </summary>
    /// <param name="min">The minimum vector.</param>
    /// <param name="max">The maximum vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clamp(Vector2Int min, Vector2Int max)
    {
        X = Math.Max(min.X, X);
        X = Math.Min(max.X, X);
        Y = Math.Max(min.Y, Y);
        Y = Math.Min(max.Y, Y);
    }

    /// <summary>
    /// Implicitly converts a Vector2Int to a Vector2.
    /// </summary>
    /// <param name="v">The Vector2Int to convert.</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector2(Vector2Int v) => new(v.X, v.Y);

    /// <summary>
    /// Explicitly converts a Vector2Int to a Vector3Int.
    /// </summary>
    /// <param name="v">The Vector2Int to convert.</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector3Int(Vector2Int v) => new(v.X, v.Y, 0);

    /// <summary>
    /// Floors the components of a Vector2 to the nearest lower integer values and returns them as a Vector2Int.
    /// </summary>
    /// <param name="v">The Vector2 to convert.</param>
    /// <returns>A Vector2Int with components floored to the nearest lower integer values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int FloorToInt(Vector2 v)
    {
        return new Vector2Int(
            Mathf.FloorToInt(v.X),
            Mathf.FloorToInt(v.Y)
       );
    }

    /// <summary>
    /// Ceils the components of a Vector2 to the nearest higher integer values and returns them as a Vector2Int.
    /// </summary>
    /// <param name="v">The Vector2 to convert.</param>
    /// <returns>A Vector2Int with components ceiled to the nearest higher integer values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int CeilToInt(Vector2 v)
    {
        return new Vector2Int(
            Mathf.CeilToInt(v.X),
            Mathf.CeilToInt(v.Y)
       );
    }

    /// <summary>
    /// Rounds the components of a Vector2 to the nearest integer values and returns them as a Vector2Int.
    /// </summary>
    /// <param name="v">The Vector2 to convert.</param>
    /// <returns>A Vector2Int with components rounded to the nearest integer values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int RoundToInt(Vector2 v)
    {
        return new Vector2Int(
            Mathf.RoundToInt(v.X),
            Mathf.RoundToInt(v.Y)
       );
    }

    /// <summary>
    /// Negates the components of the vector.
    /// </summary>
    /// <param name="v">The vector to negate.</param>
    /// <returns>A vector with negated components.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator-(Vector2Int v) => new(-v.X, -v.Y);

    /// <summary>
    /// Adds two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise addition of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator+(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);

    /// <summary>
    /// Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise subtraction of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator-(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise multiplication of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator*(Vector2Int a, Vector2Int b) => new(a.X * b.X, a.Y * b.Y);

    /// <summary>
    /// Multiplies each component of the vector by the specified scalar.
    /// </summary>
    /// <param name="a">The scalar value.</param>
    /// <param name="b">The vector to multiply.</param>
    /// <returns>A vector with each component multiplied by the scalar.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator*(int a, Vector2Int b) => new(a * b.X, a * b.Y);

    /// <summary>
    /// Multiplies each component of the vector by the specified scalar.
    /// </summary>
    /// <param name="a">The vector to multiply.</param>
    /// <param name="b">The scalar value.</param>
    /// <returns>A vector with each component multiplied by the scalar.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator*(Vector2Int a, int b) => new(a.X * b, a.Y * b);

    /// <summary>
    /// Divides each component of the vector by the specified scalar.
    /// </summary>
    /// <param name="a">The vector to divide.</param>
    /// <param name="b">The scalar value.</param>
    /// <returns>A vector with each component divided by the scalar.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Int operator/(Vector2Int a, int b) => new(a.X / b, a.Y / b);

    /// <summary>
    /// Determines whether two vectors are equal.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>true if the vectors are equal; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator==(Vector2Int lhs, Vector2Int rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y;

    /// <summary>
    /// Determines whether two vectors are not equal.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>true if the vectors are not equal; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator!=(Vector2Int lhs, Vector2Int rhs) => !(lhs == rhs);

    /// <summary>
    /// Determines whether the specified object is equal to the current vector.
    /// </summary>
    /// <param name="other">The object to compare with the current vector.</param>
    /// <returns>true if the specified object is equal to the current vector; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? other) => other is Vector2Int vector2Int && Equals(vector2Int);

    /// <summary>
    /// Determines whether the specified vector is equal to the current vector.
    /// </summary>
    /// <param name="other">The vector to compare with the current vector.</param>
    /// <returns>true if the specified vector is equal to the current vector; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector2Int other)
    {
        return X == other.X && Y == other.Y;
    }

    /// <summary>
    /// Returns the hash code for this vector.
    /// </summary>
    /// <returns>A hash code for the current vector.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(X, Y);

    /// <summary>
    /// Returns a string that represents the current vector.
    /// </summary>
    /// <returns>A string that represents the current vector.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => ToString("F2", CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a string that represents the current vector using the specified format.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <returns>A string that represents the current vector.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a string that represents the current vector using the specified format and format provider.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <returns>A string that represents the current vector.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        format ??= "0";
        formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;

        return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)})";
    }

    /// <summary>
    /// Gets a vector with both components set to zero.
    /// </summary>
    [Pure]
    public static Vector2Int Zero
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, 0);

    /// <summary>
    /// Gets a vector with both components set to one.
    /// </summary>
    [Pure]
    public static Vector2Int One
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(1, 1);

    /// <summary>
    /// Gets a vector pointing up (0, 1).
    /// </summary>
    [Pure]
    public static Vector2Int Up
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, 1);

    /// <summary>
    /// Gets a vector pointing down (0, -1).
    /// </summary>
    [Pure]
    public static Vector2Int Down
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, -1);

    /// <summary>
    /// Gets a vector pointing left (-1, 0).
    /// </summary>
    [Pure]
    public static Vector2Int Left
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(-1, 0);

    /// <summary>
    /// Gets a vector pointing right (1, 0).
    /// </summary>
    [Pure]
    public static Vector2Int Right
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(1, 0);
}
