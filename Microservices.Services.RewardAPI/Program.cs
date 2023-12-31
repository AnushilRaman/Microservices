using Microservices.Services.RewardAPI.Data;
using Microservices.Services.RewardAPI.Extension;
using Microservices.Services.RewardAPI.Messaging;
using Microservices.Services.RewardAPI.Service;
using Microservices.Services.RewardAPI.Service.IService;
using Microservices.Services.RewardAPI.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new RewardService(optionBuilder.Options));

SD._serviceBusConnectionString = builder.Configuration["MessageServices:MessageBusConnectionString"];
SD._orderCreatedTopic = builder.Configuration["TopicAndQueueNames:OrderCreatedTopic"];
SD._orderCreatedRewardSubscription = builder.Configuration["TopicAndQueueNames:OrderCreated_Rewards_Subscription"];
// Add services to the container.
builder.Services.AddScoped<IRewardService, RewardService>();
builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(option =>
{
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Reward Api");
    option.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AddMigration();

app.UseAzureServiceBusExtension();

app.Run();



void AddMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}