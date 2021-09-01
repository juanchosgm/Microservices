using Microsoft.Extensions.DependencyInjection;

namespace Microservices.MessageBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();
            return services;
        }
    }
}
