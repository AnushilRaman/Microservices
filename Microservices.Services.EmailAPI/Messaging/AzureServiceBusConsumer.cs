using Azure.Messaging.ServiceBus;
using Microservices.Services.EmailAPI.Messages;
using Microservices.Services.EmailAPI.Models.Dto;
using Microservices.Services.EmailAPI.Service;
using Microservices.Services.EmailAPI.Utility;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly EmailService emailService;
        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _emailOrderProcessor;
        private ServiceBusProcessor _registerUserProcessor;

        public AzureServiceBusConsumer(EmailService emailService)
        {
            var client = new ServiceBusClient(SD._serviceBusConnectionString);
            _emailCartProcessor = client.CreateProcessor(SD._emailCartQueue);
            _emailOrderProcessor = client.CreateProcessor(SD._emailOrderCreatedTopicName, SD._emailOrderCreatedSubscription);
            _registerUserProcessor = client.CreateProcessor(SD._emailregisterUserQueue);
            this.emailService = emailService;
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailCartProcessor.StartProcessingAsync();

            _emailOrderProcessor.ProcessMessageAsync += OnOrderPlaceRequestReceived;
            _emailOrderProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderProcessor.StartProcessingAsync();

            _registerUserProcessor.ProcessMessageAsync += OnUserRegisteredRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _registerUserProcessor.StartProcessingAsync();
        }

        private async Task OnOrderPlaceRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            var email = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                await this.emailService.LogOrderPlaced(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task OnUserRegisteredRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                await this.emailService.EmailNewUserAndLog(email);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _emailOrderProcessor.StopProcessingAsync();
            await _emailOrderProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();
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
                await this.emailService.EmailCartAndLog(cartDto);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
