using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record UpdateTicketResult
(
    bool Succeeded,
    Ticket? Ticket,
    string? ErrorMessage
);
