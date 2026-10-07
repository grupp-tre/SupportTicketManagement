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
    }
}
