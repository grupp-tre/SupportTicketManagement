using SupportTicketManagement.Application.Results;

namespace SupportTicketManagement.Application.Services;

public interface IAdminService
{
    Task<GetAllAdminsResult> GetAllAdminsAsync();
    Task<bool> AdminExistsAsync(Guid id);
}
