using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace Splines;

[DebuggerDisplay("{DebuggerDisplay}")]
internal sealed class OutlierPointInfo4D : OutlierPointInfo<Vector4>
{
    private string DebuggerDisplay
    {
        get
        {
            var px = Point.X.ToString("F3", CultureInfo.InvariantCulture);
            var py = Point.Y.ToString("F3", CultureInfo.InvariantCulture);
            var pz = Point.Z.ToString("F3", CultureInfo.InvariantCulture);
            var pw = Point.W.ToString("F3", CultureInfo.InvariantCulture);
            return $"({px}, {py}, {pz}, {pw}) {IsOutlier}";
        }
    }

    public OutlierPointInfo4D(Vector4 point) : base(point)
    {
    }
}
