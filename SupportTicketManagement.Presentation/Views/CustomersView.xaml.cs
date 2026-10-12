using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;
using SupportTicketManagement.Domain.Models;

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

    private async void EditCustomer_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Customer customer })
            await ViewModel.EditCustomerCommand.ExecuteAsync(customer);
    }
}
