using System;
using System.Collections.Generic;
using System.Text;
using Vittoria.Infrastructure.CorrelationId;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class CorrelationIdServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorrelationId(this IServiceCollection services)
        {
            services.AddSingleton<ICorrelationContextAccessor, CorrelationIdContextAccessor>();
            services.AddTransient<ICorrelationIdContextFactory, CorrelationIdContextFactory>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorrelationId(this IServiceCollection services, Action<CorrelationIdOptions> configure)
        {
            services.Configure(configure);

            return services.AddCorrelationId();
        }
    }
}
