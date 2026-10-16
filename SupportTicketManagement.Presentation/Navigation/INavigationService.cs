using Microsoft.UI.Xaml.Controls;
using SupportTicketManagement.Domain.Enums;
using System.ComponentModel;

namespace SupportTicketManagement.Presentation.Navigation;

public interface INavigationService : INotifyPropertyChanged
{
    Page? CurrentPage { get; }
    void Navigate(AppPage page);
    void NavigateToTickets(TicketStatus status);
}
