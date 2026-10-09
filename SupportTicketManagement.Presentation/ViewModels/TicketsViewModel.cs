using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    public TicketStatusOption[] StatusOptions { get; } =
        [
        new("All statuses", null),
        new("New", TicketStatus.New),
        new("In progress", TicketStatus.InProgress),
        new("Solved", TicketStatus.Solved)
        ];

    public TicketsViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;

        SelectedStatusOption = StatusOptions[0];
    }

    public ObservableCollection<Ticket> Tickets { get; } = [];

    [ObservableProperty]
    public partial string? ErrorMessage { get; set; }
    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;
    [ObservableProperty]
    public partial TicketStatusOption? SelectedStatusOption { get; set; }
    [ObservableProperty]
    public partial string? EmptyMessage { get; set; }

    [RelayCommand]
    public async Task LoadAsync()
    {
        ErrorMessage = null;
        EmptyMessage = null;

        var request = new SearchTicketRequest(SearchText, SelectedStatusOption?.Status);

        var result = await _ticketService.SearchTicketsAsync(request);

        Tickets.Clear();

        if (!result.Succeeded)
        {
            ErrorMessage = result.ErrorMessage
                ?? "Could not load tickets.";
            return;
        }

        foreach (var ticket in result.Tickets)
        {
            Tickets.Add(ticket);
        }
        if (Tickets.Count == 0)
        {
            EmptyMessage = "No matching tickets found.";
        }
    }
}
