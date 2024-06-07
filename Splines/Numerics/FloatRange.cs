using System.Numerics;
using Splines.Unity;

namespace Splines.Numerics;

/// <summary>
/// Represents a range of floating-point values with a defined start and end.
/// Provides methods for creating, manipulating, and comparing ranges.
/// </summary>
[Serializable]
public struct FloatRange
{
    /// <summary>The unit interval of 0 to 1</summary>
    public static readonly FloatRange Unit = new(0, 1);

    /// <summary>The start of this range</summary>
    public float Start
    {
        [Pure]
        get;
        set;
    }

    /// <summary>The end of this range</summary>
    public float End
    {
        [Pure]
        get;
        set;
    }

    /// <summary>Creates a new value range</summary>
    /// <param name="start">The start of the range</param>
    /// <param name="end">The end of the range</param>
    public FloatRange(float start, float end) => (Start, End) = (start, end);

    /// <summary>The value at the center of this value range</summary>
    [Pure]
    public float Center => (Start + End) / 2;

    /// <summary>The length/span of this value range</summary>
    [Pure]
    public float Length => Mathf.Abs(End - Start);

    /// <summary>The minimum value of this range</summary>
    [Pure]
    public float Min => Mathf.Min(Start, End);

    /// <summary>The maximum value of this range</summary>
    [Pure]
    public float Max => Mathf.Max(Start, End);

    /// <summary>The direction of this value range. Returns -1 if <c>b</c> is greater than <c>a</c>, otherwise returns 1</summary>
    [Pure]
    public int Direction => End > Start ? 1 : -1;

    /// <summary>Interpolates a value from <c>a</c> to <c>b</c>, based on a parameter <c>t</c></summary>
    /// <param name="t">The normalized interpolant from <c>a</c> to <c>b</c>. A value of 0 returns <c>a</c>, a value of 1 returns <c>b</c></param>
    [Pure]
    public float Lerp(float t) => Mathfs.Lerp(Start, End, t);

    /// <summary>Returns the normalized position of the input value <c>v</c> within this range</summary>
    /// <param name="v">The value to get the normalized position of</param>
    [Pure]
    public float InverseLerp(float v) => Mathfs.InverseLerp(Start, End, v);

    /// <summary>Returns whether this range contains the value <c>v</c> (inclusive)</summary>
    /// <param name="v">The value to see if it's inside</param>
    [Pure]
    public bool Contains(float v) => v >= Mathf.Min(Start, End) && v <= Mathf.Max(Start, End);

    /// <summary>Returns whether this range contains the range <c>r</c></summary>
    /// <param name="r">The range to see if it's inside</param>
    [Pure]
    public bool Contains(FloatRange r) => r.Min >= Min && r.Max <= Max;

    /// <summary>Remaps the input value from the <c>input</c> range to the <c>output</c> range</summary>
    /// <param name="value">The value to remap</param>
    /// <param name="input">The input range</param>
    /// <param name="output">The output range</param>
    [Pure]
    public static float Remap(float value, FloatRange input, FloatRange output) => output.Lerp(input.InverseLerp(value));

    /// <summary>Remaps a range from the <c>input</c> range to the <c>output</c> range</summary>
    /// <param name="value">The range to remap</param>
    /// <param name="input">The input range</param>
    /// <param name="output">The output range</param>
    [Pure]
    public static FloatRange Remap(FloatRange value, FloatRange input, FloatRange output)
        => new(Remap(value.Start, input, output), Remap(value.End, input, output));

    /// <summary>Returns whether this range overlaps another range</summary>
    /// <param name="other">The other range to test overlap with</param>
    [Pure]
    public bool Overlaps(FloatRange other)
    {
         float separation = Mathf.Abs(other.Center - Center);
         float rTotal = (other.Length + Length) / 2;
         return separation < rTotal;
    }

    /// <summary>Wraps/repeats the input value to stay within this range</summary>
    /// <param name="value">The value to wrap/repeat in this interval</param>
    [Pure]
    public float Wrap(float value)
    {
         if (value >= Start && value <= End)
              return value;

         return Start + Mathfs.Repeat(value - Start, End - Start);
    }

    /// <summary>Clamps the input value to this range</summary>
    /// <param name="value">The value to clamp to this interval</param>
    [Pure]
    public float Clamp(float value) => Mathfs.Clamp(value, Min, Max);

    /// <summary>Expands the minimum or maximum value to contain the given <c>value</c></summary>
    /// <param name="value">The value to include</param>
    [Pure]
    public FloatRange Encapsulate(float value)
    {
        return Direction switch
        {
            1 => (Mathfs.Min(Start, value), Mathfs.Max(End, value)), // forward - a is min, b is max
            _ => (Mathfs.Min(End, value), Mathfs.Max(Start, value)) // reversed - b is min, a is max
        };
    }

    /// <summary>Expands the minimum or maximum value to contain the given <c>range</c></summary>
    /// <param name="range">The value range to include</param>
    [Pure]
    public FloatRange Encapsulate(FloatRange range)
    {
        return Direction switch
        {
            1 => (Mathfs.Min(Start, range.Start), Mathfs.Max(End, range.End)), // forward - a is min, b is max
            _ => (Mathfs.Min(End, range.End), Mathfs.Max(Start, range.Start)) // reversed - b is min, a is max
        };
    }

    /// <summary>Returns a version of this range, scaled around its start value</summary>
    /// <param name="scale">The value to scale the range by</param>
    [Pure]
    public FloatRange ScaleFromStart(float scale) => new(Start, Start + scale * (End - Start));

    /// <summary>Returns this range mirrored around a given value</summary>
    /// <param name="pivot">The value to mirror around</param>
    [Pure]
    public FloatRange MirrorAround(float pivot) => new(2 * pivot - Start, 2 * pivot - End);

    /// <summary>Returns a reversed version of this range, where a and b is swapped</summary>
    [Pure]
    public FloatRange Reverse() => (End, Start);

    /// <summary>Returns the rectangle encapsulating the region defined by a range per axis. Note: The direction of each range is ignored</summary>
    /// <param name="rangeX">The range of the X axis</param>
    /// <param name="rangeY">The range of the Y axis</param>
    [Pure]
    public static Rect ToRect(FloatRange rangeX, FloatRange rangeY) => new(rangeX.Min, rangeY.Min, rangeX.Length, rangeY.Length);

    /// <summary>Returns the bounding box encapsulating the region defined by a range per axis. Note: The direction of each range is ignored</summary>
    /// <param name="rangeX">The range of the X axis</param>
    /// <param name="rangeY">The range of the Y axis</param>
    /// <param name="rangeZ">The range of the Z axis</param>
    [Pure]
    public static Bounds ToBounds(FloatRange rangeX, FloatRange rangeY, FloatRange rangeZ)
    {
        Vector3 center = new(rangeX.Center, rangeY.Center, rangeZ.Center);
        Vector3 size = new(rangeX.Length, rangeY.Length, rangeZ.Length);
        return new Bounds(center, size);
    }

    /// <summary>
    /// Subtracts a value from both the start and end of the range.
    /// </summary>
    /// <param name="range">The range to be modified.</param>
    /// <param name="v">The value to subtract from the range.</param>
    /// <returns>A new <see cref="FloatRange"/> with the modified values.</returns>
    [Pure]
    public static FloatRange operator -(FloatRange range, float v) => new(range.Start - v, range.End - v);

    /// <summary>
    /// Adds a value to both the start and end of the range.
    /// </summary>
    /// <param name="range">The range to be modified.</param>
    /// <param name="v">The value to add to the range.</param>
    /// <returns>A new <see cref="FloatRange"/> with the modified values.</returns>
    [Pure]
    public static FloatRange operator +(FloatRange range, float v) => new(range.Start + v, range.End + v);

    /// <summary>
    /// Divides both the start and end of the range by an integer value.
    /// </summary>
    /// <param name="range">The range to be modified.</param>
    /// <param name="v">The value to divide the range by.</param>
    /// <returns>A new <see cref="FloatRange"/> with the modified values.</returns>
    [Pure]
    public static FloatRange operator /(FloatRange range, int v) => new(range.Start / v, range.End / v);

    /// <summary>
    /// Divides both the start and end of the range by a floating-point value.
    /// </summary>
    /// <param name="range">The range to be modified.</param>
    /// <param name="v">The value to divide the range by.</param>
    /// <returns>A new <see cref="FloatRange"/> with the modified values.</returns>
    [Pure]
    public static FloatRange operator /(FloatRange range, float v) => new(range.Start / v, range.End / v);

    /// <summary>
    /// Multiplies both the start and end of the range by an integer value.
    /// </summary>
    /// <param name="range">The range to be modified.</param>
    /// <param name="v">The value to multiply the range by.</param>
    /// <returns>A new <see cref="FloatRange"/> with the modified values.</returns>
    [Pure]
    public static FloatRange operator *(FloatRange range, int v) => new(range.Start * v, range.End * v);

    /// <summary>
    /// Multiplies both the start and end of the range by a floating-point value.
    /// </summary>
    /// <param name="range">The range to be modified.</param>
    /// <param name="v">The value to multiply the range by.</param>
    /// <returns>A new <see cref="FloatRange"/> with the modified values.</returns>
    [Pure]
    public static FloatRange operator *(FloatRange range, float v) => new(range.Start * v, range.End * v);

    /// <summary>
    /// Implicitly converts a tuple of two floats to a <see cref="FloatRange"/>.
    /// </summary>
    /// <param name="tuple">The tuple to convert.</param>
    /// <returns>A new <see cref="FloatRange"/> with the specified start and end values.</returns>
    public static implicit operator FloatRange((float a, float b) tuple) => new(tuple.a, tuple.b);

    /// <summary>
    /// Determines whether two <see cref="FloatRange"/> instances are equal.
    /// </summary>
    /// <param name="a">The first <see cref="FloatRange"/> to compare.</param>
    /// <param name="b">The second <see cref="FloatRange"/> to compare.</param>
    /// <returns><c>true</c> if the start and end values of both ranges are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(FloatRange a, FloatRange b) => a.Start == b.Start && a.End == b.End;

    /// <summary>
    /// Determines whether two <see cref="FloatRange"/> instances are not equal.
    /// </summary>
    /// <param name="a">The first <see cref="FloatRange"/> to compare.</param>
    /// <param name="b">The second <see cref="FloatRange"/> to compare.</param>
    /// <returns><c>true</c> if the start and end values of both ranges are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(FloatRange a, FloatRange b) => a.Start != b.Start || a.End != b.End;

    /// <summary>
    /// Determines whether the current <see cref="FloatRange"/> is equal to another <see cref="FloatRange"/>.
    /// </summary>
    /// <param name="other">The other <see cref="FloatRange"/> to compare with.</param>
    /// <returns><c>true</c> if the start and end values are equal; otherwise, <c>false</c>.</returns>
    public bool Equals(FloatRange other) => Start.Equals(other.Start) && End.Equals(other.End);

    /// <summary>
    /// Determines whether the current <see cref="FloatRange"/> is equal to a specified object.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if the object is a <see cref="FloatRange"/> and the start and end values are equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is FloatRange other && Equals(other);

    /// <summary>
    /// Returns the hash code for the current <see cref="FloatRange"/>.
    /// </summary>
    /// <returns>The hash code for the current <see cref="FloatRange"/>.</returns>
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <summary>
    /// Returns a string representation of the current <see cref="FloatRange"/>.
    /// </summary>
    /// <returns>A string representing the start and end values of the range.</returns>
    public override string ToString() => $"[{Start},{End}]";
}
