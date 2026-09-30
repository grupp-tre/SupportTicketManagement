using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;

namespace SupportTicketManagement.Application.Services;

public class TicketService(ITicketRepository ticketRepository, IAdminService adminService) : ITicketService
{
    public async Task<AddTicketResult> AddTicketAsync(AddTicketRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<GetAllTicketsResult> GetAllTicketsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<GetTicketByIdResult> GetTicketByIdAsync(Guid id)
    {
        var ticket = await ticketRepository.GetById(id);

        if (ticket is null)
            return new GetTicketByIdResult(false, null, $"Could not find ticket with Id: '{id}'.");

        return new GetTicketByIdResult(true, ticket, null);
    }

    public async Task<UpdateTicketResult> UpdateTicketAsync(UpdateTicketRequest request)
    {
        GetTicketByIdResult result = await GetTicketByIdAsync(request.Id);

        if (!result.Success || result.Ticket is null)
            return new UpdateTicketResult(false, null, result.ErrorMessage);

        Ticket ticket = result.Ticket;

        ticket.Title = request.Title;
        ticket.Description = request.Description;
        if (ticket.AdministratorId != request.AdministratorId)
        {
            if (request.AdministratorId is not null && !await adminService.AdminExistsAsync(request.AdministratorId.Value))
                throw new KeyNotFoundException($"Could not find admin with Id: '{request.AdministratorId}'.");

            ticket.AdministratorId = request.AdministratorId;
        }
        ticket.Priority = request.Priority;
        ticket.Status = request.Status;

        await ticketRepository.Update(ticket);

        return new UpdateTicketResult(true, ticket, null);
    }

    public async Task<AddTicketCommentResult> AddTicketCommentAsync(AddTicketCommentRequest request)
    {
        TicketComment ticketComment;

        try
        {
            ticketComment = CreateTicketComment(request.Comment);
        }
        catch (Exception ex)
        {
            return new AddTicketCommentResult(false, ex.Message); 
        }

        GetTicketByIdResult result = await GetTicketByIdAsync(request.TicketId);

        if (!result.Success || result.Ticket is null)
            return new AddTicketCommentResult(false, result.ErrorMessage);

        Ticket ticket = result.Ticket;

        ticket.Comments.Add(ticketComment);

        await ticketRepository.Update(ticket);

        return new AddTicketCommentResult(true, null);
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
