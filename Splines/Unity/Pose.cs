using System.Numerics;
using Splines.Extensions;

namespace Splines.Unity;

/// <summary>
/// Represents a pose with a position and a rotation in 3D space.
/// </summary>
/// <remarks>
/// This structure is used to define a specific orientation and location in 3D space.
/// It can be used for various applications such as animations, physics simulations, and transformations.
/// </remarks>
[Serializable]
public struct Pose : IEquatable<Pose>
{
    /// <summary>
    /// Gets or sets the position of the pose.
    /// </summary>
    public Vector3 Position
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the rotation of the pose.
    /// </summary>
    public Quaternion Rotation
    {
        [Pure]
        get;
        set;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Pose"/> struct with the specified position and rotation.
    /// </summary>
    /// <param name="position">The position of the pose.</param>
    /// <param name="rotation">The rotation of the pose.</param>
    public Pose(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    /// <summary>
    /// Returns a string that represents the current pose.
    /// </summary>
    /// <returns>A string that represents the current pose.</returns>
    [Pure]
    public override string ToString() => $"({Position.ToString()}, {Rotation.ToString()})";

    /// <summary>
    /// Returns a string that represents the current pose using the specified format.
    /// </summary>
    /// <param name="format">The format to use.</param>
    /// <returns>A string that represents the current pose using the specified format.</returns>
    [Pure]
    public string ToString(string format) => $"({Position.ToString(format)}, {Rotation.ToString()})";

    /// <summary>
    /// Gets the transformed pose by applying the specified pose transformation.
    /// </summary>
    /// <param name="lhs">The pose transformation to apply.</param>
    /// <returns>The transformed pose.</returns>
    [Pure]
    public Pose GetTransformedBy(Pose lhs)
    {
        return new Pose
        {
            Position = lhs.Position + lhs.Rotation.Multiply(Position),
            Rotation = lhs.Rotation * Rotation
        };
    }

    // Uncomment and implement if Transform class is available
    // /// <summary>
    // /// Gets the transformed pose by applying the specified transform.
    // /// </summary>
    // /// <param name="lhs">The transform to apply.</param>
    // /// <returns>The transformed pose.</returns>
    // public Pose GetTransformedBy(Transform lhs)
    // {
    //     return new Pose
    //     {
    //         Position = lhs.TransformPoint(Position),
    //         Rotation = lhs.rotation * Rotation
    //     };
    // }

    /// <summary>
    /// Gets the transformed pose by applying the specified transformation matrix.
    /// </summary>
    /// <param name="matrix">The transformation matrix to apply.</param>
    /// <returns>The transformed pose.</returns>
    [Pure]
    public Pose GetTransformedBy(Matrix4x4 matrix)
    {
        // Transform the position
        Vector3 transformedPosition = Vector3.Transform(Position, matrix);

        // Extract rotation from the matrix and apply it to the current rotation
        Quaternion extractedRotation = Quaternion.CreateFromRotationMatrix(matrix);
        Quaternion transformedRotation = extractedRotation * Rotation;

        return new(transformedPosition, transformedRotation);
    }

    /// <summary>
    /// Gets the forward direction of the pose.
    /// </summary>
    [Pure]
    public Vector3 Forward => Rotation.Multiply(Vector3Helper.Forward);

    /// <summary>
    /// Gets the right direction of the pose.
    /// </summary>
    [Pure]
    public Vector3 Right => Rotation.Multiply(Vector3Helper.Right);

    /// <summary>
    /// Gets the up direction of the pose.
    /// </summary>
    [Pure]
    public Vector3 Up => Rotation.Multiply(Vector3Helper.Up);

    /// <summary>
    /// Gets the identity pose with zero position and identity rotation.
    /// </summary>
    public static Pose Identity { get; } = new(Vector3.Zero, Quaternion.Identity);

    /// <summary>
    /// Determines whether the specified object is equal to the current pose.
    /// </summary>
    /// <param name="obj">The object to compare with the current pose.</param>
    /// <returns><c>true</c> if the specified object is equal to the current pose; otherwise, <c>false</c>.</returns>
    [Pure]
    public override bool Equals(object? obj)
    {
        return obj is Pose pose && Equals(pose);
    }

    /// <summary>
    /// Determines whether the specified pose is equal to the current pose.
    /// </summary>
    /// <param name="other">The pose to compare with the current pose.</param>
    /// <returns><c>true</c> if the specified pose is equal to the current pose; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool Equals(Pose other)
    {
        return Position == other.Position && Rotation == other.Rotation;
    }

    /// <summary>
    /// Serves as a hash function for the pose.
    /// </summary>
    /// <returns>A hash code for the current pose.</returns>
    [Pure]
    public override int GetHashCode() => Position.GetHashCode() ^ (Rotation.GetHashCode() << 1);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Pose"/> are equal.
    /// </summary>
    /// <param name="a">The first pose to compare.</param>
    /// <param name="b">The second pose to compare.</param>
    /// <returns><c>true</c> if the two poses are equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator ==(Pose a, Pose b) => a.Equals(b);

    /// <summary>
    /// Determines whether two specified instances of <see cref="Pose"/> are not equal.
    /// </summary>
    /// <param name="a">The first pose to compare.</param>
    /// <param name="b">The second pose to compare.</param>
    /// <returns><c>true</c> if the two poses are not equal; otherwise, <c>false</c>.</returns>
    [Pure]
    public static bool operator !=(Pose a, Pose b) => !(a == b);
}
