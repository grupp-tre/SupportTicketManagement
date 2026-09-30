using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record UpdateTicketResult
(
    bool Success,
    string? Message,
    Ticket? Ticket 
);
