using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetCustomerByIdResult
(
    bool Succeeded,
    Customer? Customer,
    string? ErrorMessage
);
