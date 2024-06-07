using System.Globalization;
using System.Runtime.CompilerServices;

namespace Splines.GeometricShapes;

public partial struct Ray4D : IFormattable
{
    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() => ToString("F2", CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a string that represents the current object, using the specified format.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <returns>A string that represents the current object.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a string that represents the current object, using the specified format and format provider.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <returns>A string that represents the current object.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = "F2";
        }

        formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;

        return $"Origin: {Origin.ToString(format, formatProvider)}, Dir: {Direction.ToString(format, formatProvider)}";
    }
}
