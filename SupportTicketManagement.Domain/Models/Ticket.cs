using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Domain.Models;

public class Ticket
{
    public Guid Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public List<TicketComment> Comments { get; set; } = [];
    public Guid CustomerId { get; set; }
    public Guid? AdministratorId { get; set; } = null;
    public DateTimeOffset CreatedAt { get; set; }
}
