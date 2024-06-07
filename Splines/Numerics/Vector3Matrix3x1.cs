using System.Numerics;
using Splines.Extensions;

namespace Splines.Numerics;

/// <summary>
/// A 3x1 column matrix with Vector3 values
/// </summary>
[Serializable]
public struct Vector3Matrix3x1 : IEquatable<Vector3Matrix3x1>
{
    /// <summary>The first element of the matrix.</summary>
    public Vector3 M0
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The second element of the matrix.</summary>
    public Vector3 M1
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The third element of the matrix.</summary>
    public Vector3 M2
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Matrix3x1"/> struct.
    /// </summary>
    /// <param name="m0">The first <see cref="Vector3"/> element.</param>
    /// <param name="m1">The second <see cref="Vector3"/> element.</param>
    /// <param name="m2">The third <see cref="Vector3"/> element.</param>
    public Vector3Matrix3x1(Vector3 m0, Vector3 m1, Vector3 m2)
        => (M0, M1, M2) = (m0, m1, m2);

    /// <summary>
    /// Initializes a new instance of the <see cref="Vector3Matrix3x1"/> struct from three <see cref="Matrix3x1"/> instances.
    /// </summary>
    /// <param name="x">The <see cref="Matrix3x1"/> for the X components.</param>
    /// <param name="y">The <see cref="Matrix3x1"/> for the Y components.</param>
    /// <param name="z">The <see cref="Matrix3x1"/> for the Z components.</param>
    public Vector3Matrix3x1(Matrix3x1 x, Matrix3x1 y, Matrix3x1 z)
        => (M0, M1, M2) = (new Vector3(x.M0, y.M0, z.M0), new Vector3(x.M1, y.M1, z.M1), new Vector3(x.M2, y.M2, z.M2));

    /// <summary>
    /// Gets or sets the <see cref="Vector3"/> at the specified row index.
    /// </summary>
    /// <param name="row">The row index.</param>
    /// <returns>The <see cref="Vector3"/> at the specified row index.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the row index is out of range (0-2).</exception>
    public Vector3 this[int row]
    {
        [Pure]
        get => row switch
        {
            0 => M0,
            1 => M1,
            2 => M2,
            _ => throw new IndexOutOfRangeException($"Matrix row index has to be from 0 to 2, got: {row}")
        };
        set
        {
            switch (row)
            {
                case 0: M0 = value; break;
                case 1: M1 = value; break;
                case 2: M2 = value; break;
                default: throw new IndexOutOfRangeException($"Matrix row index has to be from 0 to 2, got: {row}");
            }
        }
    }

    /// <summary>
    /// Gets the X components as a <see cref="Matrix3x1"/>.
    /// </summary>
    [Pure]
    public Matrix3x1 X => new(M0.X, M1.X, M2.X);

    /// <summary>
    /// Gets the Y components as a <see cref="Matrix3x1"/>.
    /// </summary>
    [Pure]
    public Matrix3x1 Y => new(M0.Y, M1.Y, M2.Y);

    /// <summary>
    /// Gets the Z components as a <see cref="Matrix3x1"/>.
    /// </summary>
    [Pure]
    public Matrix3x1 Z => new(M0.Z, M1.Z, M2.Z);

    /// <summary>
    /// Linearly interpolates between two matrices, based on a value <c>t</c>.
    /// </summary>
    /// <param name="a">The first <see cref="Vector3Matrix3x1"/>.</param>
    /// <param name="b">The second <see cref="Vector3Matrix3x1"/>.</param>
    /// <param name="t">The value to blend by.</param>
    /// <returns>The interpolated <see cref="Vector3Matrix3x1"/>.</returns>
    [Pure]
    public static Vector3Matrix3x1 Lerp(Vector3Matrix3x1 a, Vector3Matrix3x1 b, float t)
        => new(a.M0.LerpUnclamped(b.M0, t), a.M1.LerpUnclamped(b.M1, t), a.M2.LerpUnclamped(b.M2, t));

    /// <summary>
    /// Determines whether two specified instances of <see cref="Vector3Matrix3x1"/> are equal.
    /// </summary>
    /// <param name="a">The first <see cref="Vector3Matrix3x1"/> to compare.</param>
    /// <param name="b">The second <see cref="Vector3Matrix3x1"/> to compare.</param>
    /// <returns><c>true</c> if the two instances are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Vector3Matrix3x1 a, Vector3Matrix3x1 b)
        => a.M0 == b.M0 && a.M1 == b.M1 && a.M2 == b.M2;

    /// <summary>
    /// Determines whether two specified instances of <see cref="Vector3Matrix3x1"/> are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="Vector3Matrix3x1"/> to compare.</param>
    /// <param name="b">The second <see cref="Vector3Matrix3x1"/> to compare.</param>
    /// <returns><c>true</c> if the two instances are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Vector3Matrix3x1 a, Vector3Matrix3x1 b)
        => !(a == b);

    /// <summary>
    /// Indicates whether the current matrix is equal to another matrix of the same type.
    /// </summary>
    /// <param name="other">A matrix to compare with this matrix.</param>
    /// <returns><c>true</c> if the current matrix is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Vector3Matrix3x1 other)
        => M0.Equals(other.M0) && M1.Equals(other.M1) && M2.Equals(other.M2);

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj)
        => obj is Vector3Matrix3x1 other && Equals(other);

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    [Pure]
    public override int GetHashCode()
        => HashCode.Combine(M0, M1, M2);

    /// <summary>
    /// Returns a string that represents the current matrix.
    /// </summary>
    /// <returns>A string that represents the current matrix.</returns>
    [Pure]
    public override string ToString()
        => $"[{M0}]\n[{M1}]\n[{M2}]";
}
