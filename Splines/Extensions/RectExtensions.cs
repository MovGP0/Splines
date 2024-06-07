namespace Splines.Extensions;

internal static class RectExtensions
{
    public static Point Center(this Rect rect) => new(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
}
