using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Presentation.Navigation;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class MainViewModel(INavigationService navigationService) : ObservableObject
{
    public INavigationService Navigation { get; } = navigationService;

    [RelayCommand]
    private void ShowOverview()
    {
        Navigation.Navigate(AppPage.Overview);
    }

    [RelayCommand]
    private void ShowCustomers()
    {
        Navigation.Navigate(AppPage.Customers);
    }

    [RelayCommand]
    private void ShowTickets()
    {
        Navigation.Navigate(AppPage.Tickets);
    }
}
