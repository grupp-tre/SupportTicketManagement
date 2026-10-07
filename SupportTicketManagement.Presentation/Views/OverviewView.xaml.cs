using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation.Views;

public sealed partial class OverviewView : Page
{
    OverviewViewModel ViewModel { get; }

    public OverviewView(OverviewViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
