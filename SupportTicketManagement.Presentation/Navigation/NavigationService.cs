using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Domain.Enums;
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
            AppPage.CreateCustomer => services.GetRequiredService<CreateCustomer>(),
            AppPage.Tickets => services.GetRequiredService<TicketsView>(), 
            AppPage.TicketDetails => services.GetRequiredService<TicketDetailsView>(),
            AppPage.TicketRegistration => services.GetRequiredService<TicketRegistrationView>(),
            _ => throw new ArgumentOutOfRangeException(nameof(page))
        };
    }

    public void NavigateToTickets(TicketStatus status)
    {
        var page = services.GetRequiredService<TicketsView>();

        page.ViewModel.ApplyStatusFilter(status);

        CurrentPage = page;
    }
}
