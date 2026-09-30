using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Application.Requests;

public record AddTicketRequest
(
    string Title, 
    string Description, 
    Guid CustomerId, 
    TicketPriority Priority
);