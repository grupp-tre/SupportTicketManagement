using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Application.Requests;

public class AddTicketRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public TicketPriority Priority { get; set; }
}