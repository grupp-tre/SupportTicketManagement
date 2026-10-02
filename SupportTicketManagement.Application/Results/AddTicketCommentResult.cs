namespace SupportTicketManagement.Application.Results;

public record AddTicketCommentResult
(
    bool Succeeded,
    string? ErrorMessage
);
