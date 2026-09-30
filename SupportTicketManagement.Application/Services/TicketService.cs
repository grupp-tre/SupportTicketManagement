using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;

namespace SupportTicketManagement.Application.Services;

public class TicketService(ITicketRepository ticketRepository) : ITicketService
{
    public async Task<AddTicketResult> AddTicketAsync(AddTicketRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<GetAllTicketsResult> GetAllTicketsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<UpdateTicketResult> UpdateTicketAsync(UpdateTicketRequest request)
    {
        try
        {
            Ticket ticket = await GetTicketByIdAsync(request.Id);

            ticket.Title = request.Title;
            ticket.Description = request.Description;
            ticket.AdministratorId = request.AdministratorId;
            ticket.Priority = request.Priority;
            ticket.Status = request.Status;

            await ticketRepository.Update(ticket);

            return new UpdateTicketResult(true, null, ticket);
        }
        catch (Exception ex)
        {
            return new UpdateTicketResult(false, ex.Message, null);
        }
    }

    public async Task<AddTicketCommentResult> AddTicketCommentAsync(AddTicketCommentRequest request)
    {
        try
        {
            TicketComment ticketComment = CreateTicketComment(request.Comment);
            Ticket ticket = await GetTicketByIdAsync(request.TicketId);

            ticket.Comments.Add(ticketComment);

            await ticketRepository.Update(ticket);

            return new AddTicketCommentResult(true, null);
        }
        catch (Exception ex)
        {
            return new AddTicketCommentResult(false, ex.Message); 
        }
    }

    private async Task<Ticket> GetTicketByIdAsync(Guid id)
    {
        var ticket = await ticketRepository.GetById(id);

        if (ticket is null)
            throw new KeyNotFoundException($"Could not find ticket with Id: '{id}'");

        return ticket;
    }

    private static TicketComment CreateTicketComment(string commentText)
    {
        if (string.IsNullOrWhiteSpace(commentText))
            throw new ArgumentException("A comment can not be empty.");

        Guid ticketId = Guid.NewGuid();
        DateTimeOffset createdAt = DateTimeOffset.UtcNow;

        return new TicketComment
        (
            ticketId,
            commentText,
            createdAt
        );
    }
}
