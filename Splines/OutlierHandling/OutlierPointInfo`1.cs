namespace Splines;

internal abstract class OutlierPointInfo<T> where T : struct
{
    protected OutlierPointInfo(T point)
    {
        Point = point;
    }

    public T Point { get; }

    /// <summary>
    /// The magnitude of the acceleration (|a|) of the point.
    /// </summary>
    public float AccelerationMagnitude { get; set; } = float.NaN;

    public bool IsOutlier { get; set; }
}
