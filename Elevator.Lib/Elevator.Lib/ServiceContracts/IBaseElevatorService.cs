using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;

namespace Elevator.Lib.ServiceContracts;

/// <summary>
/// Contract for the base elevator service.
/// </summary>
public interface IBaseElevatorService
{
    void AddRequest();
    Task StartAsync(CancellationToken cancellationToken);
    Task MoveToFloorAsync(Floor targetFloor, CancellationToken cancellationToken);
    Request? GetNextRequest();
    void BoardRequest(Request request);
    void DisembarkRequest(Request request);
}