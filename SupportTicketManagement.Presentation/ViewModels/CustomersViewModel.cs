using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Presentation.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace SupportTicketManagement.Presentation.ViewModels;

public partial class CustomersViewModel(ICustomerService customerService, INavigationService navigationService) : ObservableObject
{
    public ObservableCollection<Customer> Customers { get; } = [];

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;

    public async Task LoadCustomersAsync()
    {
        var customersResult = await customerService.GetAllCustomersAsync();

        if (!customersResult.Succeeded)
        {
            Message = customersResult.ErrorMessage ?? "The customers could not be loaded.";
            return;
        }

        Message = string.Empty;
        Customers.Clear();

        foreach (var customer in customersResult.Customers)
            Customers.Add(customer);

        if (Customers.Count == 0)
        {
            Message = "No customers registered yet.";
        }

    }

    [RelayCommand]
    private void OpenCreateCustomer()
    {
        navigationService.Navigate(AppPage.CreateCustomer);
    }
}



