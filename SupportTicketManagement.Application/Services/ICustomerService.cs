using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;

namespace SupportTicketManagement.Application.Services;

public interface ICustomerService
{
    Task<AddCustomerResult> AddCustomer(AddCustomerRequest addCustomerRequest);
    Task<GetAllCustomersResult> GetAllCustomers();
    Task<UpdateCustomerResult> UpdateCustomer(UpdateCustomerRequest updateCustomerRequest);
}
