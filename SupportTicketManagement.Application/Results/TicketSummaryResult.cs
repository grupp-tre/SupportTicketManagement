namespace SupportTicketManagement.Application.Results;

public record TicketSummaryResult
(
      bool Succeeded,
      int NewCount,
      int InProgressCount,
      int SolvedCount,
      string? ErrorMessage
);
  


