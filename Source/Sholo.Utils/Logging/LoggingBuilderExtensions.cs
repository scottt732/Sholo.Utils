using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Sholo.Utils.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddSerilogConsole(this ILoggingBuilder loggingBuilder, HostBuilderContext hostBuilderContext)
    {
        var configuration = hostBuilderContext.Configuration;
        return AddSerilogConsole(loggingBuilder, configuration);
    }

    public static ILoggingBuilder AddSerilogConsole(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
        Logger logger;
        var serilogKeys = configuration.GetSection("Serilog").AsEnumerable().ToArray();
        if (serilogKeys.Any(x => x.Key != "Serilog"))
        {
            logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Filter.ByExcluding(DefaultFilters)
                .CreateLogger();

            loggingBuilder.AddSerilog(logger, true);
        }
        else
        {
            logger = new LoggerConfiguration()
                .Filter.ByExcluding(DefaultFilters)
                .WriteTo.Console(
                    LogEventLevel.Debug,
                    "[{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    standardErrorFromLevel: LogEventLevel.Warning,
                    theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            loggingBuilder.AddSerilog(logger, true);
        }

        return loggingBuilder;
    }

    private static bool DefaultFilters(LogEvent le)
    {
        var sourceContext = le.Properties["SourceContext"]?.ToString() ?? string.Empty;

        if (sourceContext.Contains("Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager", StringComparison.Ordinal) && le.Level < LogEventLevel.Warning)
        {
            return true;
        }

        return false;
    }
}
