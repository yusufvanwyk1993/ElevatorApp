using Elevator.Lib.Models.Services;
using Microsoft.Extensions.Logging;

namespace Elevator.Lib.Models.Services;

/// <summary>
/// FreightElevatorService is a specialized elevator service for freight elevators.
/// </summary>
/// <param name="id"></param>
/// <param name="logger"></param>
/// <param name="maxCapacity"></param>
/// <param name="movementSpeed"></param>
public class FreightElevatorService(int id, ILogger logger, int maxCapacity = 50, int movementSpeed = 2000) : BaseElevatorService(id, logger, maxCapacity, false, false, movementSpeed);