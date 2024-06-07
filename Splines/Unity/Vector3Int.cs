using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Splines.Unity;

/// <summary>
/// Represents a 3D vector with integer components.
/// </summary>
public struct Vector3Int : IEquatable<Vector3Int>, IFormattable
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
    /// Gets or sets the z-component of the vector.
    /// </summary>
    public int Z
    {
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct with the specified x and y components. The z-component is set to 0.
    /// </summary>
    /// <param name="x">The x-component of the vector.</param>
    /// <param name="y">The y-component of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3Int(int x, int y)
    {
        X = x;
        Y = y;
        Z = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Int"/> struct with the specified x, y, and z components.
    /// </summary>
    /// <param name="x">The x-component of the vector.</param>
    /// <param name="y">The y-component of the vector.</param>
    /// <param name="z">The z-component of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector3Int(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Sets the x, y, and z components of the vector.
    /// </summary>
    /// <param name="x">The new x-component of the vector.</param>
    /// <param name="y">The new y-component of the vector.</param>
    /// <param name="z">The new z-component of the vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Set(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Gets or sets the x, y, or z component of the vector based on the specified index.
    /// </summary>
    /// <param name="index">The index of the component (0 for x, 1 for y, 2 for z).</param>
    /// <returns>The component of the vector at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is not 0, 1, or 2.</exception>
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
                2 => Z,
                _ => throw new IndexOutOfRangeException($"Invalid Vector3Int index addressed: {index}!")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                default:
                    throw new IndexOutOfRangeException($"Invalid Vector3Int index addressed: {index}!");
            }
        }
    }

    /// <summary>
    /// Gets the length of the vector.
    /// </summary>
    [Pure]
    public float Magnitude
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Mathf.Sqrt(SqrMagnitude);
    }

    /// <summary>
    /// Gets the squared length of the vector.
    /// </summary>
    [Pure]
    public int SqrMagnitude
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => X * X + Y * Y + Z * Z;
    }

    /// <summary>
    /// Returns the distance between two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>The distance between the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(Vector3Int a, Vector3Int b) { return (a - b).Magnitude; }

    /// <summary>
    /// Returns a vector made from the smallest components of two vectors.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>A vector made from the smallest components of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int Min(Vector3Int lhs, Vector3Int rhs) => new(Mathf.Min(lhs.X, rhs.X), Mathf.Min(lhs.Y, rhs.Y), Mathf.Min(lhs.Z, rhs.Z));

    /// <summary>
    /// Returns a vector made from the largest components of two vectors.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>A vector made from the largest components of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int Max(Vector3Int lhs, Vector3Int rhs) => new(Mathf.Max(lhs.X, rhs.X), Mathf.Max(lhs.Y, rhs.Y), Mathf.Max(lhs.Z, rhs.Z));

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise multiplication of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int Scale(Vector3Int a, Vector3Int b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    /// <summary>
    /// Multiplies every component of this vector by the same component of the specified scale vector.
    /// </summary>
    /// <param name="scale">The scale vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Scale(Vector3Int scale) { X *= scale.X; Y *= scale.Y; Z *= scale.Z; }

    /// <summary>
    /// Clamps the vector components within the specified minimum and maximum vectors.
    /// </summary>
    /// <param name="min">The minimum vector.</param>
    /// <param name="max">The maximum vector.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clamp(Vector3Int min, Vector3Int max)
    {
        X = Math.Max(min.X, X);
        X = Math.Min(max.X, X);
        Y = Math.Max(min.Y, Y);
        Y = Math.Min(max.Y, Y);
        Z = Math.Max(min.Z, Z);
        Z = Math.Min(max.Z, Z);
    }

    /// <summary>
    /// Implicitly converts a Vector3Int to a Vector3.
    /// </summary>
    /// <param name="v">The Vector3Int to convert.</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Vector3(Vector3Int v) => new(v.X, v.Y, v.Z);

    /// <summary>
    /// Explicitly converts a Vector3Int to a Vector2Int.
    /// </summary>
    /// <param name="v">The Vector3Int to convert.</param>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Vector2Int(Vector3Int v) => new(v.X, v.Y);

    /// <summary>
    /// Floors the components of a Vector3 to the nearest lower integer values and returns them as a Vector3Int.
    /// </summary>
    /// <param name="v">The Vector3 to convert.</param>
    /// <returns>A Vector3Int with components floored to the nearest lower integer values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int FloorToInt(Vector3 v)
    {
        return new Vector3Int(
            Mathf.FloorToInt(v.X),
            Mathf.FloorToInt(v.Y),
            Mathf.FloorToInt(v.Z)
       );
    }

    /// <summary>
    /// Ceils the components of a Vector3 to the nearest higher integer values and returns them as a Vector3Int.
    /// </summary>
    /// <param name="v">The Vector3 to convert.</param>
    /// <returns>A Vector3Int with components ceiled to the nearest higher integer values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int CeilToInt(Vector3 v)
    {
        return new Vector3Int(
            Mathf.CeilToInt(v.X),
            Mathf.CeilToInt(v.Y),
            Mathf.CeilToInt(v.Z)
       );
    }

    /// <summary>
    /// Rounds the components of a Vector3 to the nearest integer values and returns them as a Vector3Int.
    /// </summary>
    /// <param name="v">The Vector3 to convert.</param>
    /// <returns>A Vector3Int with components rounded to the nearest integer values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int RoundToInt(Vector3 v)
    {
        return new Vector3Int(
            Mathf.RoundToInt(v.X),
            Mathf.RoundToInt(v.Y),
            Mathf.RoundToInt(v.Z)
       );
    }

    /// <summary>
    /// Adds two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise addition of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator+(Vector3Int a, Vector3Int b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    /// <summary>
    /// Subtracts the second vector from the first vector component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise subtraction of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator-(Vector3Int a, Vector3Int b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    /// <returns>A vector that is the component-wise multiplication of the two vectors.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator*(Vector3Int a, Vector3Int b) => new(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    /// <summary>
    /// Negates the components of the vector.
    /// </summary>
    /// <param name="a">The vector to negate.</param>
    /// <returns>A vector with negated components.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator-(Vector3Int a) => new(-a.X, -a.Y, -a.Z);

    /// <summary>
    /// Multiplies each component of the vector by the specified scalar.
    /// </summary>
    /// <param name="a">The vector to multiply.</param>
    /// <param name="b">The scalar value.</param>
    /// <returns>A vector with each component multiplied by the scalar.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator*(Vector3Int a, int b) => new(a.X * b, a.Y * b, a.Z * b);

    /// <summary>
    /// Multiplies each component of the vector by the specified scalar.
    /// </summary>
    /// <param name="a">The scalar value.</param>
    /// <param name="b">The vector to multiply.</param>
    /// <returns>A vector with each component multiplied by the scalar.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator*(int a, Vector3Int b) => new(a * b.X, a * b.Y, a * b.Z);

    /// <summary>
    /// Divides each component of the vector by the specified scalar.
    /// </summary>
    /// <param name="a">The vector to divide.</param>
    /// <param name="b">The scalar value.</param>
    /// <returns>A vector with each component divided by the scalar.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3Int operator/(Vector3Int a, int b) => new(a.X / b, a.Y / b, a.Z / b);

    /// <summary>
    /// Determines whether two vectors are equal.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>true if the vectors are equal; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator==(Vector3Int lhs, Vector3Int rhs) => lhs.X == rhs.X && lhs.Y == rhs.Y && lhs.Z == rhs.Z;

    /// <summary>
    /// Determines whether two vectors are not equal.
    /// </summary>
    /// <param name="lhs">The first vector.</param>
    /// <param name="rhs">The second vector.</param>
    /// <returns>true if the vectors are not equal; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator!=(Vector3Int lhs, Vector3Int rhs) => !(lhs == rhs);

    /// <summary>
    /// Determines whether the specified object is equal to the current vector.
    /// </summary>
    /// <param name="other">The object to compare with the current vector.</param>
    /// <returns>true if the specified object is equal to the current vector; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? other) => other is Vector3Int vector3Int && Equals(vector3Int);

    /// <summary>
    /// Determines whether the specified vector is equal to the current vector.
    /// </summary>
    /// <param name="other">The vector to compare with the current vector.</param>
    /// <returns>true if the specified vector is equal to the current vector; otherwise, false.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Vector3Int other) => this == other;

    /// <summary>
    /// Returns the hash code for this vector.
    /// </summary>
    /// <returns>A hash code for the current vector.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

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
    /// <param name="formatProvider">The format provider to use.</param>
    /// <returns>A string that represents the current vector.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
        return $"({X.ToString(format, formatProvider)}, {Y.ToString(format, formatProvider)}, {Z.ToString(format, formatProvider)})";
    }

    /// <summary>
    /// Gets a vector with all components set to zero.
    /// </summary>
    [Pure]
    public static Vector3Int Zero
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, 0, 0);

    /// <summary>
    /// Gets a vector with all components set to one.
    /// </summary>
    [Pure]
    public static Vector3Int One
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(1, 1, 1);

    /// <summary>
    /// Gets a vector pointing up (0, 1, 0).
    /// </summary>
    [Pure]
    public static Vector3Int Up
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, 1, 0);

    /// <summary>
    /// Gets a vector pointing down (0, -1, 0).
    /// </summary>
    [Pure]
    public static Vector3Int Down
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, -1, 0);

    /// <summary>
    /// Gets a vector pointing left (-1, 0, 0).
    /// </summary>
    [Pure]
    public static Vector3Int Left
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(-1, 0, 0);

    /// <summary>
    /// Gets a vector pointing right (1, 0, 0).
    /// </summary>
    [Pure]
    public static Vector3Int Right
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(1, 0, 0);

    /// <summary>
    /// Gets a vector pointing forward (0, 0, 1).
    /// </summary>
    [Pure]
    public static Vector3Int Forward
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, 0, 1);

    /// <summary>
    /// Gets a vector pointing backward (0, 0, -1).
    /// </summary>
    [Pure]
    public static Vector3Int Back
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    } = new(0, 0, -1);
}
