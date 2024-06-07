namespace Splines.GeometricShapes;

/// <summary>A shared interface between Ray1D, Line1D and LineSegment1D</summary>
public interface ILinear1D
{
    /// <summary>The origin of this linear 1D object (Ray1D, Line1D or LineSegment1D)</summary>
    float Origin { get; }

    /// <summary>The direction of this linear 1D object (Ray1D, Line1D or LineSegment1D)</summary>
    /// <remarks>This vector may or may not be normalized</remarks>
    float Direction { get; }

    /// <summary>Returns whether this t-value is within this linear 1D object (Ray1D, Line1D or LineSegment1D)</summary>
    /// <param name="t">The t-value along the linear 1D object</param>
    bool IsValidTValue(float t);

    /// <summary>Clamps the value into the range of this linear 1D object (Ray1D, Line1D or LineSegment1D)</summary>
    /// <param name="t">The t-value along the linear 1D object</param>
    float ClampTValue(float t);
}
