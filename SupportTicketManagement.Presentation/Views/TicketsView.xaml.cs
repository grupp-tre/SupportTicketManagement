using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

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
}
