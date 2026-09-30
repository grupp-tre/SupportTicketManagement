namespace SupportTicketManagement.Application.Requests;

public record AddTicketCommentRequest
(
    Guid TicketId,
    string Comment
);
