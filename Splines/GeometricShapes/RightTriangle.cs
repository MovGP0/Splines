using Splines.Extensions;

namespace Splines.GeometricShapes;

/// <summary>Helper functions for right angle triangles</summary>
public static class RightTriangle
{
    /// <summary>The area of a right angle triangle, given a base and a height</summary>
    /// <param name="base">The length of the base</param>
    /// <param name="height">The height of the triangle</param>
    public static float Area(float @base, float height) => @base * height * 0.5f;

    /// <summary>Returns the angle, given the length of the opposite edge and the hypotenuse</summary>
    /// <param name="opposite">The length of the edge opposite to this angle</param>
    /// <param name="hypotenuse">The length of the hypotenuse</param>
    public static float AngleFromOppositeHypotenuse(float opposite, float hypotenuse) => Mathfs.Asin((opposite / hypotenuse).ClampNeg1to1());

    /// <summary>Returns the angle, given the length of the adjacent edge and the hypotenuse</summary>
    /// <param name="adjacent">The length of the edge adjacent to this angle</param>
    /// <param name="hypotenuse">The length of the hypotenuse</param>
    public static float AngleFromAdjacentHypotenuse(float adjacent, float hypotenuse) => Mathfs.Acos((adjacent / hypotenuse).ClampNeg1to1());

    /// <summary>Returns the angle, given the length of the opposite edge and the adjacent edge</summary>
    /// <param name="opposite">The length of the edge opposite to this angle</param>
    /// <param name="adjacent">The length of the edge adjacent to this angle</param>
    public static float AngleFromOppositeAdjacent(float opposite, float adjacent) => Mathfs.Atan(opposite / adjacent);

    /// <summary>Returns the length of the hypotenuse, given an angle and its adjacent edge length</summary>
    /// <param name="angle">The angle between the hypotenuse and its adjacent edge</param>
    /// <param name="adjacent">The length of the adjacent edge</param>
    public static float HypotenuseFromAngleAdjacent(float angle, float adjacent) => adjacent / Mathfs.Cos(angle);

    /// <summary>Returns the length of the hypotenuse, given an angle and its opposite edge length</summary>
    /// <param name="angle">The angle between the hypotenuse and its adjacent edge</param>
    /// <param name="opposite">The length of the opposite edge</param>
    public static float HypotenuseFromAngleOpposite(float angle, float opposite) => opposite / Mathfs.Sin(angle);

    /// <summary>Returns the length of the hypotenuse, given the length of the two other sides</summary>
    /// <param name="opposite">The length of the opposite edge</param>
    /// <param name="adjacent">The length of the adjacent edge</param>
    public static float HypotenuseFromOppositeAdjacent(float opposite, float adjacent) => Mathfs.Sqrt(adjacent.Square() + opposite.Square());

    /// <summary>Returns the length of the adjecent edge, given an angle and its opposite edge length</summary>
    /// <param name="angle">The angle between the hypotenuse and its adjacent edge</param>
    /// <param name="opposite">The length of the opposite edge</param>
    public static float AdjacentFromAngleOpposite(float angle, float opposite) => opposite / Mathfs.Tan(angle);

    /// <summary>Returns the length of the adjecent edge, given an angle and the hypotenuse</summary>
    /// <param name="angle">The angle between the hypotenuse and its adjacent edge</param>
    /// <param name="hypotenuse">The length of the hypotenuse</param>
    public static float AdjacentFromAngleHypotenuse(float angle, float hypotenuse) => Mathfs.Cos(angle) * hypotenuse;

    /// <summary>Returns the length of the adjecent edge, given the opposite edge and the hypotenuse</summary>
    /// <param name="opposite">The length of the opposite edge</param>
    /// <param name="hypotenuse">The length of the hypotenuse</param>
    public static float AdjacentFromOppositeHypotenuse(float opposite, float hypotenuse) => Mathfs.Sqrt(hypotenuse.Square() - opposite.Square());

    /// <summary>Returns the length of the opposite edge, given an angle and its adjacent edge length</summary>
    /// <param name="angle">The angle between the hypotenuse and its adjacent edge</param>
    /// <param name="adjacent">The length of the adjacent edge</param>
    public static float OppositeFromAngleAdjacent(float angle, float adjacent) => Mathfs.Tan(angle) * adjacent;

    /// <summary>Returns the length of the opposite edge, given an angle and the hypotenuse</summary>
    /// <param name="angle">The angle between the hypotenuse and its adjacent edge</param>
    /// <param name="hypotenuse">The length of the hypotenuse</param>
    public static float OppositeFromAngleHypotenuse(float angle, float hypotenuse) => Mathfs.Sin(angle) * hypotenuse;

    /// <summary>Returns the length of the opposite edge, given the adjacent edge and the hypotenuse</summary>
    /// <param name="adjacent">The length of the adjacent edge</param>
    /// <param name="hypotenuse">The length of the hypotenuse</param>
    public static float OppositeFromAdjacentHypotenuse(float adjacent, float hypotenuse) => Mathfs.Sqrt(hypotenuse.Square() - adjacent.Square());
}
