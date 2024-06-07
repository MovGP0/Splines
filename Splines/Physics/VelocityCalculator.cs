using System.Collections.Generic;
using System.Numerics;

namespace Splines.Physics;

/// <summary>
/// Calculate the velocity (change in position) of a moving object.
/// </summary>
internal static class VelocityCalculator
{
    /// <summary>
    /// Calculate the velocity of an object moving from p1 to p2 in 1D space.
    /// </summary>
    public static float CalculateVelocity(float p1, float p2)
    {
        float velocity = p2 - p1;
        return velocity;
    }

    /// <summary>
    /// Calculate the velocity of an object moving from p1 to p2 in 2D space.
    /// </summary>
    public static Vector2 CalculateVelocity(Vector2 p1, Vector2 p2)
    {
        Vector2 velocity = p2 - p1;
        return velocity;
    }

    /// <summary>
    /// Calculate the velocity of an object moving from p1 to p2 in 3D space.
    /// </summary>
    public static Vector3 CalculateVelocity(Vector3 p1, Vector3 p2)
    {
        Vector3 velocity = p2 - p1;
        return velocity;
    }

    /// <summary>
    /// Calculate the velocity of an object moving from p1 to p2 in 4D space.
    /// </summary>
    public static Vector4 CalculateVelocity(Vector4 p1, Vector4 p2)
    {
        Vector4 velocity = p2 - p1;
        return velocity;
    }

    /// <summary>
    /// Calculate the velocities for a sequence of 2D points.
    /// </summary>
    /// <param name="points">Array of 2D points.</param>
    /// <returns>An enumerable of 2D velocities.</returns>
    /// <exception cref="ArgumentException">Thrown when less than 2 points are provided.</exception>
    public static IEnumerable<Vector2> CalculateVelocities(Vector2[] points)
    {
        if (points.Length < 2)
            throw new ArgumentException("At least 2 points are required to calculate velocity.");

        for (int i = 0; i < points.Length - 1; i++)
        {
            yield return CalculateVelocity(points[i], points[i + 1]);
        }
    }

    /// <summary>
    /// Calculate the velocities for a sequence of 3D points.
    /// </summary>
    /// <param name="points">Array of 3D points.</param>
    /// <returns>An enumerable of 3D velocities.</returns>
    /// <exception cref="ArgumentException">Thrown when less than 2 points are provided.</exception>
    public static IEnumerable<Vector3> CalculateVelocities(Vector3[] points)
    {
        if (points.Length < 2)
            throw new ArgumentException("At least 2 points are required to calculate velocity.");

        for (int i = 0; i < points.Length - 1; i++)
        {
            yield return CalculateVelocity(points[i], points[i + 1]);
        }
    }

    /// <summary>
    /// Calculate the velocities for a sequence of 4D points.
    /// </summary>
    /// <param name="points">Array of 4D points.</param>
    /// <returns>An enumerable of 4D velocities.</returns>
    /// <exception cref="ArgumentException">Thrown when less than 2 points are provided.</exception>
    public static IEnumerable<Vector4> CalculateVelocities(Vector4[] points)
    {
        if (points.Length < 2)
            throw new ArgumentException("At least 2 points are required to calculate velocity.");

        for (int i = 0; i < points.Length - 1; i++)
        {
            yield return CalculateVelocity(points[i], points[i + 1]);
        }
    }
}
