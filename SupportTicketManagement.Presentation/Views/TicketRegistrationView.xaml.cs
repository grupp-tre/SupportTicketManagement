using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class TicketRegistrationView : Page
{
    public TicketRegistrationViewModel ViewModel { get; }

    public TicketRegistrationView(
        TicketRegistrationViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();

        Loaded += TicketRegistrationView_Loaded;
    }

    private async void TicketRegistrationView_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        await ViewModel.LoadCustomersCommand.ExecuteAsync(null);
    }
}