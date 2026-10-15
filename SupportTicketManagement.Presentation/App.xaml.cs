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

        services.AddTransient<ICustomerRepository, JsonFileCustomerRepository>();
        services.AddTransient<ICustomerService, CustomerService>();

        services.AddTransient<ITicketRepository, JsonFileTicketRepository>();
        services.AddTransient<ITicketService, TicketService>();

        services.AddTransient<IAdminRepository, JsonFileAdminRepository>();
        services.AddTransient<IAdminService, AdminService>();

        services.AddTransient<MainViewModel>();
        services.AddTransient<MainWindow>();

        services.AddTransient<OverviewViewModel>();
        services.AddTransient<OverviewView>();

        services.AddTransient<CustomersViewModel>();
        services.AddTransient<CustomersView>();

        services.AddTransient<CreateCustomerViewModel>();
        services.AddTransient<CreateCustomer>();

        services.AddTransient<TicketsViewModel>();
        services.AddTransient<TicketsView>();
        services.AddTransient<TicketDetailsViewModel>();
        services.AddTransient<TicketDetailsView>();
        services.AddTransient<TicketRegistrationViewModel>();
        services.AddTransient<TicketRegistrationView>();

        _services = services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = _services.GetRequiredService<MainWindow>();

        _window.Activate();
    }
}
