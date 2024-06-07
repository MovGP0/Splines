using System.Numerics;

namespace Splines.Curves;

internal struct PointProjectSample4D {
    public float t;
    public float distDeltaSq;
    public Vector4 f;
    public Vector4 fp;
}
