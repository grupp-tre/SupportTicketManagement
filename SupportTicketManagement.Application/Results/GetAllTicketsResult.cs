using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetAllTicketsResult
(
    bool Succeeded,
    IReadOnlyList<Ticket> Tickets,
    string? ErrorMessage
);
