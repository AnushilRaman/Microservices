using Microservices.Services.EmailAPI.Data;
using Microservices.Services.EmailAPI.Extension;
using Microservices.Services.EmailAPI.Messaging;
using Microservices.Services.EmailAPI.Service;
using Microservices.Services.EmailAPI.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddSingleton(new EmailService(optionBuilder.Options));


// Add services to the container.

SD._serviceBusConnectionString = builder.Configuration["MessageServices:MessageBusConnectionString"];
SD._emailCartQueue = builder.Configuration["TopicAndQueueNames:EmailShoppingCartQueue"];
SD._emailregisterUserQueue = builder.Configuration["TopicAndQueueNames:EmailregisterUserQueue"];
SD._emailOrderCreatedTopicName = builder.Configuration["TopicAndQueueNames:OrderCreatedTopic"];
SD._emailOrderCreatedSubscription = builder.Configuration["TopicAndQueueNames:OrderCreated_Email_Subscription"];

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
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Email Api");
    option.RoutePrefix = string.Empty;
});

app.UseDeveloperExceptionPage();

AddMigration();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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