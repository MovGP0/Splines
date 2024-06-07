using System.Runtime.CompilerServices;
using Splines.Curves;
using Splines.Numerics;

namespace Splines.Splines.NonUniformSplineSegments;

public sealed class NUHermiteCubic3D : IParamSplineSegment<Polynomial3D, Vector3Matrix4x1>
{
    /// <inheritdoc cref="NUCatRomCubic2D(Vector2Matrix4x1,Matrix4x1)"/>
    public NUHermiteCubic3D(Vector3Matrix4x1 pointMatrix, (float k0, float k1) knotVector)
    {
        _pointMatrix = pointMatrix;
        KnotVector = knotVector;
        validCoefficients = false;
        curve = default;
    }

    // serialized data
    Vector3Matrix4x1 _pointMatrix;
    public Vector3Matrix4x1 PointMatrix {
        get => _pointMatrix;
        set => _ = (_pointMatrix = value, validCoefficients = false);
    }
    float k0, k1;
    public (float k0, float k1) KnotVector {
        get => (k0, k1);
        set => _ = ((k0, k1) = value, validCoefficients = false);
    }

    Polynomial3D curve;
    public Polynomial3D Curve
    {
        get {
            ReadyCoefficients();
            return curve;
        }
    }

    // cached data to accelerate calculations
    [NonSerialized]
    bool validCoefficients; // inverted isDirty flag (can't default to true in structs)

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void ReadyCoefficients()
    {
        if (validCoefficients)
            return; // no need to update
        validCoefficients = true;
        curve = SplineUtils.CalculateHermiteCurve(_pointMatrix, k0, k1);
    }
}
