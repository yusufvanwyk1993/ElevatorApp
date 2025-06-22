using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;
using Microsoft.Extensions.Logging;

namespace Elevator.Lib.Models.ElevatorTypes;

/// <summary>
/// Base class for elevators.
/// </summary>
public class BaseElevator
{
    public int Id;
    public PassengerInformation PassengerInformation;
    public State State;
    public DoorState DoorState;
    public Floor CurrentFloor;
    public int MovementSpeed;
    public List<Request> Requests;
    public ILogger Logger;
    public bool UseDefaultResetPoint;
    public bool EnableLogs;
}