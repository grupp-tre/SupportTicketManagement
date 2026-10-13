using NSubstitute;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;
using Xunit;

namespace SupportTicketManagement.Tests.Application.UnitTests;

public class TicketServiceTests
{
    [Fact]
    [Trait("Category", "Unit")]
    public async Task UpdateTicketAsync_Should_RejectInProgressWhenNoAdministratorIsAssigned()
    {
        // Arrange
        var ticket = new Ticket(Guid.NewGuid(), "Printer offline", "Cannot print",
            TicketPriority.Normal, Guid.NewGuid());
        var repository = Substitute.For<ITicketRepository>();
        repository.GetById(ticket.Id).Returns(Task.FromResult<Ticket?>(ticket));
        var service = new TicketService(repository, Substitute.For<IAdminService>(),
            Substitute.For<ICustomerService>());
        var request = new UpdateTicketRequest(ticket.Id, ticket.Title, ticket.Description,
            null, ticket.Priority, TicketStatus.InProgress);

        // Act
        var result = await service.UpdateTicketAsync(request);

        // Assert
        Assert.False(result.Succeeded);
        Assert.False(string.IsNullOrWhiteSpace(result.ErrorMessage));
        Assert.Equal(TicketStatus.New, ticket.Status);
        await repository.DidNotReceive().Update(Arg.Any<Ticket>());
    }

    [Fact]
    [Trait("Category", "Unit")]
    public async Task UpdateTicketAsync_Should_AllowInProgressWhenAdministratorIsAssigned()
    {
        // Arrange
        var ticket = new Ticket(Guid.NewGuid(), "Printer offline", "Cannot print",
            TicketPriority.Normal, Guid.NewGuid());
        var adminId = Guid.NewGuid();
        var repository = Substitute.For<ITicketRepository>();
        repository.GetById(ticket.Id).Returns(Task.FromResult<Ticket?>(ticket));
        repository.Update(ticket).Returns(Task.FromResult(true));
        var admins = Substitute.For<IAdminService>();
        admins.AdminExistsAsync(adminId).Returns(Task.FromResult(true));
        var service = new TicketService(repository, admins, Substitute.For<ICustomerService>());
        var request = new UpdateTicketRequest(ticket.Id, ticket.Title, ticket.Description,
            adminId, ticket.Priority, TicketStatus.InProgress);

        // Act
        var result = await service.UpdateTicketAsync(request);

        // Assert
        Assert.True(result.Succeeded);
        await repository.Received().Update(
            Arg.Is<Ticket>(savedTicket => savedTicket.Id == ticket.Id
                && savedTicket.AdminId == adminId
                && savedTicket.Status == TicketStatus.InProgress));
    }
}
