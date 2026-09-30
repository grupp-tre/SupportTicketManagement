namespace SupportTicketManagement.Application.Results;

public record AddTicketCommentResult
(
    bool Success,
    string? ErrorMessage
);
