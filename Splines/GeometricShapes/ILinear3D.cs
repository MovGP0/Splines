using System.Numerics;

namespace Splines.GeometricShapes;

/// <summary>A shared interface between Ray3D, Line3D and LineSegment3D</summary>
public interface ILinear3D
{
    /// <summary>The origin of this linear 3D object (Ray3D, Line3D or LineSegment3D)</summary>
    Vector3 Origin { get; }

    /// <summary>The direction of this linear 3D object (Ray3D, Line3D or LineSegment3D)</summary>
    /// <remarks>This vector may or may not be normalized</remarks>
    Vector3 Direction { get; }

    /// <summary>Returns whether this t-value is within this linear 3D object (Ray3D, Line3D or LineSegment3D)</summary>
    /// <param name="t">The t-value along the linear 3D object</param>
    bool IsValidTValue(float t);

    /// <summary>Clamps the value into the range of this linear 3D object (Ray3D, Line3D or LineSegment3D)</summary>
    /// <param name="t">The t-value along the linear 3D object</param>
    float ClampTValue(float t);
}
