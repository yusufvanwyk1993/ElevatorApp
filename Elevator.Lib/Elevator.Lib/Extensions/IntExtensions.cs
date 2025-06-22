namespace Elevator.Lib.Extensions;

/// <summary>
/// All int extensions.
/// </summary>
public static class IntExtensions
{
    /// <summary>
    /// Converts an integer representing movement speed in milliseconds to a formatted string in meters per second.
    /// </summary>
    /// <param name="movementSpeed"></param>
    /// <returns></returns>
    public static string FormatMovementSpeed(this int movementSpeed)
    {
        double speed = 1 / (movementSpeed / 1000.0);
        return $"{speed:0.###} m/s";
    }
}