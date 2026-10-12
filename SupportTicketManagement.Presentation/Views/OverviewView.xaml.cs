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
        Loaded += OverviewView_Loaded;
    }

    private async void OverviewView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAsync();
    }
}
