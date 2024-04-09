using FluentValidation;
using FluentValidation.AspNetCore;

using Paddi.DemoUsages.ApiDemo.Hubs;
using Paddi.DemoUsages.ApiDemo.Middlewares;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddResponseCaching();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpLogging(c =>
{
    c.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<DictDtoValidator>().Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = string.Join(',', context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        var result = new
        {
            Code = 9997,
            Msg = errors,
            Success = false,
            Data = ""
        };
        return new JsonResult(result)
        {
            StatusCode = StatusCodes.Status200OK
        };
    };
});

builder.Services.AddPaddiAppServices()
                .AddPaddiHostedServices()
                .AddPaddiRedis(builder.Configuration)
                .AddPaddiDbContext(builder.Configuration)
                .AddPaddiHangfire(builder.Configuration)
                .AddMapper();


builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
                                                                           .ReadFrom.Services(services)
                                                                           .Enrich.FromLogContext());

var app = builder.Build();

// Configure the HTTP request pipeline.
string[] devEnvNames = ["Development", "docker-compose", "integration-test"];
if (devEnvNames.Any(app.Environment.IsEnvironment))
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseApiDelayMiddleware();
app.UseHttpLogging();

app.UseHttpsRedirection();

app.UsePaddiHangfire();

app.UseResponseCaching();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/ChatHub");

app.Run();

public partial class Program { }
