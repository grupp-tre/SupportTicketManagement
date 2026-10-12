using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Presentation.ViewModels;

public record TicketStatusOption
(
    string Label,
    TicketStatus? Status
);