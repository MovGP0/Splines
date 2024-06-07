namespace Splines.Splines;

public static partial class CatRomTypeExtensions
{
    /// <summary>Returns the alpha value of a given catmull-rom type</summary>
    /// <param name="catRom">The type to get the alpha value from</param>
    public static float AlphaValue(this CatRomType catRom)
    {
        return catRom switch
        {
            CatRomType.Centripetal => 0.5f,
            CatRomType.Chordal => 1f,
            _ => 0f
        };
    }
}
