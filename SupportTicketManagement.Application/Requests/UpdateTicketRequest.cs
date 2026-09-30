using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Application.Requests;

public record UpdateTicketRequest
(
    Guid Id,

    string Title, 

    string Description, 

    Guid? AdministratorId, 

    TicketPriority Priority, 

    TicketStatus Status 
);
