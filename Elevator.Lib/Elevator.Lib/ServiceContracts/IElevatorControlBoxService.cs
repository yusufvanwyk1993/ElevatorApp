using Elevator.Lib.Models.Services;

namespace Elevator.Lib.ServiceContracts;

/// <summary>
/// Contract for Elevator Control Box Service
/// </summary>
public interface IElevatorControlBoxService
{
    Task BootUpSystemAsync();
    Task LoadMenuAsync();
    void ShowStatusAll();
    Task ElevatorMenuAsync(IBaseElevatorService elevator, int elevatorNumber);
    Task InitializeTestAsync(List<int> elevatorNumbers, int count, int delayMs = 500,
        CancellationToken cancellationToken = default);
}