using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Domain.Repositories;

public interface ICustomerRepository
{
    Task<bool> Create(Customer customer);
    Task<List<Customer>> GetAll();
    Task<bool> Update(Customer customer);
}
