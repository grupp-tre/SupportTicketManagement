using SupportTicketManagement.Infrastructure.Repositories;

namespace SupportTicketManagement.Tests.Infrastructure.IntegrationTests;

internal sealed class JsonTestStorage : IDisposable
{
    private readonly string _directory;
    public string TicketPath => Path.Combine(_directory, "tickets.json");
    public string CustomerPath => Path.Combine(_directory, "customers.json");

    private JsonTestStorage(string directory)
    {
        _directory = directory;
    }

    public static JsonTestStorage Create()
    {
        var directory = Path.Combine(Path.GetTempPath(), "SupportTicketManagement.Tests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(directory);
        return new JsonTestStorage(directory);
    }

    public JsonFileTicketRepository CreateTicketRepository() => new(TicketPath);
    public JsonFileCustomerRepository CreateCustomerRepository() => new(CustomerPath);

    public void Dispose() => Directory.Delete(_directory, recursive: true);
}
