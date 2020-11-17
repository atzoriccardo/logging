using System;
using System.Collections.Generic;
using System.Text;
using Vittoria.Infrastructure.CorrelationId;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Aggiunge il DelegationHandler per la gestione del CorrelationId
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddCorrelationId(this IHttpClientBuilder builder)
        {
            builder.Services.AddTransient<CorrelationIdDelegatingHandler>();
            builder.AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

            return builder;
        }
    }
}
