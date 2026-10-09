namespace SupportTicketManagement.Domain.Models;

public class Customer(Guid id, string name, string emailAddress)
{
    public Guid Id { get; private set; } = NormalizeRequiredId(id);
    public string Name { get; private set; } = NormalizeRequiredName(name);
    public string EmailAddress { get; private set; } = NormalizeRequiredEmailAddress(emailAddress);

    private static Guid NormalizeRequiredId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Customer Id is required.");

        return id;
    }

    private static string NormalizeRequiredName(string name)
    {

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Customer name is required.");

        if (name.Length < 2)
            throw new ArgumentException("Customer name must be a valid name and contain at least 2 letters");

        return name.Trim();
    }

    private static string NormalizeRequiredEmailAddress(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new ArgumentException("Email is required.");

        if (!emailAddress.Contains('@'))
            throw new ArgumentException("Email must be a valid email address");

        return emailAddress.Trim().ToLower();
    }

    public void Rename(string name)
    {
        Name = NormalizeRequiredName(name);
    }

    public void ChangeEmailAddress(string emailAddress)
    {
        EmailAddress = NormalizeRequiredEmailAddress(emailAddress);
    }
}