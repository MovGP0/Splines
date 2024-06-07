using System.Collections.Generic;
using System.Numerics;

namespace Splines.Physics;

/// <summary>
/// Calculate the acceleration (change in velocity) of a moving object.
/// </summary>
internal static class AccelerationCalculator
{
    /// <summary>
    /// Calculate the acceleration of an object moving from p1 to p3 via p2 in 1D space.
    /// </summary>
    public static float CalculateAcceleration(float p1, float p2, float p3)
    {
        float velocity1 = VelocityCalculator.CalculateVelocity(p1, p2);
        float velocity2 = VelocityCalculator.CalculateVelocity(p2, p3);
        float acceleration = velocity2 - velocity1;
        return acceleration;
    }

    /// <summary>
    /// Calculate the acceleration of an object moving from p1 to p3 via p2 in 2D space.
    /// </summary>
    public static Vector2 CalculateAcceleration(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 velocity1 = VelocityCalculator.CalculateVelocity(p1, p2);
        Vector2 velocity2 = VelocityCalculator.CalculateVelocity(p2, p3);
        Vector2 acceleration = velocity2 - velocity1;
        return acceleration;
    }

    /// <summary>
    /// Calculate the acceleration of an object moving from p1 to p3 via p2 in 3D space.
    /// </summary>
    public static Vector3 CalculateAcceleration(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 velocity1 = VelocityCalculator.CalculateVelocity(p1, p2);
        Vector3 velocity2 = VelocityCalculator.CalculateVelocity(p2, p3);
        Vector3 acceleration = velocity2 - velocity1;
        return acceleration;
    }

    /// <summary>
    /// Calculate the acceleration of an object moving from p1 to p3 via p2 in 4D space.
    /// </summary>
    public static Vector4 CalculateAcceleration(Vector4 p1, Vector4 p2, Vector4 p3)
    {
        Vector4 velocity1 = VelocityCalculator.CalculateVelocity(p1, p2);
        Vector4 velocity2 = VelocityCalculator.CalculateVelocity(p2, p3);
        Vector4 acceleration = velocity2 - velocity1;
        return acceleration;
    }

    /// <summary>
    /// Calculate the acceleration for a sequence of 2D points.
    /// </summary>
    /// <param name="points">Array of 2D points.</param>
    /// <returns>An enumerable of 2D accelerations.</returns>
    /// <exception cref="ArgumentException">Thrown when less than 3 points are provided.</exception>
    public static IEnumerable<Vector2> CalculateAccelerations(Vector2[] points)
    {
        if (points.Length < 3)
            throw new ArgumentException("At least 3 points are required to calculate acceleration.");

        for (int i = 0; i < points.Length - 2; i++)
        {
            yield return CalculateAcceleration(points[i], points[i + 1], points[i + 2]);
        }
    }

    /// <summary>
    /// Calculate the acceleration for a sequence of 3D points.
    /// </summary>
    /// <param name="points">Array of 3D points.</param>
    /// <returns>An enumerable of 3D accelerations.</returns>
    /// <exception cref="ArgumentException">Thrown when less than 3 points are provided.</exception>
    public static IEnumerable<Vector3> CalculateAccelerations(Vector3[] points)
    {
        if (points.Length < 3)
            throw new ArgumentException("At least 3 points are required to calculate acceleration.");

        for (int i = 0; i < points.Length - 2; i++)
        {
            yield return CalculateAcceleration(points[i], points[i + 1], points[i + 2]);
        }
    }

    /// <summary>
    /// Calculate the acceleration for a sequence of 4D points.
    /// </summary>
    /// <param name="points">Array of 4D points.</param>
    /// <returns>An enumerable of 4D accelerations.</returns>
    /// <exception cref="ArgumentException">Thrown when less than 3 points are provided.</exception>
    public static IEnumerable<Vector4> CalculateAccelerations(Vector4[] points)
    {
        if (points.Length < 3)
            throw new ArgumentException("At least 3 points are required to calculate acceleration.");

        for (int i = 0; i < points.Length - 2; i++)
        {
            yield return CalculateAcceleration(points[i], points[i + 1], points[i + 2]);
        }
    }
}
