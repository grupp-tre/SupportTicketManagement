using System;

namespace SupportTicketManagement.Presentation.Models;

public record TicketRow
(
    Guid Id,
    string Title,
    string CustomerName,
    string StatusText,
    string PriorityText
);
