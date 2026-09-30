using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Domain.Repositories;

public interface ITicketRepository
{
    Task<bool> Create(Ticket ticket);
    Task<Ticket?> GetById(Guid id);
    Task<List<Ticket>> GetAll();
    Task<bool> Update(Ticket ticket);
}
