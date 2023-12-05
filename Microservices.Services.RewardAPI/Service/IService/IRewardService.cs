using Microservices.Services.RewardAPI.Messages;
using Microservices.Services.RewardAPI.Models.Dto;

namespace Microservices.Services.RewardAPI.Service.IService
{
    public interface IRewardService
    {
        Task<bool> UpdateRewards(RewardMessage rewardMessage);
    }
}
