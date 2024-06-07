using System.Diagnostics;
using System.Globalization;

namespace Splines.Curves;

/// <summary>
/// Represents a 1D spring system with damping.
/// </summary>
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public partial struct Spring1D
{
    private double InitialPosition { get; }
    private double InitialVelocity { get; }
    private double Damping { get; }
    private double Stiffness { get; }
    private double Mass { get; }
    private double TargetPosition { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Spring1D"/> struct.
    /// </summary>
    /// <param name="stiffness">The stiffness of the spring.</param>
    /// <param name="damping">The damping coefficient of the spring.</param>
    /// <param name="initialPosition">The initial position of the spring mass.</param>
    /// <param name="initialVelocity">The initial velocity of the spring mass.</param>
    /// <param name="targetPosition">The target position of the spring mass.</param>
    /// <param name="mass">The mass of the spring.</param>
    public Spring1D(
        double stiffness,
        double damping,
        double initialPosition = 0d,
        double initialVelocity = 0d,
        double targetPosition = 1d,
        double mass = 1d)
    {
        Mass = mass;
        InitialPosition = initialPosition;
        InitialVelocity = initialVelocity;
        TargetPosition = targetPosition;
        Stiffness = stiffness;
        Damping = damping;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Spring1D"/> from the specified duration and bounce parameters.
    /// </summary>
    /// <param name="duration">The duration of the spring motion.</param>
    /// <param name="bounce">The bounce coefficient of the spring.</param>
    /// <param name="initialPosition">The initial position of the spring mass.</param>
    /// <param name="initialVelocity">The initial velocity of the spring mass.</param>
    /// <param name="targetPosition">The target position of the spring mass.</param>
    /// <param name="mass">The mass of the spring.</param>
    /// <returns>A new <see cref="Spring1D"/> instance.</returns>
    [Pure]
    public static Spring1D CreateFromDurationAndBounce(
        double duration,
        double bounce,
        double initialPosition = 0d,
        double initialVelocity = 0d,
        double targetPosition = 1d,
        double mass = 1d)
    {
        var (stiffness, damping) = CalculateDampingAndStiffness(duration, bounce, mass);
        return new Spring1D(stiffness, damping, initialPosition, initialVelocity, targetPosition, mass);
    }

    /// <summary>
    /// Calculates the damping and stiffness based on the bounce and duration parameters.
    /// </summary>
    /// <param name="duration">The duration of the spring motion.</param>
    /// <param name="bounce">The bounce coefficient of the spring.</param>
    /// <param name="mass">The mass of the spring.</param>
    /// <returns>A tuple containing the stiffness and damping coefficients.</returns>
    [Pure]
    private static (double stiffness, double damping) CalculateDampingAndStiffness(double duration, double bounce, double mass)
    {
        double dampingRatio = -Math.Log(bounce) / Math.Sqrt(Math.PI * Math.PI + Math.Log(bounce) * Math.Log(bounce));
        double angularFrequency = Math.PI / duration;

        var stiffness = mass * angularFrequency * angularFrequency;
        var damping = 2 * mass * dampingRatio * angularFrequency;
        return (stiffness, damping);
    }

    /// <summary>
    /// Evaluates the position of the spring mass at the specified time.
    /// </summary>
    /// <param name="time">The time at which to evaluate the position.</param>
    /// <returns>The position of the spring mass at the specified time.</returns>
    [Pure]
    public double Eval(float time)
    {
        double displacement = InitialPosition - TargetPosition;
        double force = -Stiffness * displacement;
        double dampingForce = -Damping * InitialVelocity;
        double acceleration = (force + dampingForce) / Mass;

        double newVelocity = InitialVelocity + acceleration * time;
        double newPosition = InitialPosition + newVelocity * time;

        return newPosition;
    }

    /// <summary>
    /// Returns a string representation of the current <see cref="Spring1D"/> instance.
    /// </summary>
    /// <returns>A string that represents the current <see cref="Spring1D"/> instance.</returns>
    [Pure]
    public override string ToString()
    {
        var initialPosition = InitialPosition.ToString(CultureInfo.InvariantCulture);
        var initialVelocity = InitialVelocity.ToString(CultureInfo.InvariantCulture);
        var targetPosition = TargetPosition.ToString(CultureInfo.InvariantCulture);
        var stiffness = Stiffness.ToString(CultureInfo.InvariantCulture);
        var damping = Damping.ToString(CultureInfo.InvariantCulture);
        var mass = Mass.ToString(CultureInfo.InvariantCulture);
        return $"Spring1D(initialPosition: {initialPosition}, initialVelocity: {initialVelocity}, targetPos: {targetPosition}, stiffness: {stiffness}, damping: {damping}, mass: {mass})";
    }

    [Pure]
    private string DebuggerDisplay => ToString();
}
