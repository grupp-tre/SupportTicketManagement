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
}