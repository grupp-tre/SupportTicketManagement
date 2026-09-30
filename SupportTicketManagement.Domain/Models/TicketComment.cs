namespace SupportTicketManagement.Domain.Models;

public class TicketComment(Guid id, string text, DateTimeOffset createdAt)
{
    Guid Id { get; set; } = id;
    string Text { get; set; } = text;
    DateTimeOffset CreatedAt { get; set; } = createdAt;
}
