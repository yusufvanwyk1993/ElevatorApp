namespace Elevator.Lib.Models;

/// <summary>
/// Stores information about the passenger capacity of an elevator.
/// </summary>
public class PassengerInformation
{
    private int _maxCapacity;
    private int _capacity;

    
    public int MaxCapacity
    {
        get => _maxCapacity;
        set => _maxCapacity = value;
    }
    public int Capacity
    {
        get => _capacity;
        set =>
            _capacity =  value <= _maxCapacity
                ? value
                : throw new Exception("Capacity exceeded");
    }


}