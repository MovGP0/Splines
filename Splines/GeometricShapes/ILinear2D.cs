using System.Numerics;

namespace Splines.GeometricShapes;

/// <summary>A shared interface between Ray2D, Line2D and LineSegment2D</summary>
public interface ILinear2D
{
    /// <summary>The origin of this linear 2D object (Ray2D, Line2D or LineSegment2D)</summary>
    Vector2 Origin { get; }

    /// <summary>The direction of this linear 2D object (Ray2D, Line2D or LineSegment2D)</summary>
    /// <remarks>This vector may or may not be normalized</remarks>
    Vector2 Direction { get; }

    /// <summary>Returns whether this t-value is within this linear 2D object (Ray2D, Line2D or LineSegment2D)</summary>
    /// <param name="t">The t-value along the linear 2D object</param>
    bool IsValidTValue(float t);

    /// <summary>Clamps the value into the range of this linear 2D object (Ray2D, Line2D or LineSegment2D)</summary>
    /// <param name="t">The t-value along the linear 2D object</param>
    float ClampTValue(float t);
}
