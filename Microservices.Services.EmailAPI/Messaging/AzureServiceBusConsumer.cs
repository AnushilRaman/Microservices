using Azure.Messaging.ServiceBus;
using Microservices.Services.EmailAPI.Models.Dto;
using Microservices.Services.EmailAPI.Utility;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private ServiceBusProcessor _emailCartProcessor;
        public AzureServiceBusConsumer()
        {
            var client = new ServiceBusClient(SD._serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(SD._emailCartQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();
        }
        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.Message.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private Task _emailCartProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            throw new NotImplementedException();
        }


    }
}
