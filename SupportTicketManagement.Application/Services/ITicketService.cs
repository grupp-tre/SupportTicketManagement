using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;

namespace SupportTicketManagement.Application.Services;

public interface ITicketService
{
    Task<AddTicketResult> AddTicket(AddTicketRequest addTicketRequest);
    Task<GetAllTicketsResult> GetAllTickets();
    Task<UpdateTicketResult> UpdateTicket(UpdateTicketRequest updateTicketRequest);
}
