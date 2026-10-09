using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;
using SupportTicketManagement.Presentation.Models;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class TicketsView : Page
{
    TicketsViewModel ViewModel { get; }

    public TicketsView(TicketsViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();

        Loaded += TicketsView_Loaded;
    }

    private async void TicketsView_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadTicketsCommand.ExecuteAsync(null);
    }

    private async void OpenTicket_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: TicketRow row })
            await ViewModel.OpenTicketCommand.ExecuteAsync(row);
    }
}
