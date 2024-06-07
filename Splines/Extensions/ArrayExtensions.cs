namespace Splines.Extensions;

internal static class ArrayExtensions
{
    [Pure]
    public static T[] Copy<T>(this T[] array)
    {
        T[] copy = new T[array.Length];
        Array.Copy(array, copy, array.Length);
        return copy;
    }

    [Pure]
    public static T[]? CopyNullable<T>(this T[]? array)
    {
        if (array is null)
        {
            return null;
        }

        return Copy(array);
    }
}
