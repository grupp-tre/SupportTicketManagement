using System.Text.Json;
using NSubstitute;
using SupportTicketManagement.Application.Requests;
using SupportTicketManagement.Application.Services;
using SupportTicketManagement.Domain.Enums;
using SupportTicketManagement.Domain.Models;
using SupportTicketManagement.Tests.Infrastructure.IntegrationTests;
using Xunit;

namespace SupportTicketManagement.Tests.Application.IntegrationTests;

public class TicketServiceTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task UpdateTicketAsyncAndAddTicketCommentAsync_Should_SaveChangesThatCanBeReadBack()
    {
        // Arrange
        using var storage = JsonTestStorage.Create();
        var customer = new Customer(Guid.NewGuid(), "Example customer", "customer@example.test");
        Assert.True(await storage.CreateCustomerRepository().Create(customer));
        var original = new Ticket(Guid.NewGuid(), "Printer offline", "Cannot print",
            TicketPriority.Normal, customer.Id);
        await File.WriteAllTextAsync(storage.TicketPath, JsonSerializer.Serialize(new[] { original }));
        var adminId = Guid.NewGuid();
        var admins = Substitute.For<IAdminService>();
        admins.AdminExistsAsync(adminId).Returns(Task.FromResult(true));
        var service = new TicketService(storage.CreateTicketRepository(), admins,
            Substitute.For<ICustomerService>());
        var request = new UpdateTicketRequest(original.Id,
            "Printer repaired", "Driver reinstalled", adminId, TicketPriority.High, TicketStatus.InProgress);

        // Act
        var update = await service.UpdateTicketAsync(request);
        var beforeComment = DateTimeOffset.UtcNow;
        var commentResult = await service.AddTicketCommentAsync(new(original.Id, "Reinstalled the driver"));
        var afterComment = DateTimeOffset.UtcNow;

        // Assert
        Assert.True(update.Succeeded, update.ErrorMessage);
        Assert.True(commentResult.Succeeded, commentResult.ErrorMessage);
        var readRepository = storage.CreateTicketRepository();
        var reloaded = await readRepository.GetById(original.Id);
        var reloadedCustomer = await storage.CreateCustomerRepository().GetById(customer.Id);
        Assert.NotNull(reloaded);
        Assert.NotNull(reloadedCustomer);
        Assert.Equal(original.Id, reloaded.Id);
        Assert.Equal(original.CreatedAt, reloaded.CreatedAt);
        Assert.Equal(reloadedCustomer.Id, reloaded.CustomerId);
        Assert.Equal("Printer repaired", reloaded.Title);
        Assert.Equal("Driver reinstalled", reloaded.Description);
        Assert.Equal(adminId, reloaded.AdminId);
        Assert.Equal(TicketPriority.High, reloaded.Priority);
        Assert.Equal(TicketStatus.InProgress, reloaded.Status);
        var comment = Assert.Single(reloaded.Comments);
        Assert.Equal("Reinstalled the driver", comment.Text);
        Assert.InRange(comment.CreatedAt, beforeComment, afterComment);
    }

    [Fact]
    [Trait("Category", "Integration")]
    public async Task GetTicketByIdAsync_Should_ReportMalformedJsonAndPreventOverwritingTheFile()
    {
        // Arrange
        using var storage = JsonTestStorage.Create();
        const string malformed = "[{ invalid ticket JSON";
        await File.WriteAllTextAsync(storage.TicketPath, malformed);
        var originalBytes = await File.ReadAllBytesAsync(storage.TicketPath);
        var service = new TicketService(storage.CreateTicketRepository(),
            Substitute.For<IAdminService>(), Substitute.For<ICustomerService>());
        var ticket = new Ticket(Guid.NewGuid(), "Printer offline", "Cannot print",
            TicketPriority.Normal, Guid.NewGuid());

        // Act
        var lookup = await service.GetTicketByIdAsync(ticket.Id);
        var update = await service.UpdateTicketAsync(new(ticket.Id, ticket.Title,
            ticket.Description, null, ticket.Priority, TicketStatus.New));

        // Assert
        Assert.False(lookup.Succeeded);
        Assert.False(string.IsNullOrWhiteSpace(lookup.ErrorMessage));
        Assert.False(update.Succeeded);
        Assert.False(string.IsNullOrWhiteSpace(update.ErrorMessage));
        Assert.Equal(originalBytes, await File.ReadAllBytesAsync(storage.TicketPath));
    }
}
