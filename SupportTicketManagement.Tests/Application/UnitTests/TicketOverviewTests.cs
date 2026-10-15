using NSubstitute;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;
using Xunit;

namespace SupportTicketManagement.Tests.Application.UnitTests;

public class TicketOverviewTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public async Task GetAllTicketsAsync_Should_ReturnNewestTicketsFirst()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var oldest = new Ticket(
            Guid.NewGuid(), "Oldest ticket", "Test description",
            TicketPriority.Normal, customerId)
        {
            CreatedAt = new DateTimeOffset(
                2026, 10, 1, 10, 0, 0, TimeSpan.Zero)
        };

        var newest = new Ticket(
            Guid.NewGuid(), "Newest ticket", "Test description",
            TicketPriority.Normal, customerId)
        {
            CreatedAt = new DateTimeOffset(
                2026, 10, 3, 10, 0, 0, TimeSpan.Zero)
        };

        var middle = new Ticket(
            Guid.NewGuid(), "Middle ticket", "Test description",
            TicketPriority.Normal, customerId)
        {
            CreatedAt = new DateTimeOffset(
                2026, 10, 2, 10, 0, 0, TimeSpan.Zero)
        };

        var repository = Substitute.For<ITicketRepository>();

        repository.GetAll().Returns(
            Task.FromResult(new List<Ticket>
            {
                oldest, newest, middle
            }));

        var service = new TicketService(
            repository,
            Substitute.For<IAdminService>(),
            Substitute.For<ICustomerService>());

        // Act
        var result = await service.GetAllTicketsAsync();

        // Assert
        Assert.True(result.Succeeded, result.ErrorMessage);

        Assert.Collection(
            result.Tickets,
            ticket => Assert.Equal(newest.Id, ticket.Id),
            ticket => Assert.Equal(middle.Id, ticket.Id),
            ticket => Assert.Equal(oldest.Id, ticket.Id));
    }


    [Theory]
    [Trait("Category", "Unit")]
    [InlineData("Printer offline")]
    [InlineData("PRINTER OFFLINE")]
    [InlineData("prINT")]
    public async Task SearchTicketsAsync_Should_MatchTitleIgnoringCase(
    string searchText)
    {
        // Arrange
        var customer = new Customer(
            Guid.NewGuid(), "North Office", "contact@north.example");

        var matchingTicket = new Ticket(
            Guid.NewGuid(), "Printer offline", "Cannot print",
            TicketPriority.Normal, customer.Id);

        var otherTicket = new Ticket(
            Guid.NewGuid(), "Email problem", "Cannot send email",
            TicketPriority.Normal, customer.Id);

        var repository = Substitute.For<ITicketRepository>();

        repository.GetAll().Returns(
            Task.FromResult(new List<Ticket>
            {
            otherTicket, matchingTicket
            }));

        var customers = Substitute.For<ICustomerService>();

        customers.GetAllCustomersAsync().Returns(
            Task.FromResult(new GetAllCustomersResult(
                true, new List<Customer> { customer }, null)));

        var service = new TicketService(
            repository,
            Substitute.For<IAdminService>(),
            customers);

        // Act
        var result = await service.SearchTicketsAsync(
            new SearchTicketRequest(searchText, null));

        // Assert
        Assert.True(result.Succeeded, result.ErrorMessage);

        var foundTicket = Assert.Single(result.Tickets);
        Assert.Equal(matchingTicket.Id, foundTicket.Id);
    }

    [Theory]
    [Trait("Category", "Unit")]
    [InlineData("North Office")]
    [InlineData("NORTH OFFICE")]
    [InlineData("noR")]
    public async Task SearchTicketsAsync_Should_MatchCustomerNameIgnoringCase(
    string searchText)
    {
        // Arrange
        var matchingCustomer = new Customer(
            Guid.NewGuid(), "North Office", "contact@north.example");

        var otherCustomer = new Customer(
            Guid.NewGuid(), "Sample Store", "contact@store.example");

        var matchingTicket = new Ticket(
            Guid.NewGuid(), "Printer offline", "Cannot print",
            TicketPriority.Normal, matchingCustomer.Id);

        var otherTicket = new Ticket(
            Guid.NewGuid(), "Printer setup", "Configure a printer",
            TicketPriority.Normal, otherCustomer.Id);

        var repository = Substitute.For<ITicketRepository>();

        repository.GetAll().Returns(
            Task.FromResult(new List<Ticket>
            {
            otherTicket, matchingTicket
            }));

        var customers = Substitute.For<ICustomerService>();

        customers.GetAllCustomersAsync().Returns(
            Task.FromResult(new GetAllCustomersResult(
                true,
                new List<Customer> { otherCustomer, matchingCustomer },
                null)));

        var service = new TicketService(
            repository,
            Substitute.For<IAdminService>(),
            customers);

        // Act
        var result = await service.SearchTicketsAsync(
            new SearchTicketRequest(searchText, null));

        // Assert
        Assert.True(result.Succeeded, result.ErrorMessage);

        var foundTicket = Assert.Single(result.Tickets);
        Assert.Equal(matchingTicket.Id, foundTicket.Id);
    }

    [Theory]
    [Trait("Category", "Unit")]
    [InlineData(TicketStatus.New)]
    [InlineData(TicketStatus.InProgress)]
    [InlineData(TicketStatus.Solved)]
    [InlineData(null)]
    public async Task SearchTicketsAsync_Should_ReturnOnlySelectedStatus(
    TicketStatus? selectedStatus)
    {
        // Arrange
        var customer = new Customer(
            Guid.NewGuid(), "North Office", "contact@north.example");

        var newTicket = new Ticket(
            Guid.NewGuid(), "New ticket", "Test description",
            TicketPriority.Normal, customer.Id);

        var inProgressTicket = new Ticket(
            Guid.NewGuid(), "Ongoing work", "Test description",
            TicketPriority.Normal, customer.Id);

        var solvedTicket = new Ticket(
            Guid.NewGuid(), "Completed work", "Test description",
            TicketPriority.Normal, customer.Id);

        var adminId = Guid.NewGuid();

        inProgressTicket.SetAdminId(adminId);
        inProgressTicket.SetStatus(TicketStatus.InProgress);

        solvedTicket.SetAdminId(adminId);
        solvedTicket.SetStatus(TicketStatus.Solved);

        var repository = Substitute.For<ITicketRepository>();

        repository.GetAll().Returns(
            Task.FromResult(new List<Ticket>
            {
            solvedTicket, newTicket, inProgressTicket
            }));

        var customers = Substitute.For<ICustomerService>();

        customers.GetAllCustomersAsync().Returns(
            Task.FromResult(new GetAllCustomersResult(
                true, new List<Customer> { customer }, null)));

        var service = new TicketService(
            repository,
            Substitute.For<IAdminService>(),
            customers);

        // Act
        var result = await service.SearchTicketsAsync(
            new SearchTicketRequest(string.Empty, selectedStatus));

        // Assert
        Assert.True(result.Succeeded, result.ErrorMessage);

        if (selectedStatus is null)
        {
            Assert.Equal(3, result.Tickets.Count);

            Assert.Contains(result.Tickets, t => t.Id == newTicket.Id);
            Assert.Contains(result.Tickets, t => t.Id == inProgressTicket.Id);
            Assert.Contains(result.Tickets, t => t.Id == solvedTicket.Id);
        }
        else
        {
            var foundTicket = Assert.Single(result.Tickets);
            Assert.Equal(selectedStatus.Value, foundTicket.Status);
        }
    }

    [Theory]
    [Trait("Category", "Unit")]
    [InlineData("PRINTER")]
    [InlineData("NORTH")]
    public async Task SearchTicketsAsync_Should_CombineSearchAndStatusFilter(
    string searchText)
    {
        // Arrange
        var matchingCustomer = new Customer(
            Guid.NewGuid(), "North Office", "contact@north.example");

        var otherCustomer = new Customer(
            Guid.NewGuid(), "Sample Store", "contact@store.example");

        var expectedTicket = new Ticket(
            Guid.NewGuid(), "Printer setup", "Configure a printer",
            TicketPriority.Normal, matchingCustomer.Id);

        var wrongStatusTicket = new Ticket(
            Guid.NewGuid(), "Printer repaired", "Printer works again",
            TicketPriority.Normal, matchingCustomer.Id);

        wrongStatusTicket.SetAdminId(Guid.NewGuid());
        wrongStatusTicket.SetStatus(TicketStatus.Solved);

        var unrelatedTicket = new Ticket(
            Guid.NewGuid(), "Email problem", "Cannot send email",
            TicketPriority.Normal, otherCustomer.Id);

        var repository = Substitute.For<ITicketRepository>();

        repository.GetAll().Returns(
            Task.FromResult(new List<Ticket>
            {
            wrongStatusTicket, unrelatedTicket, expectedTicket
            }));

        var customers = Substitute.For<ICustomerService>();

        customers.GetAllCustomersAsync().Returns(
            Task.FromResult(new GetAllCustomersResult(
                true,
                new List<Customer> { matchingCustomer, otherCustomer },
                null)));

        var service = new TicketService(
            repository,
            Substitute.For<IAdminService>(),
            customers);

        // Act
        var result = await service.SearchTicketsAsync(
            new SearchTicketRequest(searchText, TicketStatus.New));

        // Assert
        Assert.True(result.Succeeded, result.ErrorMessage);

        var foundTicket = Assert.Single(result.Tickets);
        Assert.Equal(expectedTicket.Id, foundTicket.Id);
    }
}