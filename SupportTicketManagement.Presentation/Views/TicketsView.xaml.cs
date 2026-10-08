using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class TicketsView : Page
{
    private async void TicketsView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
    TicketsViewModel ViewModel { get; }

    public TicketsView(TicketsViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
        
        Loaded += TicketsView_Loaded;
    }
}
