using System.Numerics;

namespace Splines.Extensions;

public static class QuaternionExtensions
{
    /// <summary>
    /// Rotates a vector by a quaternion.
    /// </summary>
    /// <param name="rotation">The quaternion representing the rotation.</param>
    /// <param name="point">The vector to be rotated.</param>
    /// <returns>The rotated vector.</returns>
    public static Vector3 Multiply(this Quaternion rotation, Vector3 point)
    {
        // Extract the vector part of the quaternion
        Vector3 u = new Vector3(rotation.X, rotation.Y, rotation.Z);

        // Extract the scalar part of the quaternion
        float s = rotation.W;

        // Perform the quaternion rotation
        Vector3 rotatedVector = 2.0f * Vector3.Dot(u, point) * u
                                + (s * s - Vector3.Dot(u, u)) * point
                                + 2.0f * s * Vector3.Cross(u, point);

        return rotatedVector;
    }

    /// <summary>
    /// Creates a quaternion that represents a rotation with the specified forward and upwards directions.
    /// </summary>
    /// <param name="forward">The forward direction vector. This vector points in the direction the rotation is facing.</param>
    /// <param name="upwards">The upwards direction vector. This vector points in the direction considered "up" for the rotation.</param>
    /// <returns>A <see cref="Quaternion"/> representing the rotation that aligns the z-axis with the forward vector and the y-axis as close as possible to the upwards vector.</returns>
    /// <remarks>
    /// The LookRotation method constructs a rotation that looks in the forward direction with the head upwards along the upwards vector as closely as possible.
    /// It is important to note that the forward vector and upwards vector should not be collinear to ensure a well-defined rotation.
    /// If the vectors are not normalized, they will be normalized inside the method.
    /// In cases where the forward vector is near-parallel to the upwards direction, the method might not produce a stable rotation.
    /// As such, adjustments to the input vectors may be necessary to achieve the desired orientation.
    /// </remarks>
    public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
    {
        // Normalize input vectors
        Vector3 zAxis = Vector3.Normalize(forward); // Forward vector
        Vector3 xAxis = Vector3.Normalize(Vector3.Cross(upwards, zAxis)); // Right vector
        Vector3 yAxis = Vector3.Cross(zAxis, xAxis); // Up vector, corrected to be orthogonal to zAxis

        // Construct a rotation matrix from the right (x), up (y), and forward (z) vectors
        Matrix4x4 rotationMatrix = new Matrix4x4(
            xAxis.X, yAxis.X, zAxis.X, 0,
            xAxis.Y, yAxis.Y, zAxis.Y, 0,
            xAxis.Z, yAxis.Z, zAxis.Z, 0,
            0, 0, 0, 1);

        // Create a quaternion from the rotation matrix
        return Quaternion.CreateFromRotationMatrix(rotationMatrix);
    }

    /// <summary>
    /// Spherical Linear Interpolation (Slerp) for quaternions.
    /// </summary>
    /// <remarks>
    /// Used to interpolate between two quaternion orientations smoothly.
    /// </remarks>
    public static Quaternion SlerpUnclamped(this Quaternion a, Quaternion b, float t)
    {
        // Ensure the quaternions are normalized
        a = Quaternion.Normalize(a);
        b = Quaternion.Normalize(b);

        // Compute the cosine of the angle between the two vectors.
        float dot = Quaternion.Dot(a, b);

        // If the dot product is negative, the quaternions have opposite handedness
        // and slerp won't take the shorter path. To fix this, reverse one quaternion.
        if (dot < 0.0f)
        {
            b = new Quaternion(-b.X, -b.Y, -b.Z, -b.W);
            dot = -dot;
        }

        const float DOT_THRESHOLD = 0.9995f;
        if (dot > DOT_THRESHOLD)
        {
            // If the inputs are too close for comfort, linearly interpolate and normalize the result.
            Quaternion result = Quaternion.Lerp(a, b, t);
            result = Quaternion.Normalize(result);
            return result;
        }

        // Acos(dot) gives us the angle between the two quaternions
        float theta_0 = (float)Math.Acos(dot);        // theta_0 = angle between input vectors
        float theta = theta_0 * t;                    // theta = angle between a and the result
        float sin_theta = (float)Math.Sin(theta);     // compute this value only once
        float sin_theta_0 = (float)Math.Sin(theta_0); // compute this value only once

        float s0 = (float)Math.Cos(theta) - dot * sin_theta / sin_theta_0;  // == sin(theta_0 - theta) / sin(theta_0)
        float s1 = sin_theta / sin_theta_0;

        return new(
            a.X * s0 + b.X * s1,
            a.Y * s0 + b.Y * s1,
            a.Z * s0 + b.Z * s1,
            a.W * s0 + b.W * s1
       );
    }
}
