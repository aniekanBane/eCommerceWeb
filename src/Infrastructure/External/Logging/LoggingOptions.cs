using Microsoft.Extensions.Logging;

namespace eCommerceWeb.External.Logging;

public sealed class LoggingOptions
{
    public const string CONFIG_SECTION = nameof(Logging);

    public Dictionary<string, LogLevel> LogLevel { get; set; } = [];
    public FileOptions? File { get; set; }
    public SeqOptions? Seq { get; set; }

    public class FileOptions
    {
        public LogLevel MinimumLogEventLevel { get; set; }
    }

    public class SeqOptions
    {
        public bool IsEnabled { get; set; }
        public required string ServerUrl { get; set; }
        public string? ApiKey {get; set; }
    }
}
