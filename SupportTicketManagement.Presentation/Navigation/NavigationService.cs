using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Presentation.Views;
using System;

namespace SupportTicketManagement.Presentation.Navigation;

public partial class NavigationService(IServiceProvider services) : ObservableObject, INavigationService
{
    [ObservableProperty]
    public partial Page? CurrentPage { get; private set; } 

    public void Navigate(AppPage page)
    {
        CurrentPage = page switch
        {
            AppPage.Overview => services.GetRequiredService<OverviewView>(), 
            AppPage.Customers => services.GetRequiredService<CustomersView>(), 
            AppPage.Tickets => services.GetRequiredService<TicketsView>(), 
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };
    }
}
