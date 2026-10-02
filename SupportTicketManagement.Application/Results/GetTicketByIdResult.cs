using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetTicketByIdResult
(
    bool Succeeded,
    Ticket? Ticket,
    string? ErrorMessage
);
