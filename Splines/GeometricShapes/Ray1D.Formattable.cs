using System.Globalization;

namespace Splines.GeometricShapes;

public partial struct Ray1D : IFormattable
{
    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    [Pure]
    public override string ToString() => ToString("F2", CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a string that represents the current object, using the specified format.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <returns>A string that represents the current object.</returns>
    [Pure]
    public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture.NumberFormat);

    /// <summary>
    /// Returns a string that represents the current object, using the specified format and format provider.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <param name="formatProvider">The format provider to use.</param>
    /// <returns>A string that represents the current object.</returns>
    [Pure]
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (string.IsNullOrEmpty(format))
        {
            format = "F2";
        }

        formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;

        return $"Origin: {Origin.ToString(format, formatProvider)}, Direction: {Direction.ToString(format, formatProvider)}";
    }
}
