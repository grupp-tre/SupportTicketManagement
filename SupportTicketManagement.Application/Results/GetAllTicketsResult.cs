using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetAllTicketsResult
(
    bool Success,
    IReadOnlyList<Ticket> Tickets,
    string? ErrorMessage
);
