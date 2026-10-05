namespace SupportTicketManagement.Application.Results;

public record TicketSummaryResult
(
      bool Success,
      int NewCount,
      int OngoingCount,
      int SolvedCount,
      string? ErrorMessage
);
  


