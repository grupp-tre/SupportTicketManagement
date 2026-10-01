namespace SupportTicketManagement.Application.Requests;

public record UpdateCustomerRequest
(
    Guid CustomerId,
    string CustomerName,
    string EmailAddress
);

