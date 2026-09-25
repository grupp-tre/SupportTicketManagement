namespace SupportTicketManagement.Domain.Models;

public class TicketComment
{
    Guid Id { get; set; }
    string Text { get; set; } = string.Empty;
    DateTimeOffset CreatedAt { get; set; }
    Guid AuthorId { get; set; }
}
