using Elevator.Lib.Models.ElevatorTypes;
using Elevator.Lib.Models.Requests;
using Elevator.Lib.Models.Services;

namespace Elevator.Lib.ServiceContracts;

/// <summary>
/// Contract for the request service.
/// </summary>
public interface IRequestService
{
    Request CreateRequest(Dictionary<int, BaseElevatorService> elevators, out int selectedElevator);
    void ShowStatus(BaseElevator elevator);
    int SelectElevator(Dictionary<int, BaseElevatorService> elevators);
}