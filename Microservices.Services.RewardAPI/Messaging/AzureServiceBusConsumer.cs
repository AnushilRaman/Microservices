using Azure.Messaging.ServiceBus;
using Microservices.Services.RewardAPI.Messages;
using Microservices.Services.RewardAPI.Models.Dto;
using Microservices.Services.RewardAPI.Service;
using Microservices.Services.RewardAPI.Service.IService;
using Microservices.Services.RewardAPI.Utility;
using Newtonsoft.Json;
using System.Text;

namespace Microservices.Services.RewardAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IRewardService _rewardService;
        private ServiceBusProcessor _rewardProcessor;
        public AzureServiceBusConsumer(IRewardService rewardService)
        {
            var client = new ServiceBusClient(SD._serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(SD._orderCreatedTopic, SD._orderCreatedRewardSubscription);
            this._rewardService = rewardService;
        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewRewardsRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();
        }

        private async Task OnNewRewardsRequestReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            var emailRewards = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                await this._rewardService.UpdateRewards(emailRewards);
                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();
        }
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.Message.ToString());
            return Task.CompletedTask;
        }
    }
}
