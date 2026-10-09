using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Domain.Repositories;

public interface ICustomerRepository
{
    Task<bool> Create(Customer customer);
    Task<List<Customer>> GetAll();
    Task<Customer?> GetById(Guid id);
    Task<bool> Update(Customer customer);
}
