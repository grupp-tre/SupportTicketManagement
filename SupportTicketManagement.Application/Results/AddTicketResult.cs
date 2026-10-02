using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record AddTicketResult
(
    bool Succeeded, 
    Ticket? Ticket,
    string? ErrorMessage
);