using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record AddCustomerResult

( 
    bool Succeeded,

    Customer? Customer,

    string? ErrorMessage

);
