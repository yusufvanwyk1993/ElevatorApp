using Elevator.Lib.Extensions;
using System.Globalization;

namespace Elevator.Tests.ExtensionsTest;

public class IntExtensionTests
{
    public IntExtensionTests()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
    }

    [Theory]
    [InlineData(1000, "1 m/s")]
    [InlineData(500, "2 m/s")]
    [InlineData(2000, "0.5 m/s")]
    [InlineData(333, "3.003 m/s")]
    [InlineData(250, "4 m/s")]
    [InlineData(1500, "0.667 m/s")]
    public void FormatMovementSpeed_ReturnsExpectedString(int movementSpeed, string expected)
    {
        var result = movementSpeed.FormatMovementSpeed();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void FormatMovementSpeed_Zero_ThrowsDivideByZeroExceptionOrReturnsInfinity()
    {
        try
        {
            var result = 0.FormatMovementSpeed();
            Assert.Equal("∞ m/s", result);
        }
        catch (DivideByZeroException)
        {
            Assert.True(true, "DivideByZeroException was thrown as expected.");
        }
    }

    [Fact]
    public void FormatMovementSpeed_NegativeValue_ReturnsNegativeSpeed()
    {
        var result = (-1000).FormatMovementSpeed();
        Assert.Equal("-1 m/s", result);
    }

    [Fact]
    public void FormatMovementSpeed_MinValue_DoesNotThrow()
    {
        var result = int.MinValue.FormatMovementSpeed();
        Assert.False(string.IsNullOrEmpty(result));
    }
}