using SupportTicketManagement.Domain.Enums;

namespace SupportTicketManagement.Domain.Models;

public class Ticket
{
    private readonly List<TicketComment> _comments = [];

    public Guid Id { get; private set; } 
    public string Title { get; private set; } 
    public string Description { get; private set; } 
    public TicketPriority Priority { get; private set; }
    public TicketStatus Status { get; private set; }
    public IReadOnlyList<TicketComment> Comments => _comments; 
    public Guid CustomerId { get; private set; }
    public Guid? AdminId { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }

    private Ticket(Guid id, string title, string description, TicketPriority priority, Guid customerId)
    {
        Id = ValidateId(id);
        Title = ValidateAndNormalizeTitle(title);
        Description = ValidateAndNormalizeDescription(description); 
        Priority = ValidatePriority(priority);
        Status = TicketStatus.New;
        CustomerId = ValidateId(customerId);
        CreatedAt = DateTimeOffset.UtcNow;
    }

    private static Guid ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        return id;
    }

    private static string ValidateAndNormalizeTitle(string title)
    {
        if (!string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        title = title.Trim();

        return title;
    }

    private static string ValidateAndNormalizeDescription(string description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException(nameof(description));

        description = description.Trim();

        return description;
    }

    private static TicketPriority ValidatePriority(TicketPriority priority)
    {
        if (!Enum.IsDefined(priority))
            throw new ArgumentOutOfRangeException(nameof(priority));

        return priority;
    }

    private static TicketStatus ValidateStatus(TicketStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));

        return status;
    }

    public void SetTitle(string title) => Title = ValidateAndNormalizeTitle(title);

    public void SetDescription(string description) => Description = ValidateAndNormalizeDescription(description);

    public void SetPriority(TicketPriority priority) => Priority = ValidatePriority(priority);

    public void SetStatus(TicketStatus status)
    {
        status = ValidateStatus(status);

        if (AdminId is null)
        {
            if (status == TicketStatus.InProgress)
                throw new InvalidOperationException("Cannot set ticket status to In Progress on a ticket with no administrator");

            if (status == TicketStatus.Solved)
                throw new InvalidOperationException("Cannot set ticket status to Solved on a ticket with no administrator");
        }
    }

    public void AddComment(TicketComment comment)
    {
        if (string.IsNullOrWhiteSpace(comment.Text))
            throw new ArgumentException("Comment text cannot be empty.", nameof(comment.Text));

        _comments.Add(comment);
    }

    public void SetAdminId(Guid? adminId)
    {
        if (adminId is null)
        {
            if (Status == TicketStatus.InProgress)
                throw new InvalidOperationException("Cannot remove administrator on a ticket with status In Progress");

            if (Status == TicketStatus.Solved)
                throw new InvalidOperationException("Cannot remove administrator on a ticket with status Solved");
        }

        if (adminId is not null && adminId == Guid.Empty)
            throw new ArgumentNullException(nameof(adminId)); 

        AdminId = adminId;
    }
}
