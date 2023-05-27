using Microsoft.AspNetCore.RateLimiting;

using Paddi.DemoUsages.ApiDemo.Extensions;
using Paddi.DemoUsages.ApiDemo.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(o => o.AddFixedWindowLimiter("FixedWindow", c =>
{
    c.PermitLimit = 5;
    c.Window = TimeSpan.FromSeconds(5);
    c.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.NewestFirst;
    c.QueueLimit = 2;
}));
builder.Services.AddPaddiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();

app.UseApiDelayMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseRateLimiter();

app.Run();
