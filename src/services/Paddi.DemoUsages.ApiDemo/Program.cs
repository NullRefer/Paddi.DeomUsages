using FluentValidation;
using FluentValidation.AspNetCore;

using Paddi.DemoUsages.ApiDemo.Dtos.Dict;
using Paddi.DemoUsages.ApiDemo.Hubs;
using Paddi.DemoUsages.ApiDemo.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<DictDtoValidator>().Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = string.Join(',', context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return new JsonResult(new
        {
            Code = 9997,
            Msg = errors,
            Success = false,
            Data = ""
        })
        {
            StatusCode = StatusCodes.Status200OK
        };
    };
});

builder.Services.AddPaddiAppServices(builder.Configuration)
                .AddPaddiHostedServices()
                .AddPaddiRedis(builder.Configuration)
                .AddPaddiDbContext(builder.Configuration);

var app = builder.Build();

app.SetupEnv();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("docker-compose"))
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
app.MapHub<ChatHub>("/ChatHub");

app.Run();
