using System.Numerics;

namespace Splines.Curves;

internal struct PointProjectSample2D
{
    public float t;
    public float distDeltaSq;
    public Vector2 f;
    public Vector2 fp;
}