using Elevator.Lib.Models.Options;

namespace Elevator.Lib.Models.Requests;

/// <summary>
/// Represents a request to the elevator system.
/// </summary>
/// <param name="PassengerCapacity"></param>
/// <param name="DepartureFloor"></param>
/// <param name="DestinationFloor"></param>
public class Request
{
    public int PassengerCapacity { get; set; }
    public Floor DepartureFloor { get; set; }
    public Floor DestinationFloor { get; set; }
    public bool OnBoard { get; set; } = false;
}

