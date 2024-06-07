using System.Text;

namespace Splines.Numerics;

/// <summary>An integer range</summary>
[Serializable]
public struct IntRange : IEquatable<IntRange>
{
    public static readonly IntRange Empty = new(0, 0);

    /// <summary>Gets or sets the start of the range.</summary>
    public int Start
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Gets or sets the count of the range.</summary>
    public int Count
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Gets the integer at the specified index.</summary>
    /// <param name="i">The index.</param>
    /// <returns>The integer at the specified index.</returns>
    public int this[int i] => Start + i;

    /// <summary>The last integer in the range</summary>
    [Pure]
    public int Last => Start + Count - 1;

    /// <summary>The distance from first to last integer. Equivalent to <c>Count - 1</c></summary>
    [Pure]
    public int Distance => Count - 1;

    /// <summary>Creates a new integer range, given a start integer and how many integers to include in total</summary>
    /// <param name="start">The first integer</param>
    /// <param name="count">How many integers to include in the full range</param>
    public IntRange(int start, int count)
    {
        Start = start;
        Count = count;
    }

    /// <summary>Whether this range contains a given value (inclusive)</summary>
    /// <param name="value">The value to check if it's inside, or equal to the start or end</param>
    /// <returns>True if the range contains the value, otherwise false.</returns>
    [Pure]
    public bool Contains(int value) => value >= Start && value <= Last;

    /// <summary>Returns a copy of this range, without the last element (i.e., count is reduced by 1)</summary>
    /// <returns>A new IntRange without the last element.</returns>
    [Pure]
    public IntRange WithoutLast() => new(Start, Count - 1);

    /// <summary>Creates an integer range from start to end (inclusive)</summary>
    /// <param name="first">The first integer</param>
    /// <param name="last">The last integer</param>
    /// <returns>A new IntRange from the first to the last integer.</returns>
    [Pure]
    public static IntRange FirstToLast(int first, int last) => new(first, last - first + 1);

    /// <summary>
    /// Threshold for displaying the full range of elements in the range.
    /// When the range contains more elements than this, the range is displayed as a range of the first and last element.
    /// </summary>
    private const int MaxElementsToShow = 10;

    /// <summary>Returns a string representation of the range.</summary>
    /// <returns>A string representing the range.</returns>
    public override string ToString()
    {
        StringBuilder builder = new();
        builder.Append("{ ");

        if (Count <= MaxElementsToShow)
        {
            int last = Last;
            for (int i = Start; i <= last; i++)
            {
                builder.Append(i);
                if (i != last)
                {
                    builder.Append(", ");
                }
            }
        }
        else
        {
            builder.Append(Start);
            builder.Append(", ..., ");
            builder.Append(Last);
        }

        builder.Append(" }");
        return builder.ToString();
    }

    /// <summary>Gets an enumerator for the range.</summary>
    /// <returns>An enumerator for the range.</returns>
    [Pure]
    public IntRangeEnumerator GetEnumerator() => new(this);

    /// <summary>Determines whether the specified object is equal to the current range.</summary>
    /// <param name="obj">The object to compare with the current range.</param>
    /// <returns>True if the specified object is equal to the current range, otherwise false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is IntRange other && Equals(other);

    /// <summary>Determines whether the specified range is equal to the current range.</summary>
    /// <param name="other">The range to compare with the current range.</param>
    /// <returns>True if the specified range is equal to the current range, otherwise false.</returns>
    [Pure]
    public bool Equals(IntRange other) => Start == other.Start && Count == other.Count;

    /// <summary>Returns the hash code for the current range.</summary>
    /// <returns>The hash code for the current range.</returns>
    [Pure]
    public override int GetHashCode() => HashCode.Combine(Start, Count);

    /// <summary>Determines whether two ranges are equal.</summary>
    /// <param name="a">The first range to compare.</param>
    /// <param name="b">The second range to compare.</param>
    /// <returns>True if the ranges are equal, otherwise false.</returns>
    [Pure]
    public static bool operator ==(IntRange a, IntRange b) => a.Equals(b);

    /// <summary>Determines whether two ranges are not equal.</summary>
    /// <param name="a">The first range to compare.</param>
    /// <param name="b">The second range to compare.</param>
    /// <returns>True if the ranges are not equal, otherwise false.</returns>
    [Pure]
    public static bool operator !=(IntRange a, IntRange b) => !a.Equals(b);
}
