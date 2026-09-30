using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Domain.Repositories;

public interface ITicketRepository
{
    Task<bool> Create(Ticket ticket);
    Task<Ticket?> GetById(Guid id);
    Task<IReadOnlyList<Ticket>> GetAll();
    Task<bool> Update(Ticket ticket);
}
