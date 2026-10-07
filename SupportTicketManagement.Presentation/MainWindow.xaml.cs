using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.Navigation;
using SupportTicketManagement.Presentation.ViewModels;
using SupportTicketManagement.Presentation.Views;

namespace SupportTicketManagement.Presentation;

public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }

    public MainWindow(MainViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();

        ViewModel.Navigation.PropertyChanged += Navigation_PropertyChanged;
        Closed += MainWindow_Closed;

        AppNavigation.SelectedItem = OverviewNavigationItem;
        ViewModel.ShowOverviewCommand.Execute(null);
    }

    private void AppNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer?.Tag is not string tag)
            return;

        switch (tag)
        {
            case "overview":
                ViewModel.ShowOverviewCommand.Execute(null);
                break;
            case "customers":
                ViewModel.ShowCustomersCommand.Execute(null);
                break;
            case "tickets":
                ViewModel.ShowTicketsCommand.Execute(null);
                break;
            default:
                return;
        }
    }

    private void Navigation_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(INavigationService.CurrentPage))
            UpdateSelectedItem();
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
        ViewModel.Navigation.PropertyChanged -= Navigation_PropertyChanged;
        Closed -= MainWindow_Closed;
    }

    private void UpdateSelectedItem()
    {
        AppNavigation.SelectedItem = ViewModel.Navigation.CurrentPage switch
        {
            OverviewView => OverviewNavigationItem,
            CustomersView => CustomersNavigationItem,
            TicketsView => TicketsNavigationItem,
            _ => OverviewNavigationItem
        };
    }
}
