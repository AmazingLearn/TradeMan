using Microsoft.EntityFrameworkCore;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.BusinessLayer.Logic.Broker;
using TradingManBackend.BusinessLayer.Logic.Messaging;
using TradingManBackend.Cron;
using TradingManBackend.DataLayer;
using TradingManBackend.Util.Json;

var MyAllowedSpecificOrigins = "http://localhost:3001";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new NotificationJsonConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CompanyConnStr")));

builder.Services.AddScoped<UsersLogic>();
builder.Services.AddScoped<AlpacaLogic>();
builder.Services.AddScoped<NotificationsLogic>();
builder.Services.AddScoped<StockDataLogic>();
builder.Services.AddScoped<PositionLogic>();
builder.Services.AddScoped<OrderLogic>();
builder.Services.AddScoped<MessagingLogic>();
builder.Services.AddScoped<TelegramMessengerLogic>();
builder.Services.AddScoped<NotificationEvaluation>();

//Cron job to be ran 15 min after markets open open - 9:30 EDT
builder.Services.AddCronJob<NotificationEvaluationCronJobMarketsOpen>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Utc;
    c.CronExpression = @"0 45 5 * * ?";
});

//Cron job to be ran 15 min before markets close - 14:00 EDT
builder.Services.AddCronJob<NotificationEvaluationCronJobMarketsClose>(c =>
{
    c.TimeZoneInfo = TimeZoneInfo.Utc;
    c.CronExpression = @"0 45 11 * * ?";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowedSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3001",
                                              "http://localhost",
                                              "http://localhost:3000");
                          policy.AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowedSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

// Adding visibility for tests.
public partial class Program { }
