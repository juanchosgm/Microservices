using Microsoft.Extensions.DependencyInjection;

namespace PaymentProcessor
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPaymentProcessor(this IServiceCollection services)
        {
            services.AddSingleton<IProcessPayment, ProcessPayment>();
            return services;
        }
    }
}
