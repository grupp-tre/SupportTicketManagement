using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class CustomersView : Page
{
    CustomersViewModel ViewModel { get; }

    public CustomersView(CustomersViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
