using Microservices.Services.EmailAPI.Messaging;
using System.Reflection.Metadata;

namespace Microservices.Services.EmailAPI.Extension
{
    public static class ApplicationBuilderExtension
    {
        private static IAzureServiceBusConsumer _serviceBusConsumer;
        public static IApplicationBuilder UseAzureServiceBusExtension(this IApplicationBuilder app)
        {
            _serviceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostAppLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostAppLife.ApplicationStarted.Register(OnStart);
            hostAppLife.ApplicationStopped.Register(OnStop);
            return app;
        }

        private static void OnStop()
        {
            _serviceBusConsumer.Stop();
        }
        private static void OnStart()
        {
            _serviceBusConsumer?.Start();
        }
    }
}
