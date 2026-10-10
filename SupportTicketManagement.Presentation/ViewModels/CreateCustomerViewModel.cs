using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Presentation.Navigation;
using System;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class CreateCustomerViewModel(ICustomerService customerService, INavigationService navigationService) : ObservableObject
{
    [ObservableProperty]
    public partial string CustomerName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EmailAddress { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        Message = string.Empty;

        var request = new AddCustomerRequest(CustomerName, EmailAddress);
        var addCustomer = await customerService.AddCustomerAsync(request);

        if (!addCustomer.Succeeded)
        {
            Message = "The customer could not be saved. Check the customer file and that you have permission to write to the folder.";
            return;
        }

        navigationService.Navigate(AppPage.Customers);
    }

    [RelayCommand]
    private void Cancel()
    {
        navigationService.Navigate(AppPage.Customers);
    }
}




