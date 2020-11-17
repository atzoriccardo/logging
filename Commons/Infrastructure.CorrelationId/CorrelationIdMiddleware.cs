using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vittoria.Infrastructure.CorrelationId
{
    /// <summary>
    /// Middleware per la gestione del CorrelationId
    /// </summary>
    public class CorrelationIdMiddleware
    {
        #region DYNAMIC

        #region FIELDS

        private readonly RequestDelegate _next;
        private readonly IOptions<CorrelationIdOptions> _options;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Crea una nuova instanza del Middleware <see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdMiddleware"/>
        /// </summary>
        /// <param name="next">Middleware successivo delle pipeline.</param>
        /// <param name="options">Istanza del logger.</param>
        /// <param name="logger">Classe di configurazione del CorrelationId (<see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdOptions"/>)</param>
        public CorrelationIdMiddleware(RequestDelegate next, IOptions<CorrelationIdOptions> options, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region PROPERTIES
        #endregion

        #region METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="correlationIdContextFactory"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ICorrelationIdContextFactory correlationIdContextFactory)
        {
            var hasCorrelationId = context.Request.Headers.TryGetValue(_options.Value.RequestHeaderKey, out var cid);
            var correlationId = hasCorrelationId ? cid.FirstOrDefault() : null;

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add(_options.Value.RequestHeaderKey, correlationId);
            }

            correlationIdContextFactory.Create(correlationId, _options.Value.RequestHeaderKey);

            if (_options.Value.IncludeInResponse)
            {
                context.Response.OnStarting(() =>
                {
                    if (!context.Request.Headers.ContainsKey(_options.Value.RequestHeaderKey))
                    {
                        context.Response.Headers.Add(_options.Value.RequestHeaderKey, correlationId);
                    }

                    return Task.CompletedTask;
                });
            }
            
            await _next(context);
        }

        #endregion

        #endregion
    }
}
