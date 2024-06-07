using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace Splines;

[DebuggerDisplay("{DebuggerDisplay}")]
internal sealed class OutlierPointInfo2D : OutlierPointInfo<Vector2>
{
    private string DebuggerDisplay
    {
        get
        {
            var px = Point.X.ToString("F3", CultureInfo.InvariantCulture);
            var py = Point.Y.ToString("F3", CultureInfo.InvariantCulture);
            return $"({px}, {py}) {IsOutlier}";
        }
    }

    public OutlierPointInfo2D(Vector2 point) : base(point)
    {
    }
}
