
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Results;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;

namespace SupportTicketManagement.Application.Services;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<AddCustomerResult> AddCustomerAsync(AddCustomerRequest addCustomerRequest)
    {
        ArgumentNullException.ThrowIfNull(addCustomerRequest);

        Customer customer;

        try
        {
            var id = Guid.NewGuid();
            customer = new Customer(id, addCustomerRequest.Name, addCustomerRequest.EmailAddress);

            bool saved = await customerRepository.Create(customer);

            return saved
                ? new AddCustomerResult(true, customer, null)
                : new AddCustomerResult(false, null, "Unable to create the customer");
        }
        catch (Exception ex)
        {
            return new AddCustomerResult(false, null, ex.Message);
        }
    }

    public async Task<GetAllCustomersResult> GetAllCustomersAsync()
    {

        var customers = await customerRepository.GetAll();
        return new GetAllCustomersResult(true, customers, null);
    }

    public async Task<GetCustomerByIdResult> GetCustomerByIdAsync(Guid id)
    {
        var customer = await customerRepository.GetById(id);

        if (customer == null)
            return new GetCustomerByIdResult(false, null, $"Could not find customer with Id: '{id}'.");

        return new GetCustomerByIdResult(true, customer, null);
    }

    public async Task<UpdateCustomerResult> UpdateCustomerAsync(UpdateCustomerRequest updateCustomerRequest)
    {
        ArgumentNullException.ThrowIfNull(updateCustomerRequest);

        if (updateCustomerRequest.Id == Guid.Empty)
            return new UpdateCustomerResult(false, null, "Customer id is required.");

        try
        {

            var customers = await customerRepository.GetAll();
            var updatedCustomer = customers.FirstOrDefault(c => c.Id == updateCustomerRequest.Id);

            if (updatedCustomer is null)
                return new UpdateCustomerResult(false, null, "The customer could not be found.");

            updatedCustomer.Rename(updateCustomerRequest.Name);
            updatedCustomer.ChangeEmailAddress(updateCustomerRequest.EmailAddress);

            var saved = await customerRepository.Update(updatedCustomer);

            return saved
               ? new UpdateCustomerResult(true, updatedCustomer, null)
               : new UpdateCustomerResult(false, null, "Unable to update customer");

        }
        catch (Exception ex)
        {
            return new UpdateCustomerResult(false, null, ex.Message);
        }
    }
}
