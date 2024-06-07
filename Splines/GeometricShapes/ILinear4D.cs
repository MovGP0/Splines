using System.Numerics;

namespace Splines.GeometricShapes;

/// <summary>A shared interface between Ray4D, Line4D and LineSegment4D</summary>
public interface ILinear4D
{
    /// <summary>The origin of this linear 4D object (Ray4D, Line4D or LineSegment4D)</summary>
    Vector4 Origin { get; }

    /// <summary>The direction of this linear 4D object (Ray4D, Line4D or LineSegment4D)</summary>
    /// <remarks>This vector may or may not be normalized</remarks>
    Vector4 Direction { get; }

    /// <summary>Returns whether this t-value is within this linear 4D object (Ray4D, Line4D or LineSegment4D)</summary>
    /// <param name="t">The t-value along the linear 4D object</param>
    bool IsValidTValue(float t);

    /// <summary>Clamps the value into the range of this linear 4D object (Ray4D, Line4D or LineSegment4D)</summary>
    /// <param name="t">The t-value along the linear 4D object</param>
    float ClampTValue(float t);
}
