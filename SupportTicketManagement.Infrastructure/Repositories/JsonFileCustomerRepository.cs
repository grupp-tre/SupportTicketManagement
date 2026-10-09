
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;
using SupportTicketManagement.Infrastructure.Constants;
using System.Text.Json;

namespace SupportTicketManagement.Infrastructure.Repositories;

public class JsonFileCustomerRepository : ICustomerRepository
{
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public Task<bool> Create(Customer customer)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Customer>> GetAll()
    {
        if (!File.Exists(JsonFilePath.CustomerFilePath))
            return [];

        var json = await File.ReadAllTextAsync(JsonFilePath.CustomerFilePath);

        var customers = JsonSerializer.Deserialize<List<Customer>>(json, _options)
            ?? throw new JsonException("The customer files must contain a JSON-based list.");

        return customers;
    }

    public async Task<Customer?> GetById(Guid id)
    {
        List<Customer> customers = await GetAll();

        return customers.FirstOrDefault(c => c.Id == id);
    }

    public async Task<bool> Update(Customer customer)
    {
        List<Customer> customers = await GetAll();

        int index = customers.FindIndex(c => c.Id == customer.Id);

        if (index == -1)
            return false;

        customers[index] = customer;

        await SaveAllAsync(customers);
        return true;
    }
    public async Task SaveAllAsync(IEnumerable<Customer> customers)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(JsonFilePath.CustomerFilePath)!);

        string json = JsonSerializer.Serialize(customers, _options);
        var tempPath = JsonFilePath.CustomerFilePath + ".temp";

        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, JsonFilePath.CustomerFilePath, overwrite: true);
    }

}
