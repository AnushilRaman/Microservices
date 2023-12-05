using Microservices.Services.RewardAPI.Data;
using Microservices.Services.RewardAPI.Messages;
using Microservices.Services.RewardAPI.Models;
using Microservices.Services.RewardAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Microservices.Services.RewardAPI.Service
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public RewardService(DbContextOptions<AppDbContext> dbOption)
        {
            this._dbOptions = dbOption;
        }

        public async Task<bool> UpdateRewards(RewardMessage rewardMessage)
        {
            try
            {
                Rewards rewards = new()
                {
                    OrderId = rewardMessage.OrderId,
                    RewardsActivity = rewardMessage.RewardsActivity,
                    UserId = rewardMessage.UserId,
                    RewardsDate = DateTime.Now
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.Rewards.AddAsync(rewards);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
