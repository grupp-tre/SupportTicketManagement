namespace SupportTicketManagement.Domain.Models;

public class Admin
{
    Guid Id { get; set; }
    string FirstName { get; set; } = string.Empty;
    string LastName { get; set; } = string.Empty;
    string Email { get; set; } = string.Empty;
}
