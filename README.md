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
