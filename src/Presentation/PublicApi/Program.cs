using eCommerceWeb.Application;
using eCommerceWeb.External;
using eCommerceWeb.External.Storage;
using eCommerceWeb.Persistence;
using eCommerceweb.PublicApi.Extensions;
using Carter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
    .Enrich.WithProperty("ApplicationName", builder.Environment.ApplicationName)
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddExternalServices();

var configSection = builder.Configuration.GetRequiredSection(StorageOptions.CONFIG_SECTION);
builder.Services.Configure<StorageOptions>(configSection);
var storageOptions = configSection.Get<StorageOptions>();
builder.Services.AddStorageManger(storageOptions!);

builder.Services.AddCarter();

// builder.Services.AddProblemDetails(options => options.CustomizeProblemDetails = ctx =>
// {
//     if (ctx.Exception is Exception ex)
//     {
//         ctx.ProblemDetails.Title = ErrorMessages.Common.ServerError;
//         ctx.ProblemDetails.Status = StatusCodes.Status500InternalServerError;
//         ctx.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
//     }
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsDocker())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.InitializeDatabase();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapCarter();

app.Run();

public partial class Program;

static class EnvironmentExtentions
{
    public static bool IsDocker(this IHostEnvironment env)
    {
        return env.EnvironmentName == "Docker";
    }
}
