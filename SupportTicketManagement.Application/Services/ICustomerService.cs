using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;

namespace SupportTicketManagement.Application.Services;

public interface ICustomerService
{
    Task<AddCustomerResult> AddCustomerAsync(AddCustomerRequest addCustomerRequest);
    Task<GetAllCustomersResult> GetAllCustomersAsync();
    Task<GetCustomerByIdResult> GetCustomerByIdAsync(Guid id);
    Task<UpdateCustomerResult> UpdateCustomerAsync(UpdateCustomerRequest updateCustomerRequest);
}
