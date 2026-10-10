using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;
using SupportTicketManagement.Infrastructure.Constants;
using System.Text.Json;

namespace SupportTicketManagement.Infrastructure.Repositories;

public class JsonFileAdminRepository : IAdminRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<List<Admin>> GetAll()
    {
        if (!File.Exists(JsonFilePath.AdminFilePath))
            return [];

        string json = await File.ReadAllTextAsync(JsonFilePath.AdminFilePath);

        return JsonSerializer.Deserialize<List<Admin>>(json, _options)
            ?? throw new JsonException($"Could not deserialize file at: '{JsonFilePath.AdminFilePath}'");
    }

    public async Task<Admin?> GetById(Guid id)
    {
        List<Admin> admins = await GetAll();

        return admins.FirstOrDefault(a => a.Id == id);
    }
}
