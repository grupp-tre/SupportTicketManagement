using CommunityToolkit.Mvvm.ComponentModel;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Presentation.Navigation;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class OverviewViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    private readonly INavigationService _navigationService;

    public OverviewViewModel(ITicketService ticketService, INavigationService navigationService)
    {
        _ticketService = ticketService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void ShowNewTickets()
    {
        _navigationService.NavigateToTickets(TicketStatus.New);
    }
   
    [RelayCommand]
    private void ShowInProgressTickets()
    {
        _navigationService.NavigateToTickets(TicketStatus.InProgress);
    }

    [RelayCommand]
    private void ShowSolvedTickets()
    {
        _navigationService.NavigateToTickets(TicketStatus.Solved);
    }

    [ObservableProperty]
    public partial int NewCount { get; set; }
    [ObservableProperty]
    public partial int OngoingCount { get; set; }
    [ObservableProperty]
    public partial int SolvedCount { get; set; }
    [ObservableProperty]
    public partial string? ErrorMessage { get; set; }

    public async Task LoadAsync()
    {
        ErrorMessage = null;

        var result = await _ticketService.GetTicketSummaryAsync();

        if (!result.Succeeded)
        {
            NewCount = 0;
            OngoingCount = 0;
            SolvedCount = 0;
            ErrorMessage = result.ErrorMessage
                ?? "Could not load ticket summary.";
            return;
        }

        NewCount = result.NewCount;
        OngoingCount = result.InProgressCount;
        SolvedCount = result.SolvedCount;
    }
   
}
