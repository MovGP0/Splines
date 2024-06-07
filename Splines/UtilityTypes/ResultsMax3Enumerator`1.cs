using System.Collections;
using System.Collections.Generic;

namespace Splines.UtilityTypes;

public struct ResultsMax3Enumerator<T> : IEnumerator<T> where T : struct
{
    int currentIndex;
    readonly ResultsMax3<T> value;

    public ResultsMax3Enumerator(ResultsMax3<T> value) {
        this.value = value;
        currentIndex = -1;
    }

    public bool MoveNext() => ++currentIndex < value.count;
    public void Reset() => currentIndex = -1;
    public T Current => value[currentIndex];
    object IEnumerator.Current => Current;
    public void Dispose() => _ = 0;
}
