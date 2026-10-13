using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;
using SupportTicketManagement.Infrastructure.Constants;
using System.Text.Json;

namespace SupportTicketManagement.Infrastructure.Repositories;

public class JsonFileTicketRepository(string filePath) : ITicketRepository
{
    private readonly string _filePath = Path.GetFullPath(filePath);

    public JsonFileTicketRepository() : this(JsonFilePath.TicketFilePath) { }

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<bool> Create(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Ticket>> GetAll()
    {
        if (!File.Exists(_filePath))
            return [];

        string json = await File.ReadAllTextAsync(_filePath);

        return JsonSerializer.Deserialize<List<Ticket>>(json, _options)
            ?? throw new JsonException($"Could not deserialize file at: '{_filePath}'");
    }

    public async Task<Ticket?> GetById(Guid id)
    {
        List<Ticket> tickets = await GetAll();

        return tickets.FirstOrDefault(t => t.Id == id);
    }

    public async Task<bool> Update(Ticket ticket)
    {
        List<Ticket> tickets = await GetAll();

        int index = tickets.FindIndex(t => t.Id == ticket.Id);

        if (index == -1)
            return false;

        tickets[index] = ticket;

        await SaveAll(tickets);

        return true;
    }

    private async Task SaveAll(IEnumerable<Ticket> tickets)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        string json = JsonSerializer.Serialize(tickets, _options);
        string temporaryPath = _filePath + ".tmp";

        await File.WriteAllTextAsync(temporaryPath, json);
        File.Move(temporaryPath, _filePath, overwrite: true);
    }
}
