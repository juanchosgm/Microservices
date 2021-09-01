
using Microservices.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.EmailAPI.DbContexts;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<EmailLog> EmailLogs { get; set; }
}
