namespace SupportTicketManagement.Infrastructure.Constants;

public static class JsonFilePath
{
    private static readonly string LocalAppFolderName = "SupportTicketManagement";
    private static readonly string LocalAppPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
        LocalAppFolderName
    );
    private static readonly string CustomerJsonFileName = "customers.json";
    private static readonly string TicketJsonFileName = "tickets.json";
    private static readonly string AdminJsonFileName = "admins.json";

    public static readonly string CustomerFilePath = Path.Combine(LocalAppPath, CustomerJsonFileName);
    public static readonly string TicketFilePath = Path.Combine(LocalAppPath, TicketJsonFileName);
    public static readonly string AdminFilePath = Path.Combine(LocalAppPath, AdminJsonFileName);
}
