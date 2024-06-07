using System.Diagnostics;
using System.Globalization;
using System.Numerics;

namespace Splines.Curves;

/// <summary>
/// Represents a 4D spring system with damping.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public partial struct Spring4D
{
    private Vector4 InitialPosition { get; }
    private Vector4 InitialVelocity { get; }
    private float Damping { get; }
    private float Stiffness { get; }
    private float Mass { get; }
    private Vector4 TargetPosition { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Spring4D"/> struct.
    /// </summary>
    /// <param name="stiffness">The stiffness of the spring.</param>
    /// <param name="damping">The damping coefficient of the spring.</param>
    /// <param name="initialPosition">The initial position of the spring mass.</param>
    /// <param name="initialVelocity">The initial velocity of the spring mass.</param>
    /// <param name="targetPosition">The target position of the spring mass.</param>
    /// <param name="mass">The mass of the spring.</param>
    public Spring4D(
        float stiffness,
        float damping,
        Vector4 initialPosition,
        Vector4 initialVelocity,
        Vector4 targetPosition,
        float mass = 1f)
    {
        Mass = mass;
        InitialPosition = initialPosition;
        InitialVelocity = initialVelocity;
        TargetPosition = targetPosition;
        Stiffness = stiffness;
        Damping = damping;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Spring4D"/> from the specified duration and bounce parameters.
    /// </summary>
    /// <param name="duration">The duration of the spring motion.</param>
    /// <param name="bounce">The bounce coefficient of the spring.</param>
    /// <param name="initialPosition">The initial position of the spring mass.</param>
    /// <param name="initialVelocity">The initial velocity of the spring mass.</param>
    /// <param name="targetPosition">The target position of the spring mass.</param>
    /// <param name="mass">The mass of the spring.</param>
    /// <returns>A new <see cref="Spring4D"/> instance.</returns>
    public static Spring4D CreateFromDurationAndBounce(
        float duration,
        float bounce,
        Vector4 initialPosition,
        Vector4 initialVelocity,
        Vector4 targetPosition,
        float mass = 1f)
    {
        var (stiffness, damping) = CalculateDampingAndStiffness(duration, bounce, mass);
        return new Spring4D(stiffness, damping, initialPosition, initialVelocity, targetPosition, mass);
    }

    /// <summary>
    /// Calculates the damping and stiffness based on the bounce and duration parameters.
    /// </summary>
    /// <param name="duration">The duration of the spring motion.</param>
    /// <param name="bounce">The bounce coefficient of the spring.</param>
    /// <param name="mass">The mass of the spring.</param>
    /// <returns>A tuple containing the stiffness and damping coefficients.</returns>
    private static (float stiffness, float damping) CalculateDampingAndStiffness(float duration, float bounce, float mass)
    {
        double dampingRatio = -Math.Log(bounce) / Math.Sqrt(Math.PI * Math.PI + Math.Log(bounce) * Math.Log(bounce));
        double angularFrequency = Math.PI / duration;

        float stiffness = (float)(mass * angularFrequency * angularFrequency);
        float damping = (float)(2 * mass * dampingRatio * angularFrequency);
        return (stiffness, damping);
    }

    /// <summary>
    /// Evaluates the position of the spring mass at the specified time.
    /// </summary>
    /// <param name="time">The time at which to evaluate the position.</param>
    /// <returns>The position of the spring mass at the specified time.</returns>
    public Vector4 Eval(float time)
    {
        Vector4 displacement = InitialPosition - TargetPosition;
        Vector4 force = -Stiffness * displacement;
        Vector4 dampingForce = -Damping * InitialVelocity;
        Vector4 acceleration = (force + dampingForce) / Mass;

        Vector4 newVelocity = InitialVelocity + acceleration * time;
        Vector4 newPosition = InitialPosition + newVelocity * time;

        return newPosition;
    }

    /// <summary>
    /// Returns a string representation of the current <see cref="Spring4D"/> instance.
    /// </summary>
    /// <returns>A string that represents the current <see cref="Spring4D"/> instance.</returns>
    public override string ToString()
    {
        var initialPosition = InitialPosition.ToString();
        var initialVelocity = InitialVelocity.ToString();
        var targetPosition = TargetPosition.ToString();
        var stiffness = Stiffness.ToString(CultureInfo.InvariantCulture);
        var damping = Damping.ToString(CultureInfo.InvariantCulture);
        var mass = Mass.ToString(CultureInfo.InvariantCulture);
        return $"Spring4D(initialPosition: {initialPosition}, initialVelocity: {initialVelocity}, targetPos: {targetPosition}, stiffness: {stiffness}, damping: {damping}, mass: {mass})";
    }

    private string DebuggerDisplay => ToString();
}
