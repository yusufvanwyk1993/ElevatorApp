using Elevator.Lib.ServiceContracts;
using Elevator.Lib.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddScoped<IElevatorControlBoxService,ElevatorControlBoxService>();
var serviceProvider = services.BuildServiceProvider();
var elevatorControlBoxService =  serviceProvider.GetRequiredService<IElevatorControlBoxService>();
await elevatorControlBoxService.BootUpSystemAsync();
