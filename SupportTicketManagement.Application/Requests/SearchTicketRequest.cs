using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Application.Requests;

public record SearchTicketRequest
    (
    string? SearchText,
    TicketStatus? Status
    );