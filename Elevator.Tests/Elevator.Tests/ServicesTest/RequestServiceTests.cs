using Elevator.Lib.Models;
using Elevator.Lib.Models.ElevatorTypes;
using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;
using Elevator.Lib.Models.Services;
using Elevator.Lib.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elevator.Tests.ServicesTest;

public class RequestServiceTests
{
    [Fact]
    public void CreateRequest_ShouldReturnValidRequest()
    {
        // Arrange
        var logger = new LoggerFactory().CreateLogger("TestLogger");
        var service = new RequestService
        {
            _logger = logger
        };
        var elevatorService = new BaseElevatorService(1, logger, 10, true, true);
        var elevators = new Dictionary<int, BaseElevatorService>
        {
            { 1, elevatorService }
        };

        // Simulate user input: select elevator 1, departure 0, destination 1, capacity 1
        var input = new StringReader("1\n0\n1\n1\n");
        Console.SetIn(input);

        // Act
        var request = service.CreateRequest(elevators, out var selectedElevator);

        // Assert
        Assert.NotNull(request);
        Assert.Equal(Floor.Ground, request.DepartureFloor);
        Assert.Equal(Floor.FirstFloor, request.DestinationFloor);
        Assert.Equal(1, request.PassengerCapacity);
        Assert.False(request.OnBoard);
        Assert.Equal(1, selectedElevator);
    }

    [Fact]
    public void SelectElevator_ShouldReturnSelectedKey()
    {
        var logger = new LoggerFactory().CreateLogger("TestLogger");
        var service = new RequestService();
        service._logger = logger;
        var elevatorService1 = new BaseElevatorService(1, logger, 10, true, true);
        var elevatorService2 = new BaseElevatorService(2, logger, 10, true, true);
        var elevators = new Dictionary<int, BaseElevatorService>
        {
            { 1, elevatorService1 },
            { 2, elevatorService2 }
        };
        Console.SetIn(new StringReader("2\n"));
        var selected = service.SelectElevator(elevators);
        Assert.Equal(2, selected);
    }

    [Fact]
    public void ShowStatus_ShouldNotThrow()
    {
        var logger = new LoggerFactory().CreateLogger("TestLogger");
        var service = new RequestService();
        service._logger = logger;
        var elevator = new BaseElevator
        {
            CurrentFloor = Floor.Ground,
            State = State.Stationary,
            DoorState = DoorState.Closed,
            MovementSpeed = 1000,
            PassengerInformation = new PassengerInformation {  MaxCapacity = 10 , Capacity = 1}
        };
        var ex = Record.Exception(() => service.ShowStatus(elevator));
        Assert.Null(ex);
    }
}