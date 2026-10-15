using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using Xunit;

namespace SupportTicketManagement.Tests.Models;

public class TicketModelTests
{
    [Fact]
    public void Create_Should_SetStatusToNew_WhenTicketIsValid()
    {
        var ticketId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var ticket = new Ticket(
            ticketId,
            "Printer not working",
            "The printer does not respond.",
            TicketPriority.Normal,
            customerId);

        Assert.Equal(TicketStatus.New, ticket.Status);
    }
}