using Elevator.Lib.Extensions;
using Elevator.Lib.Models.ElevatorTypes;
using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;
using Elevator.Lib.Models.Services;
using Elevator.Lib.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Elevator.Lib.Services;

/// <summary>
/// represents a service for handling elevator requests.
/// </summary>
public class RequestService : IRequestService
{
    public ILogger _logger;

    /// <summary>
    /// Initializes a new request based on user input.
    /// departure and destination floors are selected by the user,
    /// </summary>
    /// <param name="elevators"></param>
    /// <param name="selectedElevator"></param>
    /// <returns></returns>
    public Request CreateRequest(Dictionary<int, BaseElevatorService> elevators, out int selectedElevator)
    {
        selectedElevator = SelectElevator(elevators);
        int GetValidFloor(string prompt)
        {
            int floor;
            while (true)
            {
                _logger.LogInformation(prompt);
                _logger.LogInformation("-1. Basement");
                _logger.LogInformation("0. Ground");
                _logger.LogInformation("1. First Floor");
                _logger.LogInformation("2. Second Floor");
                _logger.LogInformation("3. Third Floor");
                var input = Console.ReadLine();
                if (int.TryParse(input, out floor) && floor is >= -1 and <= 3)
                    break;
                _logger.LogInformation("Invalid floor. Please enter a number between -1 and 3.");
            }
            return floor;
        }
        int departureFloor = GetValidFloor("Select Departure Floor:");
        int destinationFloor = GetValidFloor("Select Destination Floor:");
        int capacity;
        while (true)
        {
            _logger.LogInformation("Select how many passengers: 1 - ?");
            var capInput = Console.ReadLine();
            if (int.TryParse(capInput, out capacity) && capacity > 0)
                break;
            _logger.LogInformation("Invalid number of passengers. Please enter a positive integer.");
        }

        var request = new Request
        {
            PassengerCapacity = capacity,
            DepartureFloor = (Floor)departureFloor, 
            DestinationFloor = (Floor)destinationFloor,
            OnBoard = false
        };
        return request;
    }

    /// <summary>
    /// Shows the status of an elevator
    /// Current floor, state, door state, movement speed, and passenger information.
    /// </summary>
    /// <param name="elevator"></param>
    public void ShowStatus(BaseElevator elevator)
    {
        _logger.LogInformation($"Current Floor: {elevator.CurrentFloor}");
        _logger.LogInformation($"State: {elevator.State}");
        _logger.LogInformation($"Door State: {elevator.DoorState}");
        _logger.LogInformation($"Movement Speed: {elevator.MovementSpeed.FormatMovementSpeed()}");
        _logger.LogInformation($"Passenger Information: Capacity - {elevator.PassengerInformation.Capacity}");
        _logger.LogInformation($"Passenger Information: Max Capacity - {elevator.PassengerInformation.MaxCapacity}");
    }

    /// <summary>
    /// Selects an elevator from the available options.
    /// This links the request (departure and destination floor) and the elevator (by int #).
    /// </summary>
    /// <param name="elevators"></param>
    /// <returns></returns>
    public int SelectElevator(Dictionary<int, BaseElevatorService> elevators)
    {
        _logger.LogInformation("Select which elevator you want to make a request to:");
        foreach (var kvp in elevators)
        {
            _logger.LogInformation($"{kvp.Key}. Elevator #{kvp.Key}");
        }
        int selected;
        while (true)
        {
            _logger.LogInformation("Enter the elevator number:");
            var input = Console.ReadLine();
            if (int.TryParse(input, out selected) && elevators.ContainsKey(selected))
                break;
            _logger.LogInformation("Invalid selection. Please try again.");
        }
        return selected;
    }
}