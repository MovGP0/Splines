using System.Diagnostics;
using System.Globalization;

namespace Splines;

[DebuggerDisplay("{DebuggerDisplay}")]
internal sealed class OutlierPointInfo1D : OutlierPointInfo<float>
{
    private string DebuggerDisplay
    {
        get
        {
            var px = Point.ToString("F3", CultureInfo.InvariantCulture);
            return $"({px}) {IsOutlier}";
        }
    }

    public OutlierPointInfo1D(float point) : base(point)
    {
    }
}
