using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Tests1.Models;

public class CustomerTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_Should_ThrowException_WhenNameIsEmpty(string? name)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(Guid.NewGuid(), name!, "info@meron.se"));

        // Assert
        Assert.Equal("Customer name is required.", exception.Message);
    }

    [Fact]
    public void Create_Should_ThrowException_WhenEmailHasNoAtSign()
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(Guid.NewGuid(), "Meron AB", "not-an-email"));

        // Assert
        Assert.Equal("Email must be a valid email address", exception.Message);
    }

    [Fact]
    public void Create_Should_TrimAndLowercaseEmail()
    {
        // Act
        var customer = new Customer(Guid.NewGuid(), "Meron AB", "  Info@meron.SE  ");

        // Assert
        Assert.Equal("info@meron.se", customer.EmailAddress);
    }

    [Fact]
    public void Rename_Should_KeepTheSameId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer(id, "Meron AB", "info@meron.se");

        // Act
        customer.Rename("Meron Stockholm AB");

        // Assert
        Assert.Equal("Meron Stockholm AB", customer.Name);
        Assert.Equal(id, customer.Id);
    }
}