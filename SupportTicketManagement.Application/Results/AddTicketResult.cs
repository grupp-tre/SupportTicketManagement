using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public class AddTicketResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Ticket? Ticket { get; set; }
}