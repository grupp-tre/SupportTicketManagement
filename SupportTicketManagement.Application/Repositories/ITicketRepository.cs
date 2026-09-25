using SupportTicketManagement.Domain.Models;

namespace SupportTicketManagement.Application.Repositories;

public interface ITicketRepository
{
    Task<bool> Create(Ticket ticket);
    Task<IReadOnlyList<Ticket>> GetAll();
    Task<bool> Update(Ticket ticket);
}
