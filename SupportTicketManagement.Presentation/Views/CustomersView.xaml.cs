using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class CustomersView : Page
{
   public CustomersViewModel ViewModel { get; }

    public CustomersView(CustomersViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadCustomersAsync();
    }
}
