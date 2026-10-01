using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record UpdateCustomerResult
(
  bool Succeeded,
  Customer? Customer,
  string? ErrorMessage
);
