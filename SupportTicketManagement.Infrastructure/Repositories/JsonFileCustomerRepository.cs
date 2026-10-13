
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Domain.Repositories;
using SupportTicketManagement.Infrastructure.Constants;
using System.Text.Json;

namespace SupportTicketManagement.Infrastructure.Repositories;

public class JsonFileCustomerRepository(string filePath) : ICustomerRepository
{
    private readonly string _filePath = Path.GetFullPath(filePath);

    public JsonFileCustomerRepository() : this(JsonFilePath.CustomerFilePath) { }

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public async Task<bool> Create(Customer customer)
    {
        List<Customer> customers = await GetAll();

        customers.Add(customer);

        await SaveAllAsync(customers);
        return true;
    }

    public async Task<List<Customer>> GetAll()
    {
        if (!File.Exists(_filePath))
            return [];

        var json = await File.ReadAllTextAsync(_filePath);

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
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

        string json = JsonSerializer.Serialize(customers, _options);
        var tempPath = _filePath + ".temp";

        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }

}
