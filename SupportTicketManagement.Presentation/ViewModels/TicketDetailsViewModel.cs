using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Presentation.Models;
using SupportTicketManagement.Presentation.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SupportTicketManagement.Presentation.ViewModels;

public partial class TicketDetailsViewModel(
    ITicketService ticketService,
    ICustomerService customerService,
    IAdminService adminService,
    INavigationService navigationService) : ObservableObject
{
    private Guid _ticketId;

    public ObservableCollection<AssigneeOption> Assignees { get; } = [];
    public ObservableCollection<TicketCommentRow> Comments { get; } = [];
    public IReadOnlyList<TicketPriority> Priorities { get; } = Enum.GetValues<TicketPriority>();
    public IReadOnlyList<string> StatusOptions { get; } = ["New", "In progress", "Solved"];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    public partial bool IsLoaded { get; set; }

    public bool CanEdit => IsLoaded && !IsBusy;

    [ObservableProperty]
    public partial string Message { get; set; } = "";

    [ObservableProperty]
    public partial string Metadata { get; set; } = "";

    [ObservableProperty]
    public partial string Title { get; set; } = "";

    [ObservableProperty]
    public partial string Description { get; set; } = "";

    [ObservableProperty]
    public partial AssigneeOption? SelectedAssignee { get; set; }

    [ObservableProperty]
    public partial TicketPriority Priority { get; set; }

    [ObservableProperty]
    public partial int StatusIndex { get; set; }

    [ObservableProperty]
    public partial string NewComment { get; set; } = "";

    public async Task LoadTicketAsync(Guid ticketId)
    {
        _ticketId = ticketId;
        IsBusy = true;
        IsLoaded = false;
        Message = "";
        Metadata = "";
        Title = "";
        Description = "";
        NewComment = "";
        SelectedAssignee = null;
        Assignees.Clear();
        Comments.Clear();

        try
        {
            var result = await ticketService.GetTicketByIdAsync(ticketId);
            if (!result.Succeeded || result.Ticket is null)
            {
                Message = result.ErrorMessage ?? "Ticket not found.";
                return;
            }

            var ticket = result.Ticket;
            var customerResult = await customerService.GetCustomerByIdAsync(ticket.CustomerId);
            var adminResult = await adminService.GetAllAdminsAsync();
            if (!adminResult.Succeeded)
            {
                Message = adminResult.ErrorMessage ?? "Could not load assignees.";
                return;
            }

            SetAssignees(adminResult.Admins, ticket.AdminId);
            SetTicketFields(ticket);
            SetComments(ticket.Comments);

            var customerName = customerResult.Customer?.Name ?? "Unknown customer";
            Metadata = $"Customer: {customerName} · Created: {ticket.CreatedAt.ToLocalTime():g} · ID: {ticket.Id}";
            IsLoaded = true;

            if (!customerResult.Succeeded)
                Message = customerResult.ErrorMessage ?? "Customer details could not be loaded.";
        }
        catch (Exception ex)
        {
            Message = $"Could not load ticket: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Back() => navigationService.Navigate(AppPage.Tickets);

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!CanEdit)
            return;

        Message = ValidateFields();
        if (Message.Length > 0)
            return;

        IsBusy = true;
        try
        {
            var request = new UpdateTicketRequest(
                _ticketId, Title, Description, SelectedAssignee?.Id,
                Priority, (TicketStatus)StatusIndex);

            var result = await ticketService.UpdateTicketAsync(request);
            if (!result.Succeeded || result.Ticket is null)
            {
                Message = result.ErrorMessage ?? "Could not save changes.";
                return;
            }

            SetTicketFields(result.Ticket);
            Message = "Changes saved.";
        }
        catch (Exception ex)
        {
            Message = $"Could not save changes: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task AddCommentAsync()
    {
        if (!CanEdit)
            return;

        if (string.IsNullOrWhiteSpace(NewComment))
        {
            Message = "Enter a comment.";
            return;
        }

        IsBusy = true;
        Message = "";
        try
        {
            var result = await ticketService.AddTicketCommentAsync(new(_ticketId, NewComment));
            if (!result.Succeeded)
            {
                Message = result.ErrorMessage ?? "Could not add comment.";
                return;
            }

            NewComment = "";
            var ticketResult = await ticketService.GetTicketByIdAsync(_ticketId);
            if (!ticketResult.Succeeded || ticketResult.Ticket is null)
            {
                Message = ticketResult.ErrorMessage ?? "Could not reload comments.";
                return;
            }

            SetComments(ticketResult.Ticket.Comments);
            Message = "Comment added.";
        }
        catch (Exception ex)
        {
            Message = $"Could not add comment: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private string ValidateFields()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description))
            return "Title and description are required.";

        if (StatusIndex < 0 || StatusIndex >= StatusOptions.Count)
            return "Select a status.";

        if (StatusIndex > 0 && SelectedAssignee?.Id is null)
            return "Select an assignee for In progress or Solved.";

        return "";
    }

    private void SetTicketFields(Ticket ticket)
    {
        Title = ticket.Title;
        Description = ticket.Description;
        Priority = ticket.Priority;
        StatusIndex = (int)ticket.Status;
    }

    private void SetAssignees(IReadOnlyList<Admin> admins, Guid? selectedId)
    {
        Assignees.Add(new(null, "Unassigned"));
        foreach (var admin in admins)
            Assignees.Add(new(admin.Id, $"{admin.FirstName} {admin.LastName}"));

        SelectedAssignee = Assignees.FirstOrDefault(a => a.Id == selectedId);
        if (SelectedAssignee is null)
        {
            SelectedAssignee = new(selectedId, "Unknown assignee");
            Assignees.Add(SelectedAssignee);
        }
    }

    private void SetComments(IReadOnlyList<TicketComment> comments)
    {
        Comments.Clear();
        foreach (var comment in comments.OrderBy(c => c.CreatedAt))
            Comments.Add(new(comment.CreatedAt.ToLocalTime().ToString("g"), comment.Text));
    }
}
