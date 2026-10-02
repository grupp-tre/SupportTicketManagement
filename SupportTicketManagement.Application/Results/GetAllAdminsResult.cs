using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Results;

public record GetAllAdminsResult
(
    bool Succeeded,
    IReadOnlyList<Admin> Admins,
    string? ErrorMessage
);
