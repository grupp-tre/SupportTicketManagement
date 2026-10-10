using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class TicketDetailsView : Page
{
    public TicketDetailsViewModel ViewModel { get; }

    public TicketDetailsView(TicketDetailsViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();
    }
}
