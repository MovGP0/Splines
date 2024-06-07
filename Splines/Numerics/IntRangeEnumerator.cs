using System.Collections;
using System.Collections.Generic;

namespace Splines.Numerics;

/// <summary>
/// Enumerator for the <see cref="IntRange"/> struct.
/// </summary>
public struct IntRangeEnumerator: IEnumerator<int>
{
    private IntRange _range;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntRangeEnumerator"/> struct with the specified range.
    /// </summary>
    /// <param name="range">The range to enumerate.</param>
    public IntRangeEnumerator(IntRange range) => (_range, Current) = (range, range.Start - 1);

    /// <summary>
    /// Advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
    public bool MoveNext() => ++Current <= _range.Last;

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator.
    /// </summary>
    public int Current
    {
        [Pure]
        get;
        private set;
    }

    /// <summary>
    /// Sets the enumerator to its initial position, which is before the first element in the collection.
    /// </summary>
    public void Reset() => Current = _range.Start - 1;

    /// <summary>
    /// Gets the element in the collection at the current position of the enumerator (non-generic).
    /// </summary>
    [Pure]
    object IEnumerator.Current => Current;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        // No resources to dispose
    }
}
