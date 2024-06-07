using System.Numerics;

namespace Splines.Extensions;

public static class Vector3Helper
{
    /// <summary>
    /// Right direction unit vector (1, 0, 0)
    /// </summary>
    [Pure]
    public static Vector3 Right => new(1, 0, 0);

    /// <summary>
    /// Up direction unit vector (0, 1, 0)
    /// </summary>
    [Pure]
    public static Vector3 Up => new(0, 1, 0);

    /// <summary>
    /// Forward direction unit vector (0, 0, -1) in a right-handed coordinate system
    /// </summary>
    [Pure]
    public static Vector3 Forward => new(0, 0, -1);
}
