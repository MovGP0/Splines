using System.Linq;
using System.Runtime.CompilerServices;

namespace Splines;

public static partial class Statistics
{
    /// <summary>
    /// Calculates the mean (average) of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The mean of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Mean(this double[] values) => values.Average();

    /// <summary>
    /// Calculates the standard deviation of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The standard deviation of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double StandardDeviation(this double[] values) => Math.Sqrt(Variance(values));

    /// <summary>
    /// Calculates the variance of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The variance of the values.</returns>
    [Pure]
    public static double Variance(this double[] values)
    {
        double mean = Mean(values);
        return Variance(values, mean);
    }

    /// <summary>
    /// Calculates the variance of the double values using the given mean.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <param name="mean">The mean of the values.</param>
    /// <returns>The variance of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Variance(double[] values, double mean)
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
    /// Calculates the mean and standard deviation of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>A tuple containing the mean and standard deviation.</returns>
    [Pure]
    public static (double mean, double stdDev) MeanAndStandardDeviation(this double[] values)
    {
        double mean = values.Average();
        double stdDev = Math.Sqrt(Variance(values, mean));
        return (mean, stdDev);
    }

    /// <summary>
    /// Calculates the range of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The range of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Range(this double[] values) => values.Max() - values.Min();

    /// <summary>
    /// Calculates the median of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The median of the values.</returns>
    [Pure]
    public static double Median(this double[] values)
    {
        double[] sortedValues = values.OrderBy(n => n).ToArray();
        int count = sortedValues.Length;

        if (count % 2 != 0)
        {
            // use the value at the middle position
            return sortedValues[count / 2];
        }

        // calculate the average of the value before and after the middle position
        var before = sortedValues[count / 2 - 1];
        var after = sortedValues[count / 2];
        return (before + after) / 2.0;
    }

    /// <summary>
    /// Calculates the mode of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The mode of the values, or null if no mode exists.</returns>
    [Pure]
    public static double? Mode(this double[] values)
    {
        return values
            .GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .FirstOrDefault()?.Key;
    }

    /// <summary>
    /// Finds the minimum and maximum values in the array of double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>A tuple containing the minimum and maximum values.</returns>
    [Pure]
    public static (double min, double max) MinMax(this double[] values)
    {
        if (values.Length == 0)
        {
            return (double.NaN, double.NaN);
        }

        double min = values[0];
        double max = values[0];

        foreach (var value in values)
        {
            if (value < min) min = value;
            if (value > max) max = value;
        }

        return (min, max);
    }

    /// <summary>
    /// Calculates the percentile of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <param name="percentile">The percentile to calculate (0-100).</param>
    /// <returns>The value at the given percentile.</returns>
    [Pure]
    public static double Percentile(this double[] values, double percentile)
    {
        if (percentile is < 0d or > 100d)
            throw new ArgumentOutOfRangeException(nameof(percentile), "Percentile must be between 0 and 100.");

        double[] sortedValues = values.OrderBy(n => n).ToArray();
        double index = percentile / 100d * (sortedValues.Length - 1);
        int lower = (int)Math.Floor(index);
        int upper = (int)Math.Ceiling(index);

        if (lower == upper)
        {
            return sortedValues[lower];
        }

        return sortedValues[lower] + (index - lower) * (sortedValues[upper] - sortedValues[lower]);
    }

    /// <summary>
    /// Calculates the quartiles (Q1, Q2 (Median), Q3) of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>A tuple containing the first, second, and third quartiles.</returns>
    [Pure]
    public static (double Q1, double Q2, double Q3) Quartiles(this double[] values)
    {
        double Q1 = values.Percentile(25);
        double Q2 = values.Median();
        double Q3 = values.Percentile(75);
        return (Q1, Q2, Q3);
    }

    /// <summary>
    /// Calculates the interquartile range (IQR) of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The interquartile range of the values.</returns>
    [Pure]
    public static double InterquartileRange(this double[] values)
    {
        var (Q1, _, Q3) = values.Quartiles();
        return Q3 - Q1;
    }

    /// <summary>
    /// Calculates the skewness of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The skewness of the values.</returns>
    [Pure]
    public static double Skewness(this double[] values)
    {
        double mean = values.Mean();
        double n = values.Length;
        double m3 = values.Select(v => (v - mean) * (v - mean) * (v - mean)).Sum() / n;
        double m2 = values.Select(v => (v - mean) * (v - mean)).Sum() / n;
        return m3 / Math.Pow(m2, 1.5);
    }

    /// <summary>
    /// Calculates the kurtosis of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The kurtosis of the values.</returns>
    /// <remarks>
    /// See <a href="https://en.wikipedia.org/wiki/Kurtosis">Wikipedia: Kurtosis</a> for details.
    /// </remarks>
    [Pure]
    public static double Kurtosis(this double[] values)
    {
        double mean = values.Mean();
        double n = values.Length;
        double m4 = values.Select(v => (v - mean) * (v - mean) * (v - mean) * (v - mean)).Sum() / n;
        double m2 = values.Select(v => (v - mean) * (v - mean)).Sum() / n;
        return m4 / (m2 * m2) - 3;
    }

    /// <summary>
    /// Calculates the geometric mean of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The geometric mean of the values.</returns>
    [Pure]
    public static double GeometricMean(this double[] values)
    {
        return Math.Pow(values.Aggregate(1.0, (prod, val) => prod * val), 1.0 / values.Length);
    }

    /// <summary>
    /// Calculates the harmonic mean of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The harmonic mean of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double HarmonicMean(this double[] values)
        => values.Length / values.Sum(v => 1 / v);

    /// <summary>
    /// Calculates the coefficient of variation of the double values.
    /// </summary>
    /// <param name="values">The array of double values.</param>
    /// <returns>The coefficient of variation of the values.</returns>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CoefficientOfVariation(this double[] values)
        => values.StandardDeviation() / values.Mean();

    /// <summary>
    /// Calculates the correlation between two arrays of double values.
    /// </summary>
    /// <param name="x">The first array of double values.</param>
    /// <param name="y">The second array of double values.</param>
    /// <returns>The correlation coefficient between the two arrays.</returns>
    [Pure]
    public static double Correlation(this double[] x, double[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException("Arrays must be of the same length.");

        double meanX = x.Mean();
        double meanY = y.Mean();
        double covariance = x.Zip(y, (xi, yi) => (xi - meanX) * (yi - meanY)).Sum() / x.Length;
        double stdDevX = x.StandardDeviation();
        double stdDevY = y.StandardDeviation();
        return covariance / (stdDevX * stdDevY);
    }
}
