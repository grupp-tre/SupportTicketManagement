using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetAllAdminsResult
(
    bool Success,
    IReadOnlyList<Admin> Admins,
    string? ErrorMessage
);
