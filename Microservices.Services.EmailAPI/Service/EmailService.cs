using Microservices.Services.EmailAPI.Data;
using Microservices.Services.EmailAPI.Messages;
using Microservices.Services.EmailAPI.Models;
using Microservices.Services.EmailAPI.Models.Dto;
using Microservices.Services.EmailAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Microservices.Services.EmailAPI.Service
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOption)
        {
            this._dbOptions = dbOption;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDto.cartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailNewUserAndLog(string email)
        {
            string message = "User registeration successful. <b/> : " + email;
            await LogAndEmail(message, email);
        }

        public async Task LogOrderPlaced(RewardMessage rewardMessageDto)
        {
            string message = "New Order Placed. <b/> OrderId: " + rewardMessageDto.OrderId;
            await LogAndEmail(message, "Admin@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new EmailLogger()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
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
