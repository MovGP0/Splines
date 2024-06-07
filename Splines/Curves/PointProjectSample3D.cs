using System.Numerics;

namespace Splines.Curves;

internal struct PointProjectSample3D
{
    public float t;
    public float distDeltaSq;
    public Vector3 f;
    public Vector3 fp;
}