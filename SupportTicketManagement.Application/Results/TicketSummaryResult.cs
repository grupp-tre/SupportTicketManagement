namespace SupportTicketManagement.Application.Results;

public record TicketSummaryResult
(
      bool Succeeded,
      int NewCount,
      int OngoingCount,
      int SolvedCount,
      string? ErrorMessage
);
  


