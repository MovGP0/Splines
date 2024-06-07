using System.Linq;
using System.Runtime.CompilerServices;
using Splines.Unity;

namespace Splines;

public static partial class Statistics
{
    /// <summary>
    /// Calculates the mean (average) of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The mean of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Mean(this float[] values) => values.Average();

    /// <summary>
    /// Calculates the standard deviation of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The standard deviation of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float StandardDeviation(this float[] values) => Mathf.Sqrt(Variance(values));

    /// <summary>
    /// Calculates the variance of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The variance of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Variance(this float[] values)
    {
        float mean = Mean(values);
        return Variance(values, mean);
    }

    /// <summary>
    /// Calculates the variance of the values using the given mean.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <param name="mean">The mean of the values.</param>
    /// <returns>The variance of the values.</returns>
    [Pure]
    private static float Variance(float[] values, float mean)
    {
        return values
            .Select(v =>
            {
                var deltaFromMean = v - mean;
                return deltaFromMean * deltaFromMean;
            })
            .Average();
    }

    /// <summary>
    /// Calculates the mean and standard deviation of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>A tuple containing the mean and standard deviation.</returns>
    [Pure]
    public static (float mean, float stdDev) MeanAndStandardDeviation(this float[] values)
    {
        float mean = values.Average();
        float stdDev = Mathf.Sqrt(Variance(values, mean));
        return (mean, stdDev);
    }

    /// <summary>
    /// Calculates the range of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The range of the values.</returns>
    [Pure]
    public static float Range(this float[] values) => values.Max() - values.Min();

    /// <summary>
    /// Calculates the median of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The median of the values.</returns>
    [Pure]
    public static float Median(this float[] values)
    {
        float[] sortedValues = values.OrderBy(n => n).ToArray();
        int count = sortedValues.Length;

        if (count % 2 != 0)
        {
            // use the value at the middle position
            return sortedValues[count / 2];
        }

        // calculate the average of the value before and after the middle position
        var before = sortedValues[count / 2 - 1];
        var after = sortedValues[count / 2];
        return (before + after) / 2f;
    }

    /// <summary>
    /// Calculates the mode of the values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The mode of the values, or null if no mode exists.</returns>
    [Pure]
    public static float? Mode(this float[] values)
    {
        return values
            .GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .FirstOrDefault()?.Key;
    }

    /// <summary>
    /// Finds the minimum and maximum values in the array.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>A tuple containing the minimum and maximum values.</returns>
    [Pure]
    public static (float min, float max) MinMax(this float[] values)
    {
        if (values.Length == 0)
        {
            return (float.NaN, float.NaN);
        }

        float min = values[0];
        float max = values[0];

        foreach (var value in values)
        {
            if (value < min) min = value;
            if (value > max) max = value;
        }

        return (min, max);
    }

    /// <summary>
    /// Calculates the percentile of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <param name="percentile">The percentile to calculate (0-100).</param>
    /// <returns>The value at the given percentile.</returns>
    [Pure]
    public static float Percentile(this float[] values, float percentile)
    {
        if (percentile is < 0f or > 100f)
            throw new ArgumentOutOfRangeException(nameof(percentile), "Percentile must be between 0 and 100.");

        float[] sortedValues = values.OrderBy(n => n).ToArray();
        double index = (percentile / 100f) * (sortedValues.Length - 1);
        int lower = (int)Math.Floor(index);
        int upper = (int)Math.Ceiling(index);

        if (lower == upper)
        {
            return sortedValues[lower];
        }

        return sortedValues[lower] + (float)((index - lower) * (sortedValues[upper] - sortedValues[lower]));
    }

    /// <summary>
    /// Calculates the quartiles (Q1, Q2 (Median), Q3) of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>A tuple containing the first, second, and third quartiles.</returns>
    [Pure]
    public static (float Q1, float Q2, float Q3) Quartiles(this float[] values)
    {
        float Q1 = values.Percentile(25);
        float Q2 = values.Median();
        float Q3 = values.Percentile(75);
        return (Q1, Q2, Q3);
    }

    /// <summary>
    /// Calculates the interquartile range (IQR) of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The interquartile range of the values.</returns>
    [Pure]
    public static float InterquartileRange(this float[] values)
    {
        var (Q1, _, Q3) = values.Quartiles();
        return Q3 - Q1;
    }

    /// <summary>
    /// Calculates the skewness of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The skewness of the values.</returns>
    [Pure]
    public static float Skewness(this float[] values)
    {
        float mean = values.Mean();
        float n = values.Length;
        float m3 = values.Select(v => (v - mean) * (v - mean) * (v - mean)).Sum() / n;
        float m2 = values.Select(v => (v - mean) * (v - mean)).Sum() / n;
        return m3 / (float)Math.Pow(m2, 1.5);
    }

    /// <summary>
    /// Calculates the kurtosis of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The kurtosis of the values.</returns>
    /// <remarks>
    /// See <a href="https://en.wikipedia.org/wiki/Kurtosis">Wikipedia: Kurtosis</a> for details.
    /// </remarks>
    [Pure]
    public static float Kurtosis(this float[] values)
    {
        float mean = values.Mean();
        float n = values.Length;
        float m4 = values.Select(v => (v - mean) * (v - mean) * (v - mean) * (v - mean)).Sum() / n;
        float m2 = values.Select(v => (v - mean) * (v - mean)).Sum() / n;
        return m4 / (m2 * m2) - 3;
    }

    /// <summary>
    /// Calculates the geometric mean of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The geometric mean of the values.</returns>
    [Pure]
    public static float GeometricMean(this float[] values)
    {
        return (float)Math.Pow(values.Aggregate(1.0, (prod, val) => prod * val), 1.0 / values.Length);
    }

    /// <summary>
    /// Calculates the harmonic mean of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The harmonic mean of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float HarmonicMean(this float[] values)
        => values.Length / values.Sum(v => 1 / v);

    /// <summary>
    /// Calculates the coefficient of variation of the float values.
    /// </summary>
    /// <param name="values">The array of float values.</param>
    /// <returns>The coefficient of variation of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CoefficientOfVariation(this float[] values)
        => values.StandardDeviation() / values.Mean();

    /// <summary>
    /// Calculates the correlation between two arrays of float values.
    /// </summary>
    /// <param name="x">The first array of float values.</param>
    /// <param name="y">The second array of float values.</param>
    /// <returns>The correlation coefficient between the two arrays.</returns>
    [Pure]
    public static float Correlation(this float[] x, float[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException("Arrays must be of the same length.");

        float meanX = x.Mean();
        float meanY = y.Mean();
        float covariance = x.Zip(y, (xi, yi) => (xi - meanX) * (yi - meanY)).Sum() / x.Length;
        float stdDevX = x.StandardDeviation();
        float stdDevY = y.StandardDeviation();
        return covariance / (stdDevX * stdDevY);
    }
}
