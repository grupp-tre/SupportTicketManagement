namespace SupportTicketManagement.Domain.Models;

public class Admin(Guid id, string firstName, string lastName, string email)
{
    public Guid Id { get; private set; } = ValidateId(id);
    public string FirstName { get; private set; } = ValidateAndNormalizeName(firstName);
    public string LastName { get; private set; } = ValidateAndNormalizeName(lastName);
    public string Email { get; private set; } = ValidateAndNormalizeEmail(email);

    private static Guid ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        return id; 
    }

    private static string ValidateAndNormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new ArgumentNullException(nameof(name));

        name = name.Trim();

        return name;
    }

    private static string ValidateAndNormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email));

        if (!email.Contains('@'))
            throw new ArgumentException("Email must be a valid email address");  

        email = email.Trim();

        return email;
    }
}
