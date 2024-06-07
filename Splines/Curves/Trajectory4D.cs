using System.Numerics;
using Splines.Unity;
using Splines.UtilityTypes;

namespace Splines.Curves;

/// <summary>Various trajectory methods to calculate displacement, velocity, max ranges, and more in 4D</summary>
public partial struct Trajectory4D
{
    public Vector4 Position { get; set; }
    public Vector4 Velocity { get; set; }
    public Vector4 Acceleration { get; set; }

    /// <summary>Constructs a trajectory using initial velocity</summary>
    /// <param name="position">The initial position at time 0</param>
    /// <param name="velocity">The initial velocity at time 0</param>
    /// <param name="acceleration">The constant acceleration or gravity vector</param>
    public Trajectory4D(Vector4 position, Vector4 velocity, Vector4 acceleration)
    {
        Position = position;
        Velocity = velocity;
        Acceleration = acceleration;
    }

    /// <summary>Returns the position at time t</summary>
    public Vector4 GetPosition(float t) => Position + t * Velocity + 0.5f * t * t * Acceleration;

    /// <summary>Returns the velocity at time t</summary>
    public Vector4 GetVelocity(float t) => Velocity + t * Acceleration;

    /// <summary>Returns the acceleration, which is the same regardless of time</summary>
    public Vector4 GetAcceleration() => Acceleration;

    /// <summary>Returns the time when this trajectory reaches its apex in the Y dimension</summary>
    public float TimeAtApex => -(Velocity.Y / Acceleration.Y);

    /// <summary>Returns the position at the apex in the Y dimension</summary>
    public Vector4 Apex => GetPosition(TimeAtApex);

    /// <summary>Returns the time when this trajectory hits the ground in the Y dimension, assuming it starts at ground level</summary>
    public float GetLandingTime() => TimeAtApex * 2;

    /// <summary>Returns the landing position of this trajectory in the Y dimension, assuming it starts at ground level</summary>
    public Vector4 GetLandingPosition() => new(Position.X + GetLandingOffsetX(), Position.Y, Position.Z + GetLandingOffsetZ(), Position.W + GetLandingOffsetW());

    /// <summary>Returns the horizontal displacement in the X dimension from the starting position when this trajectory lands, assuming it starts and ends at ground level</summary>
    public float GetLandingOffsetX() => -2 * Velocity.X * Velocity.Y / Acceleration.Y;

    /// <summary>Returns the horizontal displacement in the Z dimension from the starting position when this trajectory lands, assuming it starts and ends at ground level</summary>
    public float GetLandingOffsetZ() => -2 * Velocity.Z * Velocity.Y / Acceleration.Y;

    /// <summary>Returns the horizontal displacement in the W dimension from the starting position when this trajectory lands, assuming it starts and ends at ground level</summary>
    public float GetLandingOffsetW() => -2 * Velocity.W * Velocity.Y / Acceleration.Y;

    /// <summary>Returns the two time values when passing by a specific height in the Y dimension. Note that this may return either 0, 1, or 2 results. The time values may also be negative</summary>
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
    public static Vector4 GetPosition(Vector4 position, Vector4 velocity, Vector4 acceleration, float time)
    {
        return position + velocity * time + 0.5f * time * time * acceleration;
    }

    /// <summary>Returns the maximum height that can possibly be reached if speed was redirected upwards in the Y dimension, given a current height and speed</summary>
    /// <param name="gravity">Gravitational acceleration in meters per second</param>
    /// <param name="currentHeight">Current height in meters</param>
    /// <param name="speed">Launch speed in meters per second</param>
    /// <returns>Potential height in meters</returns>
    public static float GetHeightPotential(float gravity, float currentHeight, float speed)
        => currentHeight + speed * speed / (2 * -gravity);

    /// <summary>Outputs the speed of an object with a given height potential and current height in the Y dimension, if it exists</summary>
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
