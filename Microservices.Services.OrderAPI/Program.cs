using AutoMapper;
using Microservices.MessageBus;
using Microservices.Services.OrderAPI;
using Microservices.Services.OrderAPI.Data;
using Microservices.Services.OrderAPI.Extensions;
using Microservices.Services.OrderAPI.Service;
using Microservices.Services.OrderAPI.Service.IService;
using Microservices.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
IMapper mapper = MapperConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();
builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddHttpClient("Product", x => x.BaseAddress
= new Uri(builder.Configuration["ServiceUrls:ProductApi"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

StaticClass.TopicName = builder.Configuration.GetSection("TopicAndQueueNames:OrderCreatedTopic").Value;
StaticClass.MessageBusConnectionString = builder.Configuration["MessageServices:MessageBusConnectionString"];

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Bearer Created JWT Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id= JwtBearerDefaults.AuthenticationScheme
                }
            },new string[]{}
        }
    });
});
StaticClass.MessageBusConnectionString = builder.Configuration["MessageServices:MessageBusConnectionString"];
StaticClass.EmailShoppingCart = builder.Configuration["TopicAndQueueNames:EmailShoppingCartQueue"];

builder.AddAppAuthentication();
// Add services to the container.

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
    option.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Api");
    option.RoutePrefix = string.Empty;
});

Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Value;

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


AddMigration();

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