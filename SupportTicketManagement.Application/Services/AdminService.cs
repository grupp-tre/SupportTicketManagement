using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;

namespace SupportTicketManagement.Application.Services;

public class AdminService(IAdminRepository adminRepository) : IAdminService
{
    public async Task<bool> AdminExistsAsync(Guid id)
    {
        Admin? admin = await adminRepository.GetById(id);

        return admin is not null;
    }

    public async Task<GetAllAdminsResult> GetAllAdminsAsync()
    {
        IReadOnlyList<Admin> admins = await adminRepository.GetAll();

        return new GetAllAdminsResult(true, admins, null);
    }
}
