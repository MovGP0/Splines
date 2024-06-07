using System.Numerics;
using Splines.Unity;
using Splines.UtilityTypes;

namespace Splines.Curves;

/// <summary>Various trajectory methods to calculate displacement, velocity, max ranges, and more in 3D</summary>
public partial struct Trajectory3D
{
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
    public Vector3 Acceleration { get; set; }

    /// <summary>Constructs a trajectory using initial velocity</summary>
    /// <param name="position">The initial position at time 0</param>
    /// <param name="velocity">The initial velocity at time 0</param>
    /// <param name="acceleration">The constant acceleration or gravity vector</param>
    public Trajectory3D(Vector3 position, Vector3 velocity, Vector3 acceleration)
    {
        this.Position = position;
        this.Velocity = velocity;
        this.Acceleration = acceleration;
    }

    /// <summary>Returns the position at time t</summary>
    public Vector3 GetPosition(float t) => Position + t * Velocity + 0.5f * t * t * Acceleration;

    /// <summary>Returns the velocity at time t</summary>
    public Vector3 GetVelocity(float t) => Velocity + t * Acceleration;

    /// <summary>Returns the acceleration, which is the same regardless of time</summary>
    public Vector3 GetAcceleration() => Acceleration;

    public float TimeAtApex => -(Velocity.Y / Acceleration.Y);
    public Vector3 Apex => GetPosition(TimeAtApex);

    /// <summary>Returns the time when this trajectory hits the ground, assuming this trajectory starts at ground level</summary>
    public float GetLandingTime() => TimeAtApex * 2;

    /// <summary>Returns the landing position of this trajectory, assuming this trajectory starts at ground level</summary>
    public Vector3 GetLandingPosition() => new(Position.X + GetLandingOffset(), Position.Y, Position.Z);

    /// <summary>Returns the horizontal displacement from the starting position this trajectory lands, assuming this trajectory starts and ends at ground level</summary>
    public float GetLandingOffset() => -2 * Velocity.X * Velocity.Y / Acceleration.Y;

    /// <summary>Returns the two time values when passing by a specific height. Note that this may return either 0, 1, or 2 results. The time values may also be negative</summary>
    /// <param name="height">The height to get time values of</param>
    public ResultsMax2<float> GetTimesAtHeight(float height)
    {
        float discriminant = Velocity.Y * Velocity.Y - 2 * Acceleration.Y * (Position.Y - height);
        if (Approximately(discriminant, 0))
        {
            // 1 solution
            return new ResultsMax2<float>(TimeAtApex);
        }

        if (discriminant < 0)
        {
            // 0 solutions
            return default;
        }

        // 2 solutions
        float sqrt = Mathf.Sqrt(discriminant);
        float mvy = -Velocity.Y;
        return new ResultsMax2<float>(
            (mvy + sqrt) / Acceleration.Y,
            (mvy - sqrt) / Acceleration.Y
        );
    }

    /// <summary>Returns the position in a given trajectory, at the given time</summary>
    /// <param name="position">The initial position</param>
    /// <param name="velocity">The initial velocity</param>
    /// <param name="acceleration">The constant acceleration or gravity vector</param>
    /// <param name="time">The time to get the position at</param>
    public static Vector3 GetPosition(Vector3 position, Vector3 velocity, Vector3 acceleration, float time)
    {
        return position + velocity * time + 0.5f * time * time * acceleration;
    }

    /// <summary>Returns the maximum height that can possibly be reached if speed was redirected upwards, given a current height and speed</summary>
    /// <param name="gravity">Gravitational acceleration in meters per second</param>
    /// <param name="currentHeight">Current height in meters</param>
    /// <param name="speed">Launch speed in meters per second</param>
    /// <returns>Potential height in meters</returns>
    public static float GetHeightPotential(float gravity, float currentHeight, float speed)
        => currentHeight + speed * speed / (2 * -gravity);

    /// <summary>Outputs the speed of an object with a given height potential and current height, if it exists</summary>
    /// <param name="gravity">Gravitational acceleration in meters per second</param>
    /// <param name="currentHeight">Current height in meters</param>
    /// <param name="heightPotential">Potential height in meters</param>
    /// <param name="speed">Speed in meters per second</param>
    /// <returns>Whether there is a valid speed</returns>
    public static bool TryGetSpeedFromHeightPotential(float gravity, float currentHeight, float heightPotential, out float speed)
    {
        float speedSq = (heightPotential - currentHeight) * -2 * gravity;
        if (speedSq <= 0)
        {
            speed = default; // Imaginary speed :sparkles:
            return false;
        }

        speed = Mathf.Sqrt(speedSq);
        return true;
    }

    private static bool Approximately(float a, float b, float tolerance = 0.0001f)
    {
        return Mathf.Abs(a - b) < tolerance;
    }
}
