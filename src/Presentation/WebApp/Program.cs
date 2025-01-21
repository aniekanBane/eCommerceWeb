using eCommerceWeb.External;
using eCommerceWeb.External.Logging;
using eCommerceWeb.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAppLogger();

if (builder.Environment.IsDocker())
{
    var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisCache")!);
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(redis);
}

builder.Services
    .AddPersistence()
    .AddExternalServices();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() || !app.Environment.IsDocker())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static class EnviromentExtentions
{
    public static bool IsDocker(this IHostEnvironment env)
    {
        return env.EnvironmentName == "Docker";
    }
}
