using Microservices.Services.RewardAPI.Data;
using Microservices.Services.RewardAPI.Extension;
using Microservices.Services.RewardAPI.Messaging;
using Microservices.Services.RewardAPI.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
SD._serviceBusConnectionString = builder.Configuration["MessageServices:MessageBusConnectionString"];
SD._orderCreatedTopic = builder.Configuration["TopicAndQueueNames:OrderCreatedTopic"];
SD._orderCreatedRewardSubscription = builder.Configuration["TopicAndQueueNames:OrderCreated_Rewards_Subscription"];
// Add services to the container.
builder.Services.AddScoped<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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