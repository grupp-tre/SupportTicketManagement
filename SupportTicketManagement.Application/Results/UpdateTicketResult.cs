using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record UpdateTicketResult
(
    bool Success,
    Ticket? Ticket,
    string? ErrorMessage
);
