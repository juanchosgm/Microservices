
using Microservices.Services.EmailAPI.DbContexts;
using Microservices.Services.EmailAPI.Models;
using Microservices.Services.EmailAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.EmailAPI.Repository;
public class EmailRepository
{
    private readonly DbContextOptions<ApplicationDbContext> options;

    public EmailRepository(DbContextOptions<ApplicationDbContext> options)
    {
        this.options = options;
    }

    public async Task SendAndLogEmailAsync(UpdatePaymentResultMessageDto updatePaymentResultMessage)
    {
        EmailLog log = new()
        {
            Email = updatePaymentResultMessage.Email,
            EmailSent = DateTime.UtcNow,
            Log = $"Order - {updatePaymentResultMessage.OrderId} has been created successfully"
        };
        await using ApplicationDbContext? context = new(options);
        await context.EmailLogs.AddAsync(log);
        await context.SaveChangesAsync();
    }
}
