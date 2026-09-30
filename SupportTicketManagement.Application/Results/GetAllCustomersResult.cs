using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetAllCustomersResult

(

    bool Succeeded,

    IReadOnlyList<Customer> Customers,

    string? ErrorMessage
);
