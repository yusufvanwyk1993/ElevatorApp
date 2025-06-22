using Elevator.Lib.Models.Options;
using Elevator.Lib.Models.Requests;
using Elevator.Lib.Models.Services;
using Elevator.Lib.ServiceContracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Elevator.Lib.Services;

/// <summary>
/// Service for managing the elevator control box, handling multiple elevators and user interactions.
/// </summary>
public class ElevatorControlBoxService : IElevatorControlBoxService
{
    private readonly int _elevatorCount;
    private Dictionary<int, BaseElevatorService> _elevators;
    private readonly ILogger _logger;
    public static Dictionary<int, BaseElevatorService> GlobalElevators { get; private set; }
    private bool _testElevatorsOnStartup = true;
    
    /// <summary>
    /// Initializes a new instance of the ElevatorControlBoxService class, setting up the logger and reading the number of elevators from user input.
    /// </summary>
    public ElevatorControlBoxService()
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
        _logger = loggerFactory.CreateLogger("Elevator System");

        _logger.LogInformation(
            "Welcome to the Elevator Control Box Service!: Select the number of elevators to initialize.");
        string? elevatorCount = Console.ReadLine();

        ArgumentException.ThrowIfNullOrWhiteSpace(elevatorCount);
        _elevatorCount = int.Parse(elevatorCount);

    }

    /// <summary>
    /// Boots up the elevator system by initializing the elevators and starting the elevator tasks.
    /// </summary>
    public async Task BootUpSystemAsync()
    {
        _elevators = new Dictionary<int, BaseElevatorService>();
        GlobalElevators = _elevators;
        for (int i = 0; i < _elevatorCount; i++)
        {
            var elevator = new BaseElevatorService(i, _logger, 20, false, true);
            _elevators.Add(i, elevator);
        }

        await LoadTestQAndA();

        if (_testElevatorsOnStartup)
        {
            var elevatorKeys = _elevators.Keys.ToList();
            await InitializeTestAsync(elevatorKeys, _elevatorCount);
        }

        var cts = new CancellationTokenSource();
        var elevatorTasks = _elevators.Values
            .Select(elevator => Task.Run(async () => { await elevator.StartAsync(cts.Token); }, cts.Token)).ToList();

        // Attach the new menu to the control box
        await LoadMenuAsync();
        await Task.WhenAll(elevatorTasks);
    }

    /// <summary>
    /// Loads test questions and answers to determine if the user wants to run a test on the elevators.
    /// </summary>
    private async Task LoadTestQAndA()
    {
        _logger.LogInformation("Welcome to the Elevator Control Box Service Test! Would like to run a test? (yes/no)");
        string? input = Console.ReadLine()?.Trim().ToLower();
        if (string.IsNullOrWhiteSpace(input))
            await LoadTestQAndA();
        _testElevatorsOnStartup = input == "yes";
    }

    /// <summary>
    /// Loads the main menu for controlling elevators, allowing users to select an elevator and perform actions such as adding requests or showing status.
    /// </summary>
    public async Task LoadMenuAsync()
    {
        var cts = new CancellationTokenSource();
        while (!cts.IsCancellationRequested)
        {
            _logger.LogInformation("\nElevator Control Menu:");
            foreach (var _elevator in _elevators)
            {
                _logger.LogInformation($"{_elevator.Key + 1}. Control Elevator #{_elevator.Key}");
            }

            _logger.LogInformation("0. Exit");
            _logger.LogInformation("Select an elevator to control (or 0 to exit): ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int selected))
            {
                if (selected == 0)
                {
                    Environment.Exit(0);
                }

                int elevatorIndex = selected - 1;
                if (_elevators.ContainsKey(elevatorIndex))
                {
                    await ElevatorMenuAsync(_elevators[elevatorIndex], elevatorIndex);
                }
                else
                {
                    _logger.LogInformation("Invalid selection. Please try again.");
                }
            }
            else
            {
                _logger.LogInformation("Invalid selection. Please try again.");
            }
        }
    }

    /// <summary>
    /// Shows the status of all elevators, including their current requests and state.
    /// </summary>
    public void ShowStatusAll()
    {
        _logger.LogInformation("\n--- All Elevators Status ---");
        foreach (var kvp in _elevators)
        {
            _logger.LogInformation($"\nElevator #{kvp.Key} Status:");
            kvp.Value.ShowStatus(kvp.Value.Elevator);
        }

        _logger.LogInformation("---------------------------\n");
    }

    /// <summary>
    /// Runs the menu for a specific elevator, allowing users to add requests, show status, or return to the main menu.
    /// </summary>
    /// <param name="elevator"></param>
    /// <param name="elevatorNumber"></param>
    /// <returns></returns>
    public Task ElevatorMenuAsync(IBaseElevatorService elevator, int elevatorNumber)
    {
        while (true)
        {
            _logger.LogInformation($"\nElevator #{elevatorNumber} Menu:");
            _logger.LogInformation("1. Add Request");
            _logger.LogInformation("2. Show Status (All Elevators)");
            _logger.LogInformation("3. Back to Main Menu");
            _logger.LogInformation("Select an option: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    elevator.AddRequest();
                    break;
                case "2":
                    ShowStatusAll();
                    break;
                case "3":
                    return Task.CompletedTask;
                default:
                    _logger.LogInformation("Invalid choice, please try again.");
                    break;
            }
        }
    }

    /// <summary>
    /// Sets up a test scenario by randomly generating requests for elevators based on the provided elevator numbers.
    /// </summary>
    /// <param name="elevatorNumbers"></param>
    /// <param name="count"></param>
    /// <param name="delayMs"></param>
    /// <param name="cancellationToken"></param>
    public async Task InitializeTestAsync(List<int> elevatorNumbers, int count, int delayMs = 500,
        CancellationToken cancellationToken = default)
    {
        var random = new Random();
        var floors = Enum.GetValues(typeof(Floor)).Cast<Floor>().ToList();
        for (int i = 0; i < count; i++)
        {
            if (cancellationToken.IsCancellationRequested)
                break;
            // Randomly select an elevator number from the provided list
            int elevatorNumber = elevatorNumbers[random.Next(elevatorNumbers.Count)];
            if (!_elevators.ContainsKey(elevatorNumber))
                continue;
            var elevatorService = _elevators[elevatorNumber];
            int totalCapacity = elevatorService.Elevator.PassengerInformation.Capacity;
            var departure = floors[random.Next(floors.Count)];
            Floor destination;
            do
            {
                destination = floors[random.Next(floors.Count)];
            } while (destination == departure);

            int remaining = elevatorService.Elevator.PassengerInformation.MaxCapacity - totalCapacity;
            if (remaining <= 0) continue;
            int passengers = (i == count - 1) ? Math.Max(1, remaining) : Math.Min(random.Next(1, 6), remaining);
            var request = new Request
            {
                PassengerCapacity = passengers,
                DepartureFloor = departure,
                DestinationFloor = destination,
                OnBoard = false
            };
            elevatorService.Elevator.Requests.Add(request);
            if (elevatorService.Elevator.EnableLogs)
                elevatorService.Elevator.Logger.LogInformation(
                    $"[TestMax] Added request: {passengers} passengers from {departure} to {destination} on Elevator #{elevatorNumber}.");
            await Task.Delay(delayMs, cancellationToken);
        }
        _logger.LogInformation("Test initialization complete. Requests added to elevators.");
    }
}