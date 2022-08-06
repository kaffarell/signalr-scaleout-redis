using Owin;
using Microsoft.AspNetCore.SignalR.StackExchangeRedis;
using DeviceApi.Hubs;
using Microsoft.AspNet.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR().AddStackExchangeRedis("redis", options => 
{
    options.Configuration.ChannelPrefix = "DeviceStatus";
});

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
    builder =>
    {
        builder.AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials();
    }
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");


app.UseAuthorization();

app.MapControllers();


//GlobalHost.DependencyResolver.UseStackExchangeRedis("redis", 6379, "", "DeviceStatus");

app.MapHub<DeviceStatusHub>("/statusHub");


app.Run();
