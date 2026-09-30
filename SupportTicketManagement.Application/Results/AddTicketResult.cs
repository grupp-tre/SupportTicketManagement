using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record AddTicketResult
(
    bool Success, 
    Ticket? Ticket,
    string? ErrorMessage
);