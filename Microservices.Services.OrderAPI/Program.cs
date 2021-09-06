using Microservices.MessageBus;
using Microservices.Services.OrderAPI;
using Microservices.Services.OrderAPI.DbContexts;
using Microservices.Services.OrderAPI.Messaging;
using Microservices.Services.OrderAPI.Models;
using Microservices.Services.OrderAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
DbContextOptionsBuilder<ApplicationDbContext>? optionsBuilder = new();
optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new OrderRepository(optionsBuilder.Options));
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();
builder.Services.AddOptions();
builder.Services.Configure<AzureServiceBusConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Microservices.Services.OrderAPI", Version = "v1" });
});

WebApplication? app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices.Services.OrderAPI v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetService<ApplicationDbContext>().Database.MigrateAsync();
app.MapControllers();
app.UseAzureServiceBusConsumer();

app.Run();
