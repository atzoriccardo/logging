using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Filters;
using System;
using Vittoria.Infrastructure.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseVittoriaLog(this IHostBuilder builder)
        {
            builder.UseSerilog((context, provider, loggerConfiguration) =>
            {
                loggerConfiguration
                    .MinimumLevel.Information()
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"))
                    .Filter.ByExcluding(Matching.FromSource("System"))
                    .WriteTo.Console(new VittoriaJsonFormatter());
            });

            return builder;
        }

    }
}
