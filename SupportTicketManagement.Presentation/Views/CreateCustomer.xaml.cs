
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class CreateCustomer : Page 
{
    public CreateCustomerViewModel ViewModel { get; }
    public CreateCustomer(CreateCustomerViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
   
}

