using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetTicketByIdResult
(
    bool Success,
    Ticket? Ticket,
    string? ErrorMessage
);
