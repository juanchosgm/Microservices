using Microservices.Services.EmailAPI;
using Microservices.Services.EmailAPI.DbContexts;
using Microservices.Services.EmailAPI.Messaging;
using Microservices.Services.EmailAPI.Models;
using Microservices.Services.EmailAPI.Repository;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
DbContextOptionsBuilder<ApplicationDbContext>? optionsBuilder = new();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailRepository(optionsBuilder.Options));
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddOptions();
builder.Services.Configure<AzureServiceBusConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Microservices.Services.Email", Version = "v1" });
});

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.Email v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetService<ApplicationDbContext>().Database.MigrateAsync();
app.MapControllers();
app.UseAzureServiceBusConsumer();

app.Run();
