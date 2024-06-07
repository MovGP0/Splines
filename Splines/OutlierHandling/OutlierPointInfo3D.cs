using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace Splines;

[DebuggerDisplay("{DebuggerDisplay}")]
internal sealed class OutlierPointInfo3D : OutlierPointInfo<Vector3>
{
    private string DebuggerDisplay
    {
        get
        {
            var px = Point.X.ToString("F3", CultureInfo.InvariantCulture);
            var py = Point.Y.ToString("F3", CultureInfo.InvariantCulture);
            var pz = Point.Z.ToString("F3", CultureInfo.InvariantCulture);
            return $"({px}, {py}, {pz}) {IsOutlier}";
        }
    }

    public OutlierPointInfo3D(Vector3 point) : base(point)
    {
    }
}
