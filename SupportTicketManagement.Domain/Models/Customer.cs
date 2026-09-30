namespace SupportTicketManagement.Domain.Models;

public class Customer(Guid customerId, string customerName, string emailAddress)

{
    public Guid CustomerId { get; private set; } = NormalizeRequiredCustomerId(customerId);

    public string CustomerName { get; private set; } = NormalizeRequiredCustomerName(customerName);

    public string EmailAddress { get; private set; } = NormalizeRequiredEmailAddress(emailAddress);


    private static Guid NormalizeRequiredCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer Id is required.");

        return customerId;


    }

    private static string NormalizeRequiredCustomerName(string customerName)


    {

        if (string.IsNullOrWhiteSpace(customerName))

            throw new ArgumentException("Customer name is required.");

        if (customerName.Length < 2)

            throw new ArgumentException("Customer name must be a valid name and contain at least 2 letters");

        return customerName.Trim();
    }

    private static string NormalizeRequiredEmailAddress(string emailAddress)
    {

        if (string.IsNullOrWhiteSpace(emailAddress))

            throw new ArgumentException("Email is required.");


        if (!emailAddress.Contains('@'))

            throw new ArgumentException("Email must be a valid email address");

        return emailAddress.Trim().ToLower();
    }

    public void Rename(string customerName)
    {
        CustomerName = NormalizeRequiredCustomerName(customerName);

    }

    public void ChangeEmailAddress(string emailAddress)
    {

        EmailAddress = NormalizeRequiredEmailAddress(emailAddress);
    }
}