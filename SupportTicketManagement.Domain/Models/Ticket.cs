using SupportTicketManagement.Domain.Enums;

using System.Text.Json.Serialization;

namespace SupportTicketManagement.Domain.Models;

public class Ticket(Guid id, string title, string description, TicketPriority priority, Guid customerId)
{
    private readonly List<TicketComment> _comments = [];

    public Guid Id { get; private set; } = ValidateId(id);
    public string Title { get; private set; } = ValidateAndNormalizeTitle(title);
    public string Description { get; private set; } = ValidateAndNormalizeDescription(description);
    public TicketPriority Priority { get; private set; } = ValidatePriority(priority);
    public TicketStatus Status { get; private set; } = TicketStatus.New;
    public IReadOnlyList<TicketComment> Comments => _comments;
    public Guid CustomerId { get; private set; } = ValidateId(customerId);
    public Guid? AdminId { get; private set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    [JsonConstructor]
    private Ticket(Guid id, string title, string description, TicketPriority priority,
        Guid customerId, TicketStatus status, Guid? adminId, DateTimeOffset createdAt,
        IReadOnlyList<TicketComment>? comments)
        : this(id, title, description, priority, customerId)
    {
        SetAdminId(adminId);
        SetStatus(status);
        CreatedAt = createdAt;
        foreach (var comment in comments ?? [])
            AddComment(comment);
    }

    private static Guid ValidateId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentNullException(nameof(id));

        return id;
    }

    private static string ValidateAndNormalizeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        title = title.Trim();

        return title;
    }

    private static string ValidateAndNormalizeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
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
        Status = status;
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
