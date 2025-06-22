# Elevator Control System Console Application

## Description

This is a console-based Elevator Control System that supports:
- Multiple floors
- Multiple elevators
- Different elevator types such as high-speed elevators, glass elevators, and freight elevators

The system provides real-time tracking and interaction with elevators in a building environment.

---

## Features

### 1. Real-Time Elevator Status
- Displays current floor of each elevator
- Shows direction of movement (up, down, or idle)
- Indicates motion status (moving or stationary)
- Displays number of passengers currently inside

### 2. Interactive Elevator Control
- Users can call elevators to a specific floor
- Specify number of passengers waiting on a floor
- Elevators respond dynamically based on status and availability

### 3. Multiple Floors and Elevators Support
- Application is designed to simulate buildings with many floors
- Handles multiple elevators operating simultaneously
- Ensures smooth elevator movement between different floors

### 4. Efficient Elevator Dispatching
- Implements a smart dispatching algorithm
- Nearest available elevator is selected to respond to a call
- Reduces passenger wait time and optimizes system efficiency

### 5. Passenger Limit Handling
- Each elevator has a predefined passenger capacity
- Prevents elevators from exceeding their capacity
- Automatically assigns additional elevators if needed

### 6. Consideration for Different Elevator Types
- Architecture supports extension for:
  - High-speed elevators
  - Glass elevators
  - Freight elevators
- Facilitates future upgrades and improvements

### 7. Real-Time Operation
- Fully interactive and responsive in real-time
- Elevator status updates immediately reflect user actions
- Elevators move and operate in simulated real-world timing

---

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

### Running the Application
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/elevator-console-app.git
   ```
2. Run Application
3. Specify how many elevators you want operational, example type 7 and hit enter.
   ```cmd
   2025-06-22 14:57:57 info: Elevator System[0] Welcome to the Elevator Control Box Service!: Select the number of elevators to initialize.
   ```
4. The Elevator App has a built in test that can be run optionally as part of a diagnostic test before letting real passengers use it. example type yes and the diagnostic test would run by creating random requests. however if you typed no then the test will be skipped.
   ```cmd
   2025-06-22 14:59:35 info: Elevator System[0] Welcome to the Elevator Control Box Service Test! Would like to run a test? (yes/no)
   ```
5. Menu Selection - select the desired elevator you'd want to use. Example type 4 to make a request to the 4th elevator.
   ```cmd
   2025-06-22 15:05:42 info: Elevator System[0] 
    Elevator Control Menu:
    2025-06-22 15:05:42 info: Elevator System[0] 1. Control Elevator #0
    2025-06-22 15:05:42 info: Elevator System[0] 2. Control Elevator #1
    2025-06-22 15:05:42 info: Elevator System[0] 3. Control Elevator #2
    2025-06-22 15:05:42 info: Elevator System[0] 4. Control Elevator #3
    2025-06-22 15:05:42 info: Elevator System[0] 5. Control Elevator #4
    2025-06-22 15:05:42 info: Elevator System[0] 6. Control Elevator #5
    2025-06-22 15:05:42 info: Elevator System[0] 7. Control Elevator #6
    2025-06-22 15:05:42 info: Elevator System[0] 0. Exit
   ```
6. Now see the specified elevator's menu. Example type 1 to start making a request to the specified elevator, alternatively type 2 to show the statuses of all elevators then lastly type 3 if you want to go back to a different elevator.
   ```cmd
   Elevator #3 Menu:
    2025-06-22 15:08:25 info: Elevator System[0] 1. Add Request
    2025-06-22 15:08:25 info: Elevator System[0] 2. Show Status (All Elevators)
    2025-06-22 15:08:25 info: Elevator System[0] 3. Back to Main Menu
    2025-06-22 15:08:25 info: Elevator System[0] Select an option:
   ```
7. Add request and confirm specified elevator (as a security measure). Example confirm and type 4
   ```cmd
    2025-06-22 15:12:54 info: Elevator System[0] Select which elevator you want to make a request to:
    2025-06-22 15:12:54 info: Elevator System[0] 0. Elevator #0
    2025-06-22 15:12:54 info: Elevator System[0] 1. Elevator #1
    2025-06-22 15:12:54 info: Elevator System[0] 2. Elevator #2
    2025-06-22 15:12:54 info: Elevator System[0] 3. Elevator #3
    2025-06-22 15:12:54 info: Elevator System[0] 4. Elevator #4
    2025-06-22 15:12:54 info: Elevator System[0] 5. Elevator #5
    2025-06-22 15:12:54 info: Elevator System[0] 6. Elevator #6
   ```
8. Select departure floor. Example typing -1 would be the basement.
   ```cmd
    2025-06-22 15:27:35 info: Elevator System[0] Select Departure Floor:
    2025-06-22 15:27:35 info: Elevator System[0] -1. Basement
    2025-06-22 15:27:35 info: Elevator System[0] 0. Ground
    2025-06-22 15:27:35 info: Elevator System[0] 1. First Floor
    2025-06-22 15:27:35 info: Elevator System[0] 2. Second Floor
    2025-06-22 15:27:35 info: Elevator System[0] 3. Third Floor
   ```
9. Select destination floor. Example typing 3 would be the third floor.
   ```cmd
    2025-06-22 15:30:08 info: Elevator System[0] Select Destination Floor:
    2025-06-22 15:30:08 info: Elevator System[0] -1. Basement
    2025-06-22 15:30:08 info: Elevator System[0] 0. Ground
    2025-06-22 15:30:08 info: Elevator System[0] 1. First Floor
    2025-06-22 15:30:08 info: Elevator System[0] 2. Second Floor
    2025-06-22 15:30:08 info: Elevator System[0] 3. Third Floor
   ```

10. Select how many passenger are expected to board. Exmaple type 17 for 17 passengers to be      onboarded on this request.
    ```cmd
    2025-06-22 15:31:07 info: Elevator System[0] Select how many passengers: 1 - ?
    ```
11. Output of request shown below. Notice the #4 which means the 4th elevator was selected and the logs includes the current activity of all elevators, what's shown below is just one request.
    ```cmd
    2025-06-22 15:33:51 info: Elevator System[0] [Elevator #4] Boarded 17 passengers at floor Basement. Current capacity: 17/20.
    2025-06-22 15:33:51 info: Elevator System[0] [Elevator #4] Currently on floor: Basement, Moving down at 0,2 m/s...Current capacity: 17/20...Door is now closed.
    2025-06-22 15:33:56 info: Elevator System[0] [Elevator #4] Currently on floor: Ground, Moving up at 0,2 m/s...Current capacity: 17/20...Door is now closed.
    2025-06-22 15:34:01 info: Elevator System[0] [Elevator #4] Currently on floor: FirstFloor, Moving up at 0,2 m/s...Current capacity: 17/20...Door is now closed.
    2025-06-22 15:34:06 info: Elevator System[0] [Elevator #4] Currently on floor: SecondFloor, Moving up at 0,2 m/s...Current capacity: 17/20...Door is now closed.
    2025-06-22 15:34:11 info: Elevator System[0] [Elevator #4] Disembarked 17 passengers at floor ThirdFloor. Current capacity: 0/20.
    ```
12. See all elevator statuses.
    ```cmd
      Elevator #0 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: Ground
      2025-06-22 15:41:18 info: Elevator System[0] State: Stationary
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Opened
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0]
      Elevator #1 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: Ground
      2025-06-22 15:41:18 info: Elevator System[0] State: Stationary
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Opened
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0]
      Elevator #2 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: Ground
      2025-06-22 15:41:18 info: Elevator System[0] State: Stationary
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Opened
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0]
      Elevator #3 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: Ground
      2025-06-22 15:41:18 info: Elevator System[0] State: Stationary
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Opened
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0]
      Elevator #4 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: ThirdFloor
      2025-06-22 15:41:18 info: Elevator System[0] State: Stationary
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Opened
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0]
      Elevator #5 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: Basement
      2025-06-22 15:41:18 info: Elevator System[0] State: Moving
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Closed
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0]
      Elevator #6 Status:
      2025-06-22 15:41:18 info: Elevator System[0] Current Floor: Ground
      2025-06-22 15:41:18 info: Elevator System[0] State: Stationary
      2025-06-22 15:41:18 info: Elevator System[0] Door State: Opened
      2025-06-22 15:41:18 info: Elevator System[0] Movement Speed: 0,2 m/s
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Capacity - 0
      2025-06-22 15:41:18 info: Elevator System[0] Passenger Information: Max Capacity - 20
      2025-06-22 15:41:18 info: Elevator System[0] ---------------------------

    ```

## Roadmap
### Features
1. Toggle logs on and off.
2. Add emergency button per elevator.
3. Add a way to cancel request if all users decide not to take the elevator anymore.
4. ...
### Development Changes
1. Unify prompts into a template to be reused.
2. Enhance unit test.
3. Make services more extensible.
4. ...