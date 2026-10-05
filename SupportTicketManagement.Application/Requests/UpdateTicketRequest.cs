using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Application.Requests;

public record UpdateTicketRequest
(
    Guid Id,

    string Title, 

    string Description, 

    Guid? AdminId, 

    TicketPriority Priority, 

    TicketStatus Status 
);
