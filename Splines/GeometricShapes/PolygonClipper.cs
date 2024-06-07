using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Splines.Extensions;
using Splines.Numerics;

namespace Splines.GeometricShapes;

/// <summary>Utility type to clip polygons</summary>
public static class PolygonClipper
{
    /// <summary>
    /// Clips the given polygon with the specified line.
    /// </summary>
    /// <param name="poly">The polygon to be clipped.</param>
    /// <param name="line">The line used for clipping the polygon.</param>
    /// <param name="clippedPolygons">The resulting clipped polygons, if any.</param>
    /// <returns>The result state of the clipping operation.</returns>
    public static ResultState Clip(Polygon poly, Line2D line, out List<Polygon> clippedPolygons)
    {
        List<PointSideState> states = DeterminePointSideStates(poly, line, out bool hasDiscards, out int startIndex);

        if (!hasDiscards)
        {
            clippedPolygons = new();
            return ResultState.OriginalLeftIntact;
        }

        if (startIndex == -1)
        {
            clippedPolygons = new();
            return ResultState.FullyDiscarded;
        }

        // find keep points, spread outwards until it's cut off from the rest
        SortedSet<PolygonSection> sections = ExtractKeepSections(poly, line, states);

        // combine all clipped polygonal regions
        clippedPolygons = CombineSectionsIntoPolygons(sections).ToList();

        return ResultState.Clipped;
    }

    /// <summary>
    /// Combines the extracted sections into a list of polygons.
    /// </summary>
    private static IEnumerable<Polygon> CombineSectionsIntoPolygons(SortedSet<PolygonSection> sections)
    {
        while (sections.Count > 0)
        {
            // find solid polygon
            PolygonSection solid = sections.First();
            sections.Remove(solid);
            int solidDir = solid.tRange.Direction;

            // find holes in that polygon
            float referencePoint = solid.tRange.Min;
            while (true) // should break early anyway
            {
                FloatRange checkRange = new FloatRange(referencePoint, solid.tRange.Max);
                PolygonSection? hole = sections.FirstOrDefault(s => s.tRange.Direction != solidDir && checkRange.Contains(s.tRange));
                if (hole == null)
                {
                    // nothing inside - we're done with this solid
                    yield return new Polygon(solid.Points);
                    break;
                }

                // append the hole polygon to the solid points
                sections.Remove(hole);
                AppendHoleToSolid(solid, hole, solidDir);

                // skip everything inside the hole by shifting forward
                referencePoint = hole.tRange.Max;
            }
        }
    }

    /// <summary>
    /// Appends a hole polygon section to a solid polygon section.
    /// </summary>
    private static void AppendHoleToSolid(PolygonSection solid, PolygonSection hole, int solidDir)
    {
        switch (solidDir)
        {
            case 1:
                solid.Points.InsertRange(0, hole.Points);
                break;
            default:
                solid.Points.AddRange(hole.Points);
                break;
        }
    }

    /// <summary>
    /// Determines the side states of points in the polygon relative to the clipping line.
    /// </summary>
    private static List<PointSideState> DeterminePointSideStates(Polygon poly, Line2D line, out bool hasDiscards, out int startIndex)
    {
        List<PointSideState> states = new();
        hasDiscards = false;
        startIndex = -1;

        for (int i = 0; i < poly.Count; i++)
        {
            float sd = line.SignedDistance(poly[i]);
            if (Mathfs.Approximately(sd, 0))
            {
                states.Add(PointSideState.Edge);
            }
            else if (sd > 0)
            {
                if (startIndex == -1)
                {
                    startIndex = i;
                }

                states.Add(PointSideState.Keep);
            }
            else
            {
                hasDiscards = true;
                states.Add(PointSideState.Discard);
            }
        }

        return states;
    }

    /// <summary>
    /// Extracts the sections of the polygon that are on the "keep" side of the clipping line.
    /// </summary>
    private static SortedSet<PolygonSection> ExtractKeepSections(Polygon poly, Line2D line, List<PointSideState> states)
    {
        SortedSet<PolygonSection> sections = new();
        for (int i = 0; i < poly.Count; i++)
        {
            if (states[i] == PointSideState.Keep)
            {
                sections.Add(ExtractPolygonSection(poly, line, i, states));
            }
        }

        return sections;
    }

    /// <summary>
    /// Extracts a section of the polygon that is on the "keep" side of the clipping line starting from the specified index.
    /// </summary>
    /// <param name="poly">The polygon to extract a section from.</param>
    /// <param name="line">The clipping line used to determine the section.</param>
    /// <param name="sourceIndex">The index in the polygon to start the extraction from.</param>
    /// <param name="states"></param>
    /// <returns>A section of the polygon that is on the "keep" side of the clipping line.</returns>
    private static PolygonSection ExtractPolygonSection(Polygon poly, Line2D line, int sourceIndex, List<PointSideState> states)
    {
        List<Vector2> points = new List<Vector2>();
        AddFront(sourceIndex);

        (float tStart, float tEnd) = FindSectionEndpoints(
            poly,
            line,
            sourceIndex,
            states,
            AddFront,
            AddBack,
            points);

        return new PolygonSection((tStart, tEnd), points);

        void AddBack(int i)
        {
            states[i.Mod(states.Count)] = PointSideState.Handled;
            points.Insert(0, poly[i]);
        }

        void AddFront(int i)
        {
            states[i.Mod(states.Count)] = PointSideState.Handled;
            points.Add(poly[i]);
        }
    }

    /// <summary>
    /// Finds the endpoints of the polygon section on the "keep" side of the clipping line.
    /// </summary>
    private static (float tStart, float tEnd) FindSectionEndpoints(
        Polygon poly,
        Line2D line,
        int sourceIndex,
        List<PointSideState> states,
        Action<int> addFront,
        Action<int> addBack,
        IList<Vector2> points)
    {
        float tStart = 0, tEnd = 0;

        for (int dir = -1; dir <= 1; dir += 2)
        {
            for (int i = 1; i < poly.Count; i++)
            {
                int index = sourceIndex + dir * i;
                if (states[index.Mod(states.Count)] is PointSideState.Discard or PointSideState.Edge)
                {
                    Line2D edge = new Line2D(poly[index - dir], poly[index] - poly[index - dir]);
                    if (IntersectionTest.LinearTValues(line, edge, out float tLine, out float tEdge))
                    {
                        Vector2 intPt = edge.GetPoint(tEdge);
                        switch (dir)
                        {
                            case 1:
                                tEnd = tLine;
                                points.Add(intPt);
                                break;
                            default:
                                tStart = tLine;
                                points.Insert(0, intPt);
                                break;
                        }

                        break;
                    }

                    throw new Exception("Polygon clipping failed due to line intersection not working as expected. You may have duplicate points or a degenerate polygon in general");
                }

                switch (dir)
                {
                    case 1:
                        addFront(index);
                        break;
                    default:
                        addBack(index);
                        break;
                }
            }
        }

        return (tStart, tEnd);
    }
}
