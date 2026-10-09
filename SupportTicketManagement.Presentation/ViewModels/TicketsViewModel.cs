using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public record TicketRow
(
    string Title,
    string CustomerName,
    string StatusText,
    string PriorityText
);

public partial class TicketsViewModel(ITicketService ticketService, ICustomerService customerService) : ObservableObject
{
    public ObservableCollection<TicketRow> Tickets { get; } = [];

    public IReadOnlyList<string> StatusOptions { get; } = ["All statuses", "New", "In progress", "Solved"];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    public partial bool IsLoading { get; set; }

    public bool IsNotLoading => !IsLoading;

    [ObservableProperty]
    public partial string Message { get; set; } = "";

    [RelayCommand]
    private async Task LoadTicketsAsync()
    {
        IsLoading = true;
        Message = "";
        Tickets.Clear();

        try
        {
            var ticketResult = await ticketService.GetAllTicketsAsync();

            if (!ticketResult.Succeeded)
            {
                Message = ticketResult.ErrorMessage ?? "Could not load tickets.";
                return;
            }

            var customerResult = await customerService.GetAllCustomersAsync();

            if (!customerResult.Succeeded)
            {
                Message = customerResult.ErrorMessage ?? "Could not load customers.";
                return;
            }

            var customerNames = customerResult.Customers.ToDictionary
            (
                customer => customer.Id,
                customer => customer.Name
            );

            foreach (var ticket in ticketResult.Tickets)
            {
                Tickets.Add(new TicketRow(
                    ticket.Title,
                    customerNames.GetValueOrDefault(
                        ticket.CustomerId, "Unknown customer"),
                    ticket.Status == TicketStatus.InProgress
                        ? "In progress"
                        : ticket.Status.ToString(),
                    ticket.Priority.ToString()));
            }

            if (Tickets.Count == 0)
                Message = "No tickets yet.";
        }
        catch (Exception ex)
        {
            Message = $"Could not load tickets: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
