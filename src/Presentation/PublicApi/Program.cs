using eCommerceWeb.External;
using eCommerceWeb.External.Storage;
using eCommerceWeb.Persistence;
using eCommerceweb.PublicApi.Extensions;
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

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddExternalServices();

var configSection = builder.Configuration.GetRequiredSection(StorageOptions.CONFIG_SECTION);
builder.Services.Configure<StorageOptions>(configSection);
var storageOptions = configSection.Get<StorageOptions>();
builder.Services.AddStorageManger(storageOptions!);

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

app.Run();

public partial class Program;

static class EnviromentExtentions
{
    public static bool IsDocker(this IHostEnvironment env)
    {
        return env.EnvironmentName == "Docker";
    }
}
