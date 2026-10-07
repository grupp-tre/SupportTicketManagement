using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;

namespace SupportTicketManagement.Application.Services;

public class TicketService(ITicketRepository ticketRepository, IAdminService adminService, ICustomerService customerService) : ITicketService
{
    public async Task<AddTicketResult> AddTicketAsync(AddTicketRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<GetAllTicketsResult> GetAllTicketsAsync()
    {
        try
        {
            List<Ticket> tickets = await ticketRepository.GetAll();

            List<Ticket> sortedTickets = tickets
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return new GetAllTicketsResult(true, sortedTickets, null);
        }
        catch (System.IO.IOException)
        {
            return new GetAllTicketsResult(false, [], "Could not read the ticket file.");
        }
        catch (UnauthorizedAccessException)
        {
            return new GetAllTicketsResult(false, [], "You do not have permission to read the ticket file.");
        }
        catch (System.Text.Json.JsonException)
        {
            return new GetAllTicketsResult(false, [], "The ticket file contains invalid JSON.");
        }
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

        if (!result.Succeeded || result.Ticket is null)
            return new UpdateTicketResult(false, null, result.ErrorMessage);

        Ticket ticket = result.Ticket;

        try
        {
            if (ticket.Title != request.Title)
                ticket.SetTitle(request.Title);

            if (ticket.Description != request.Description)
                ticket.SetDescription(request.Description);

            if (ticket.AdminId != request.AdminId)
                await SetAdminId(request.AdminId, ticket);

            if (ticket.Priority != request.Priority)
                ticket.SetPriority(request.Priority);

            if (ticket.Status != request.Status)
                ticket.SetStatus(request.Status);

            await ticketRepository.Update(ticket);
        }
        catch (Exception ex)
        {
            return new UpdateTicketResult(false, null, ex.Message);
        }

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

        if (!result.Succeeded || result.Ticket is null)
            return new AddTicketCommentResult(false, result.ErrorMessage);

        Ticket ticket = result.Ticket;

        ticket.AddComment(ticketComment);

        await ticketRepository.Update(ticket);

        return new AddTicketCommentResult(true, null);
    }

    private static TicketComment CreateTicketComment(string commentText)
    {
        if (string.IsNullOrWhiteSpace(commentText))
            throw new ArgumentException("A comment can not be empty.");

        Guid ticketId = Guid.NewGuid();

        return new TicketComment(ticketId, commentText);
    }

    private async Task SetAdminId(Guid? adminId, Ticket ticket)
    {
        if (adminId is not null && !await adminService.AdminExistsAsync(adminId.Value))
            throw new KeyNotFoundException($"Could not find admin with Id: '{adminId}'.");

        ticket.SetAdminId(adminId);
    }

    public async Task<GetAllTicketsResult> SearchTicketsAsync(SearchTicketRequest request)
    {
        if (request.Status.HasValue && !Enum.IsDefined(request.Status.Value))
        {
            return new GetAllTicketsResult(false, [], "Invalid ticket filter.");
        }

        var result = await GetAllTicketsAsync();

        if (!result.Succeeded)
        {
            return result;
        }
        var customerResult = await customerService.GetAllCustomersAsync();

        if (!customerResult.Succeeded)
        {
            return new GetAllTicketsResult(false, [], customerResult.ErrorMessage);
        }

        string searchText = request.SearchText?.Trim() ?? string.Empty;
        List<Ticket> matchingTickets = [];

        foreach (var ticket in result.Tickets)
        {
            var customer = customerResult.Customers.FirstOrDefault(
                c => c.CustomerId == ticket.CustomerId);

            bool matchesSearchCustomer = 
                customer is not null && 
                customer.CustomerName.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase);

            bool matchesSearchTitle = ticket.Title.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase);

            bool matchesStatus =
                request.Status is null || ticket.Status == request.Status.Value;

            if ((matchesSearchTitle || matchesSearchCustomer) && matchesStatus)
            {
                matchingTickets.Add(ticket);
            }
        }

        return new GetAllTicketsResult(true, matchingTickets, null);
    }
    public async Task<TicketSummaryResult> GetTicketSummaryAsync()
    {
        var result = await GetAllTicketsAsync();
        
        if (!result.Succeeded)
        {
            return new TicketSummaryResult(false, 0, 0, 0, result.ErrorMessage);
        }

        int newCount = 0;
        int ongoingCount = 0;
        int solvedCount = 0;

        foreach (var ticket in result.Tickets)
        {
            switch (ticket.Status)
            {
                case TicketStatus.New:
                    newCount++;
                    break;
                case TicketStatus.Ongoing:
                    ongoingCount++;
                    break;
                case TicketStatus.Solved:
                    solvedCount++;
                    break;

                default:
                    return new TicketSummaryResult(
                        false, 0, 0, 0, "A ticket has an invalid status.");
            }
        }
        
        return new TicketSummaryResult(
            true, newCount, ongoingCount, solvedCount, null);
    }
}
