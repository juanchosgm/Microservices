using Microsoft.Extensions.DependencyInjection;

namespace Microservices.MessageBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();
            return services;
        }
    }
}
