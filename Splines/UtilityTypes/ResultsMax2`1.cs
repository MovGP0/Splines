using System.Collections;
using System.Collections.Generic;

namespace Splines.UtilityTypes;

/// <summary>Contains either 0, 1 or 2 valid return values</summary>
public readonly struct ResultsMax2<T> : IEnumerable<T> where T : struct
{
    /// <inheritdoc cref="ResultsMax3{T}.count"/>
    public readonly int count;

    /// <inheritdoc cref="ResultsMax3{T}.a"/>
    public readonly T a;

    /// <inheritdoc cref="ResultsMax3{T}.b"/>
    public readonly T b;

    /// <inheritdoc cref="ResultsMax3{T}(T,T)"/>
    public ResultsMax2(T a, T b) {
        (this.a, this.b) = (a, b);
        count = 2;
    }

    /// <inheritdoc cref="ResultsMax3{T}(T)"/>
    public ResultsMax2(T a) {
        (this.a, this.b) = (a, default);
        count = 1;
    }

    /// <inheritdoc cref="ResultsMax3{T}.this"/>
    public T this[ int i ] {
        get {
            if (i >= 0 && i < count)
            {
                switch (i)
                {
                    case 0: return a;
                    case 1: return b;
                }
            }

            throw new IndexOutOfRangeException();
        }
    }

    /// <inheritdoc cref="ResultsMax3{T}.Add(T)"/>
    public ResultsMax2<T> Add(T value)
    {
        switch (count)
        {
            case 0: return new ResultsMax2<T>(value);
            case 1: return new ResultsMax2<T>(a, value);
            default: throw new IndexOutOfRangeException("Can't add more than two values to ResultsMax2");
        }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public ResultsMax2Enumerator<T> GetEnumerator() => new(this);
}
