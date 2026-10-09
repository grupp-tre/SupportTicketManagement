using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SupportTicketManagement.Domain.Repositories;
using SupportTicketManagement.Infrastructure.Repositories;
using SupportTicketManagement.Application.Services;
using System;
using SupportTicketManagement.Presentation.Navigation;
using SupportTicketManagement.Presentation.Views;
using SupportTicketManagement.Presentation.ViewModels;

namespace SupportTicketManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    private readonly IServiceProvider _services;
    private Window? _window;

    public App()
    {
        InitializeComponent();

        var services = new ServiceCollection();

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<ICustomerRepository, JsonFileCustomerRepository>();
        services.AddSingleton<ICustomerService, CustomerService>();

        services.AddSingleton<ITicketRepository, JsonFileTicketRepository>();
        services.AddSingleton<ITicketService, TicketService>();

        services.AddSingleton<IAdminRepository, JsonFileAdminRepository>();
        services.AddSingleton<IAdminService, AdminService>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();

        services.AddSingleton<OverviewViewModel>();
        services.AddSingleton<OverviewView>();

        services.AddSingleton<CustomersViewModel>();
        services.AddSingleton<CustomersView>();

        services.AddSingleton<TicketsViewModel>();
        services.AddSingleton<TicketsView>();
        services.AddSingleton<TicketDetailsViewModel>();
        services.AddTransient<TicketDetailsView>();

        _services = services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = _services.GetRequiredService<MainWindow>();

        _window.Activate();
    }
}
