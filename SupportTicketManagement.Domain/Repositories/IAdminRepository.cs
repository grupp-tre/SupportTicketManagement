using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Domain.Repositories;

public interface IAdminRepository
{
    Task<Admin?> GetById(Guid id);
    Task<List<Admin>> GetAll();
}
