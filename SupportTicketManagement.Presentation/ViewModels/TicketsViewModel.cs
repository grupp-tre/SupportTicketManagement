using CommunityToolkit.Mvvm.ComponentModel;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    public TicketsViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public ObservableCollection<Ticket> Tickets { get; } = [];

    [ObservableProperty]
    public partial string? ErrorMessage { get; set; }

    public async Task LoadAsync()
    {
        ErrorMessage = null;

        var result = await _ticketService.GetAllTicketsAsync();

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
    }
}
