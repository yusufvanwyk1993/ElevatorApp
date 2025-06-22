using Elevator.Lib.Extensions;
using Elevator.Lib.Models.ElevatorTypes;
using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;
using Elevator.Lib.ServiceContracts;
using Elevator.Lib.Services;
using Microsoft.Extensions.Logging;

namespace Elevator.Lib.Models.Services;

/// <summary>
/// Base service for elevator operations, handling requests, movement, and passenger management.
/// </summary>
public class BaseElevatorService : RequestService, IBaseElevatorService
{
    public readonly BaseElevator Elevator;
    private readonly int _id;
 
    /// <summary>
    /// Initializes a new instance of the ElevatorService class, ready for use.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="logger"></param>
    /// <param name="maxCapacity"></param>
    /// <param name="useGroundResetPoint"></param>
    /// <param name="enableLogs"></param>
    /// <param name="movementSpeed"></param>
    public BaseElevatorService(int id ,ILogger logger, int maxCapacity, bool useGroundResetPoint, bool enableLogs ,int movementSpeed = 5000)
    {
        _logger = logger;
        _id = id;
        Elevator = new BaseElevator
        {
            Id = id,
            PassengerInformation = new PassengerInformation
            {
                MaxCapacity = maxCapacity
            },
            State = State.Stationary,
            DoorState = DoorState.Opened,
            CurrentFloor = Floor.Ground,
            MovementSpeed = movementSpeed,
            Requests = new List<Request>(),
            UseDefaultResetPoint = useGroundResetPoint,
            EnableLogs = enableLogs,
            Logger = _logger,
        };
    }

    /// <summary>
    /// Adds a new request to the elevator's request queue.
    /// </summary>
    public virtual void AddRequest()
    {
        try
        {
            var elevators = ElevatorControlBoxService.GlobalElevators;
            var request = CreateRequest(elevators, out var selectedElevator);
            if (elevators.TryGetValue(selectedElevator, out var targetElevatorService))
            {
                targetElevatorService.Elevator.Requests.Add(request);
                if (targetElevatorService.Elevator.EnableLogs)
                    targetElevatorService.Elevator.Logger.LogInformation($"[Elevator #{selectedElevator}] Request added: {request.PassengerCapacity} passengers from {request.DepartureFloor} to {request.DestinationFloor}.");
            }
            else
            {
                _logger.LogInformation($"Elevator #{selectedElevator} not found.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// Starts the elevator service, processing requests and moving the elevator as needed.
    /// </summary>
    /// <param name="cancellationToken"></param>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Elevator.Requests.Count > 0)
                {
                    // Find the next request to process based on the closest floor (either for pickup or drop-off)
                    var nextRequest = GetNextRequest();
                    if (nextRequest != null)
                    {
                        // If not onboard, move to departure and board
                        if (!nextRequest.OnBoard)
                        {
                            await MoveToFloorAsync(nextRequest.DepartureFloor, cancellationToken);
                            BoardRequest(nextRequest);
                        }
                        // Move to destination (disembarking will be handled by HandleBoardingAndDisembarking)
                        await MoveToFloorAsync(nextRequest.DestinationFloor, cancellationToken);
                    }
                }
                else
                {
                    await Task.Delay(500, cancellationToken);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// Moves the elevator to the specified floor, handling boarding and disembarking along the way.
    /// </summary>
    /// <param name="targetFloor"></param>
    /// <param name="cancellationToken"></param>
    public virtual async Task MoveToFloorAsync(Floor targetFloor, CancellationToken cancellationToken)
    {
        try
        {
            while (Elevator.CurrentFloor != targetFloor && !cancellationToken.IsCancellationRequested)
            {
                Elevator.State = State.Moving;
                Elevator.DoorState = DoorState.Closed;
                if ((int)Elevator.CurrentFloor < (int)targetFloor)
                    Elevator.CurrentFloor++;
                else
                    Elevator.CurrentFloor--;
                await Task.Delay(Elevator.MovementSpeed, cancellationToken);
                HandleBoardingAndDisembarking();
                var direction = (int)Elevator.CurrentFloor < (int)targetFloor ? "up" : "down";
                if (Elevator.EnableLogs)
                    Elevator.Logger.LogInformation($"[Elevator #{_id}] Currently on floor: {Elevator.CurrentFloor}, Moving {direction} at {Elevator.MovementSpeed.FormatMovementSpeed()}...Current capacity: {Elevator.PassengerInformation.Capacity}/{Elevator.PassengerInformation.MaxCapacity}...Door is now closed.");
            }
            Elevator.State = State.Stationary;
            Elevator.DoorState = DoorState.Opened;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// Gets the next request to process based on the current elevator state.
    /// </summary>
    /// <returns></returns>
    public virtual Request? GetNextRequest()
    {
        try
        {
            // Prioritize drop-offs for onboard passengers, then pickups
            var onboardRequests = Elevator.Requests.Where(r => r.OnBoard).OrderBy(r => Math.Abs((int)Elevator.CurrentFloor - (int)r.DestinationFloor)).ToList();
            if (onboardRequests.Any())
                return onboardRequests.First();
            var pickupRequests = Elevator.Requests.Where(r => !r.OnBoard).OrderBy(r => Math.Abs((int)Elevator.CurrentFloor - (int)r.DepartureFloor)).ToList();
            if (pickupRequests.Any())
                return pickupRequests.First();
            return null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// Boards a request at the current floor if it matches the departure floor and updates passenger capacity.
    /// </summary>
    /// <param name="request"></param>
    public virtual void BoardRequest(Request request)
    {
        try
        {
            if (Elevator.CurrentFloor == request.DepartureFloor && !request.OnBoard)
            {
                // Capacity check before boarding
                if (Elevator.PassengerInformation.Capacity + request.PassengerCapacity > Elevator.PassengerInformation.MaxCapacity)
                {
                    if (Elevator.EnableLogs)
                        Elevator.Logger.LogWarning($"[Elevator #{_id}] Cannot board {request.PassengerCapacity} passengers at floor {request.DepartureFloor}: capacity would be exceeded ({Elevator.PassengerInformation.Capacity}/{Elevator.PassengerInformation.MaxCapacity}).");
                    return;
                }
                request.OnBoard = true;
                Elevator.PassengerInformation.Capacity += request.PassengerCapacity;
                if (Elevator.EnableLogs)
                    Elevator.Logger.LogInformation($"[Elevator #{_id}] Boarded {request.PassengerCapacity} passengers at floor {request.DepartureFloor}. Current capacity: {Elevator.PassengerInformation.Capacity}/{Elevator.PassengerInformation.MaxCapacity}.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// Disembarks a request at the current floor if it matches the destination floor and updates passenger capacity.
    /// </summary>
    /// <param name="request"></param>
    public virtual void DisembarkRequest(Request request)
    {
        try
        {
            if (Elevator.CurrentFloor == request.DestinationFloor && request.OnBoard)
            {
                Elevator.PassengerInformation.Capacity -= request.PassengerCapacity;
                if (Elevator.PassengerInformation.Capacity < 0)
                    Elevator.PassengerInformation.Capacity = 0;
                if (Elevator.EnableLogs)
                    Elevator.Logger.LogInformation($"[Elevator #{_id}] Disembarked {request.PassengerCapacity} passengers at floor {request.DestinationFloor}. Current capacity: {Elevator.PassengerInformation.Capacity}/{Elevator.PassengerInformation.MaxCapacity}.");
                Elevator.Requests.Remove(request);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }

    /// <summary>
    /// Handles boarding and disembarking requests at the current floor.
    /// </summary>
    protected virtual void HandleBoardingAndDisembarking()
    {
        try
        {
            var requestsCopy = Elevator.Requests.ToList();
            foreach (var req in requestsCopy)
            {
                if (!req.OnBoard && req.DepartureFloor == Elevator.CurrentFloor)
                    BoardRequest(req);
                if (req.OnBoard && req.DestinationFloor == Elevator.CurrentFloor)
                    DisembarkRequest(req);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }
    }
}
