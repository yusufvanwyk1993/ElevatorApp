using Microsoft.Extensions.Logging;

namespace Elevator.Lib.Models.Services;

/// <summary>
/// HighSpeedElevatorService is a specialized elevator service for high-speed elevators.
/// </summary>
/// <param name="id"></param>
/// <param name="logger"></param>
/// <param name="maxCapacity"></param>
/// <param name="movementSpeed"></param>
public class HighSpeedElevatorService(int id, ILogger logger, int maxCapacity = 20, int movementSpeed = 1000) : BaseElevatorService(id, logger, maxCapacity, false, false, movementSpeed);