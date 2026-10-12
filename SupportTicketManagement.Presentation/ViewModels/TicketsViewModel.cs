using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Presentation.Models;
using SupportTicketManagement.Presentation.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;
    private readonly ICustomerService _customerService;
    private readonly INavigationService _navigationService;
    private readonly TicketDetailsViewModel _ticketDetailsViewModel;

    public TicketsViewModel(
        ITicketService ticketService,
        ICustomerService customerService,
        INavigationService navigationService,
        TicketDetailsViewModel ticketDetailsViewModel)
    {
        _ticketService = ticketService;
        _customerService = customerService;
        _navigationService = navigationService;
        _ticketDetailsViewModel = ticketDetailsViewModel;

        SelectedStatusOption = StatusOptions[0];
    }

    public ObservableCollection<TicketRow> Tickets { get; } = [];

    public TicketStatusOption[] StatusOptions { get; } =
    [
        new("All statuses", null),
        new("New", TicketStatus.New),
        new("In progress", TicketStatus.InProgress),
        new("Solved", TicketStatus.Solved)
    ];

    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial TicketStatusOption? SelectedStatusOption { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))]
    public partial bool IsLoading { get; set; }

    public bool IsNotLoading => !IsLoading;

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;

    [RelayCommand]
    private async Task LoadTicketsAsync()
    {
        IsLoading = true;
        Message = string.Empty;
        Tickets.Clear();

        try
        {
            var request = new SearchTicketRequest(
                SearchText,
                SelectedStatusOption?.Status);

            var ticketResult =
                await _ticketService.SearchTicketsAsync(request);

            if (!ticketResult.Succeeded)
            {
                Message = ticketResult.ErrorMessage
                    ?? "Could not load tickets.";
                return;
            }

            var customerResult =
                await _customerService.GetAllCustomersAsync();

            if (!customerResult.Succeeded)
            {
                Message = customerResult.ErrorMessage
                    ?? "Could not load customers.";
                return;
            }

            var customerNames = customerResult.Customers.ToDictionary(
                customer => customer.Id,
                customer => customer.Name);

            foreach (var ticket in ticketResult.Tickets)
            {
                Tickets.Add(new TicketRow(
                    ticket.Id,
                    ticket.Title,
                    customerNames.GetValueOrDefault(
                        ticket.CustomerId, "Unknown customer"),
                    ticket.Status == TicketStatus.InProgress
                        ? "In progress"
                        : ticket.Status.ToString(),
                    ticket.Priority.ToString()));
            }

            if (Tickets.Count == 0)
            {
                Message = "No matching tickets found.";
            }
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

    [RelayCommand]
    private async Task OpenTicketAsync(TicketRow row)
    {
        IsLoading = true;

        try
        {
            await _ticketDetailsViewModel.LoadTicketAsync(row.Id);
            _navigationService.Navigate(AppPage.TicketDetails);
        }
        finally
        {
            IsLoading = false;
        }
    }
}