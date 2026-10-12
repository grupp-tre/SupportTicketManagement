using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Presentation.Navigation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class CreateCustomerViewModel(ICustomerService customerService, INavigationService navigationService) : ObservableObject
{
    private Guid _customerId;

    [ObservableProperty]
    public partial string CustomerName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EmailAddress { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Message { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string PageTitle { get; set; } ="Add customer";

    public void ClearForm ()
    {
        _customerId = Guid.Empty;
        CustomerName = string.Empty;
        EmailAddress = string.Empty;
        Message = string.Empty;
        PageTitle = "Add customer";
    }

    public async Task LoadCustomerAsync(Guid customerId)
    {
        var result = await customerService.GetCustomerByIdAsync(customerId);

        if (!result.Succeeded || result.Customer is null)
        {
            Message = result.ErrorMessage ?? "The customer could not be found.";
            return;
        }

        _customerId = customerId;
        CustomerName = result.Customer.Name;
        EmailAddress = result.Customer.EmailAddress;
        Message = string.Empty;
        PageTitle = "Edit customer";

    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Message = string.Empty;

        try
        {
            if (_customerId == Guid.Empty)
            {
                var addCustomer = await customerService.AddCustomerAsync(
                    new AddCustomerRequest(CustomerName, EmailAddress));

                if (!addCustomer.Succeeded)
                {
                    Message = addCustomer.ErrorMessage ?? "The customer could not be saved.";
                    return;
                }
            }
            else
            {
                var updateCustomer = await customerService.UpdateCustomerAsync(
                    new UpdateCustomerRequest(_customerId, CustomerName, EmailAddress));

                if (!updateCustomer.Succeeded)
                {
                    Message = updateCustomer.ErrorMessage ?? "The customer could not be updated.";
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Message = $"The customer could not be saved: {ex.Message}.";
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




