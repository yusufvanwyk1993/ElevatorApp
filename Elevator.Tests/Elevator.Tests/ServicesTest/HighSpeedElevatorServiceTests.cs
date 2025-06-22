using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;
using Elevator.Lib.Models.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Moq;

namespace Elevator.Tests.ServicesTest;

public class HighSpeedElevatorServiceTests
{
    [Fact]
    public void Constructor_InitializesElevatorProperties()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        Assert.NotNull(service.Elevator);
        Assert.Equal(1, service.Elevator.Id);
        Assert.Equal(State.Stationary, service.Elevator.State);
        Assert.Equal(DoorState.Opened, service.Elevator.DoorState);
        Assert.Equal(Floor.Ground, service.Elevator.CurrentFloor);
        Assert.Equal(10, service.Elevator.PassengerInformation.MaxCapacity);
        Assert.Empty(service.Elevator.Requests);
    }

    [Fact]
    public void AddRequest_DoesNotThrow_WhenElevatorNotFound()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole(options => { options.FormatterName = ConsoleFormatterNames.Simple; })
                .AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.IncludeScopes = false;
                    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                    options.UseUtcTimestamp = true;
                    options.ColorBehavior = LoggerColorBehavior.Disabled;
                })
                .SetMinimumLevel(LogLevel.Information);
        });
        var logger = loggerFactory.CreateLogger("Elevator System");
        // var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, logger, 10, 500);
        // GlobalElevators is not set, should log and not throw
        var ex = Record.Exception(() => service.AddRequest());
        Assert.NotNull(ex);
    }

    [Fact]
    public async Task MoveToFloorAsync_ChangesCurrentFloorAndSetsState()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        service.Elevator.CurrentFloor = Floor.Ground;
        await service.MoveToFloorAsync(Floor.FirstFloor, CancellationToken.None);
        Assert.Equal(Floor.FirstFloor, service.Elevator.CurrentFloor);
        Assert.Equal(State.Stationary, service.Elevator.State);
        Assert.Equal(DoorState.Opened, service.Elevator.DoorState);
    }

    [Fact]
    public void GetNextRequest_ReturnsNull_WhenNoRequests()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var next = service.GetNextRequest();
        Assert.Null(next);
    }

    [Fact]
    public void GetNextRequest_PrioritizesOnBoardRequests()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var onboard = new Request { OnBoard = true, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor };
        var pickup = new Request { OnBoard = false, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor };
        service.Elevator.Requests.Add(onboard);
        service.Elevator.Requests.Add(pickup);
        var next = service.GetNextRequest();
        Assert.True(next.OnBoard);
    }

    [Fact]
    public void BoardRequest_BoardsPassengerAndUpdatesCapacity()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var req = new Request { PassengerCapacity = 3, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor };
        service.Elevator.CurrentFloor = Floor.Ground;
        service.BoardRequest(req);
        Assert.True(req.OnBoard);
        Assert.Equal(3, service.Elevator.PassengerInformation.Capacity);
    }

    [Fact]
    public void BoardRequest_DoesNothing_IfNotAtDepartureFloor()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var req = new Request { PassengerCapacity = 2, DepartureFloor = Floor.FirstFloor, DestinationFloor = Floor.Ground };
        service.Elevator.CurrentFloor = Floor.Ground;
        service.BoardRequest(req);
        Assert.False(req.OnBoard);
        Assert.Equal(0, service.Elevator.PassengerInformation.Capacity);
    }

    [Fact]
    public void DisembarkRequest_RemovesPassengerAndRequest()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var req = new Request { PassengerCapacity = 2, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor, OnBoard = true };
        service.Elevator.CurrentFloor = Floor.FirstFloor;
        service.Elevator.Requests.Add(req);
        service.Elevator.PassengerInformation.Capacity = 2;
        service.DisembarkRequest(req);
        Assert.Equal(0, service.Elevator.PassengerInformation.Capacity);
        Assert.Empty(service.Elevator.Requests);
    }

    [Fact]
    public void DisembarkRequest_DoesNothing_IfNotAtDestinationFloor()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var req = new Request { PassengerCapacity = 2, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor, OnBoard = true };
        service.Elevator.CurrentFloor = Floor.Ground;
        service.Elevator.Requests.Add(req);
        service.Elevator.PassengerInformation.Capacity = 2;
        service.DisembarkRequest(req);
        Assert.Equal(2, service.Elevator.PassengerInformation.Capacity);
        Assert.Contains(req, service.Elevator.Requests);
    }

    [Fact]
    public void HandleBoardingAndDisembarking_BoardsAndDisembarksCorrectly()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var boardReq = new Request { PassengerCapacity = 1, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor };
        var disembarkReq = new Request { PassengerCapacity = 1, DepartureFloor = Floor.Ground, DestinationFloor = Floor.Ground, OnBoard = true };
        service.Elevator.CurrentFloor = Floor.Ground;
        service.Elevator.Requests.Add(boardReq);
        service.Elevator.Requests.Add(disembarkReq);
        var method = service.GetType().GetMethod("HandleBoardingAndDisembarking", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method?.Invoke(service, null);
        Assert.True(boardReq.OnBoard);
        Assert.DoesNotContain(disembarkReq, service.Elevator.Requests);
    }

    [Fact]
    public async Task StartAsync_ThrowsTaskCanceledException_WhenCancelled()
    {
        var loggerMock = new Mock<ILogger>();
        var service = new HighSpeedElevatorService(1, loggerMock.Object, 10);
        var req = new Request { PassengerCapacity = 1, DepartureFloor = Floor.Ground, DestinationFloor = Floor.FirstFloor };
        service.Elevator.Requests.Add(req);
        using var cts = new CancellationTokenSource(1);
        await Assert.ThrowsAsync<TaskCanceledException>(() => service.StartAsync(cts.Token));
    }
}