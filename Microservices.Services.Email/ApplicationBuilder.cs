
using Microservices.Services.EmailAPI.Messaging;

namespace Microservices.Services.EmailAPI;
public static class ApplicationBuilder
{
    public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

    public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
    {
        ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
        IHostApplicationLifetime? hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
        hostApplicationLifetime.ApplicationStarted.Register(OnStart);
        hostApplicationLifetime.ApplicationStopped.Register(OnStop);
        return app;
    }

    private static void OnStart()
    {
        ServiceBusConsumer.Start();
    }

    private static void OnStop()
    {
        ServiceBusConsumer.Stop();
    }
}
