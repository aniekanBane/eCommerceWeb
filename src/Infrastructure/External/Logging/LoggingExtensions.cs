using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Extensions.Hosting;

namespace eCommerceWeb.External.Logging;

public static class LoggingExtensions
{
    // private const string OUTPUT_TEMPLATE = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    public static IHostBuilder UseAppLogger(
        this IHostBuilder builder, 
        Func<IConfiguration, LoggingOptions?>? loggingOptionsFactory = default)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        loggingOptionsFactory ??= configuration 
            => configuration.GetSection(LoggingOptions.CONFIG_SECTION).Get<LoggingOptions>();

        return builder.ConfigureLogging((ctx, logging) =>
        {
            LoggingOptions options = SetDefaults(loggingOptionsFactory(ctx.Configuration));
            ctx.HostingEnvironment.UseSerilog(options);

            logging.ClearProviders();
            logging.AddSerilog();
            logging.Services.AddSingleton(typeof(DiagnosticContext), new DiagnosticContext(Log.Logger));
        });
    }

    private static void UseSerilog(this IHostEnvironment env, LoggingOptions options)
    {
        var AssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

        // Serilog.Debugging.SelfLog.Enable(Console.WriteLine);
        var loggerConfiguration = new LoggerConfiguration();

        foreach (var (category, logLevel) in options.LogLevel)
        {
            var serilogLevel = ConvertToSerilogEventLevel(logLevel);

            if (category == "Default")
            {
                loggerConfiguration.MinimumLevel.Is(serilogLevel);
                continue;
            }

            loggerConfiguration.MinimumLevel.Override(category, serilogLevel);
        }

        loggerConfiguration = loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithProperty("ProcessId", Environment.ProcessId)
            .Enrich.WithProperty("AssemblyName", AssemblyName)
            .Enrich.WithProperty("ApplicationName", env.ApplicationName)
            .Enrich.WithProperty("EnviromentName", env.EnvironmentName)
            .Enrich.WithProperty("ContentRootPath", env.ContentRootPath)
            .Enrich.WithExceptionDetails()
            .WriteTo.File(
                path: Path.Combine("logs", "log.txt"),
                formatProvider: CultureInfo.InvariantCulture,
                rollOnFileSizeLimit: true,
                shared: true,
                fileSizeLimitBytes: 50 * 1024 * 1024,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                restrictedToMinimumLevel: ConvertToSerilogEventLevel(options.File!.MinimumLogEventLevel)
            );
        
        if (!env.IsProduction())
        {
            loggerConfiguration.WriteTo.Console();
        }

        if (options.Seq?.IsEnabled ?? false)
        {
            loggerConfiguration.WriteTo.Seq(
                serverUrl: options.Seq.ServerUrl, 
                apiKey: options.Seq.ApiKey
            );
        }

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static LoggingOptions SetDefaults(LoggingOptions? options)
    {
        options ??= new(); 

        options.LogLevel ??= [];

        if (!options.LogLevel.ContainsKey("Default"))
        {
            options.LogLevel["Default"] = LogLevel.Information;
        }

        options.File ??= new() 
        { 
            MinimumLogEventLevel = LogLevel.Warning 
        };

        return options;
    }

    private static LogEventLevel ConvertToSerilogEventLevel(LogLevel logLevel) 
        => logLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            _ or LogLevel.Critical or LogLevel.None => LogEventLevel.Fatal
        };
}
