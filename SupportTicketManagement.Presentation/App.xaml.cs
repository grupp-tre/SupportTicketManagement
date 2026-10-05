using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SupportTicketManagement.Domain.Repositories;
using SupportTicketManagement.Infrastructure.Repositories;
using SupportTicketManagement.Application.Services;
using System;

namespace SupportTicketManagement.Presentation;

public partial class App : Microsoft.UI.Xaml.Application
{
    private readonly IServiceProvider _services;
    private Window? _window;
    public App()
    {
        InitializeComponent();

        var services = new ServiceCollection();

        services.AddSingleton<ICustomerRepository, JsonFileCustomerRepository>();
        services.AddSingleton<ICustomerService, CustomerService>();

        _services = services.BuildServiceProvider();
    }
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}
