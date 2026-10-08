using CommunityToolkit.Mvvm.ComponentModel;
using SupportTicketManagement.Application.Services;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class OverviewViewModel : ObservableObject
{
    private readonly ITicketService _ticketService;

    public OverviewViewModel(ITicketService ticketService)
    {
        _ticketService = ticketService;
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
        OngoingCount = result.OngoingCount;
        SolvedCount = result.SolvedCount;
    }
   
}
