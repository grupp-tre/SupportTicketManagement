using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Presentation.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class TicketRegistrationViewModel(
    ICustomerService customerService,
    ITicketService ticketService,
    INavigationService navigationService) : ObservableObject
{
    public ObservableCollection<Customer> Customers { get; } = [];

    public IReadOnlyList<TicketPriority> Priorities { get; } =
        Enum.GetValues<TicketPriority>();

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial Customer? SelectedCustomer { get; set; }

    [ObservableProperty]
    public partial TicketPriority SelectedPriority { get; set; }
        = TicketPriority.Normal;

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        Message = string.Empty;
        Customers.Clear();

        try
        {
            var result = await customerService.GetAllCustomersAsync();

            if (!result.Succeeded)
            {
                Message = result.ErrorMessage ?? "Could not load customers.";
                return;
            }

            foreach (var customer in result.Customers)
            {
                Customers.Add(customer);
            }

            if (Customers.Count == 0)
            {
                Message = "No registered customers.";
            }
        }
        catch (Exception ex)
        {
            Message = $"Could not load customers: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task RegisterTicketAsync()
    {
        Message = string.Empty;

        if (string.IsNullOrWhiteSpace(Title))
        {
            Message = "Title is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            Message = "Description is required.";
            return;
        }

        if (SelectedCustomer is null)
        {
            Message = "Please select a customer.";
            return;
        }

        var request = new AddTicketRequest(
            Title,
            Description,
            SelectedCustomer.Id,
            SelectedPriority);

        try
        {
            var result = await ticketService.AddTicketAsync(request);

            if (!result.Succeeded)
            {
                Message = result.ErrorMessage ?? "Could not create ticket.";
                return;
            }

            Message = "Ticket created successfully.";

            Title = string.Empty;
            Description = string.Empty;
            SelectedCustomer = null;
            SelectedPriority = TicketPriority.Normal;
        }
        catch (Exception ex)
        {
            Message = $"Could not create ticket: {ex.Message}";
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        navigationService.Navigate(AppPage.Tickets);
    }
}