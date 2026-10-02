namespace SupportTicketManagement.Domain.Models;

public class TicketComment(Guid id, string text)
{
    public Guid Id { get; private set; } = ValidateId(id);
    public string Text { get; private set; } = ValidateAndNormalizeText(text);
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public void ChangeText(string text)
    {
        Text = ValidateAndNormalizeText(text);
    }

    private static Guid ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Comment ID cannot be empty.", nameof(id));

        return id;
    }

    private static string ValidateAndNormalizeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Comment text cannot be empty.", nameof(text));

        text = text.Trim();

        return text;
    }
}
