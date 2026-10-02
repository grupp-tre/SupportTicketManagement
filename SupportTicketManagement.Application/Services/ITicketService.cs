using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;

namespace SupportTicketManagement.Application.Services;

public interface ITicketService
{
    Task<AddTicketResult> AddTicketAsync(AddTicketRequest addTicketRequest);
    Task<GetTicketByIdResult> GetTicketByIdAsync(Guid ticketId);
    Task<GetAllTicketsResult> GetAllTicketsAsync();
    Task<UpdateTicketResult> UpdateTicketAsync(UpdateTicketRequest updateTicketRequest);
    Task<AddTicketCommentResult> AddTicketCommentAsync(AddTicketCommentRequest addTicketCommentRequest);
    Task<GetAllTicketsResult> SearchTicketsAsync(SearchTicketRequest request);
}
