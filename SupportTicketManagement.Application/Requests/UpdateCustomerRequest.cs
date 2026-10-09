namespace SupportTicketManagement.Application.Requests;

public record UpdateCustomerRequest
(
    Guid Id,
    string Name,
    string EmailAddress
);

