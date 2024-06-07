using System.Numerics;
using Splines.Extensions;

namespace Splines.Numerics;

/// <summary>A 4x1 column matrix with Quaternion values</summary>
[Serializable]
public struct QuaternionMatrix4x1 : IEquatable<QuaternionMatrix4x1>
{
    /// <summary>The first element of the matrix.</summary>
    public Quaternion M0
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The second element of the matrix.</summary>
    public Quaternion M1
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The third element of the matrix.</summary>
    public Quaternion M2
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The fourth element of the matrix.</summary>
    public Quaternion M3
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Initializes a new instance of the <see cref="QuaternionMatrix4x1"/> struct.</summary>
    /// <param name="m0">The first element.</param>
    /// <param name="m1">The second element.</param>
    /// <param name="m2">The third element.</param>
    /// <param name="m3">The fourth element.</param>
    public QuaternionMatrix4x1(Quaternion m0, Quaternion m1, Quaternion m2, Quaternion m3) => (M0, M1, M2, M3) = (m0, m1, m2, m3);

    /// <summary>Gets or sets the value at the specified row.</summary>
    /// <param name="row">The row index (0-3).</param>
    /// <returns>The value at the specified row.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when the row index is not between 0 and 3.</exception>
    public Quaternion this[int row]
    {
        [Pure]
        get => row switch
        {
            0 => M0,
            1 => M1,
            2 => M2,
            3 => M3,
            _ => throw new IndexOutOfRangeException($"Matrix row index has to be from 0 to 3, got: {row}")
        };
        set
        {
            switch (row)
            {
                case 0: M0 = value; break;
                case 1: M1 = value; break;
                case 2: M2 = value; break;
                case 3: M3 = value; break;
                default: throw new IndexOutOfRangeException($"Matrix row index has to be from 0 to 3, got: {row}");
            }
        }
    }

    /// <summary>Linearly interpolates between two matrices, based on a value <c>t</c>.</summary>
    /// <param name="a">The first matrix.</param>
    /// <param name="b">The second matrix.</param>
    /// <param name="t">The value to blend by.</param>
    /// <returns>A new <see cref="QuaternionMatrix4x1"/> that is the result of the interpolation.</returns>
    [Pure]
    public static QuaternionMatrix4x1 Slerp(QuaternionMatrix4x1 a, QuaternionMatrix4x1 b, float t)
        => new(a.M0.SlerpUnclamped(b.M0, t), a.M1.SlerpUnclamped(b.M1, t), a.M2.SlerpUnclamped(b.M2, t), a.M3.SlerpUnclamped(b.M3, t));

    /// <summary>Determines whether two matrices are equal.</summary>
    /// <param name="a">The first matrix to compare.</param>
    /// <param name="b">The second matrix to compare.</param>
    /// <returns><c>true</c> if the matrices are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(QuaternionMatrix4x1 a, QuaternionMatrix4x1 b) => a.M0 == b.M0 && a.M1 == b.M1 && a.M2 == b.M2 && a.M3 == b.M3;

    /// <summary>Determines whether two matrices are not equal.</summary>
    /// <param name="a">The first matrix to compare.</param>
    /// <param name="b">The second matrix to compare.</param>
    /// <returns><c>true</c> if the matrices are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(QuaternionMatrix4x1 a, QuaternionMatrix4x1 b) => !(a == b);

    /// <summary>Determines whether the specified matrix is equal to the current matrix.</summary>
    /// <param name="other">The matrix to compare with the current matrix.</param>
    /// <returns><c>true</c> if the specified matrix is equal to the current matrix; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(QuaternionMatrix4x1 other) => M0.Equals(other.M0) && M1.Equals(other.M1) && M2.Equals(other.M2) && M3.Equals(other.M3);

    /// <summary>Determines whether the specified object is equal to the current matrix.</summary>
    /// <param name="obj">The object to compare with the current matrix.</param>
    /// <returns><c>true</c> if the specified object is equal to the current matrix; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is QuaternionMatrix4x1 other && Equals(other);

    /// <summary>Returns the hash code for the current matrix.</summary>
    /// <returns>The hash code for the current matrix.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(M0, M1, M2, M3);

    /// <summary>Returns a string representation of the current matrix.</summary>
    /// <returns>A string representation of the current matrix.</returns>
    [Pure]
    public override string ToString() => $"[{M0}]\n[{M1}]\n[{M2}]\n[{M3}]";
}
