using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vittoria.Infrastructure.CorrelationId
{
	/// <summary>
	/// Delegatin Handler per l'aggiunta del CorrelationId per le richiesta HTTP in uscita.
	/// </summary>
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
		#region DYNAMIC

		#region FIELDS

		private readonly ICorrelationContextAccessor _correlationContextAccessor;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Crea una nuova istanza del handler <see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdDelegatingHandler"/>
		/// </summary>
		/// <param name="correlationContextAccessor"></param>
		public CorrelationIdDelegatingHandler(ICorrelationContextAccessor correlationContextAccessor)
        {
			_correlationContextAccessor = correlationContextAccessor ?? throw new ArgumentNullException(nameof(correlationContextAccessor));
        }

		#endregion

		#region PROPERTIES
		#endregion

		#region METHODS

		/// <inheritdoc cref="DelegatingHandler"/>
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
            if (!request.Headers.Contains(_correlationContextAccessor.Context.HeaderKey))
            {
				request.Headers.Add(_correlationContextAccessor.Context.HeaderKey, _correlationContextAccessor.Context.CorrelationId);
            }

			return base.SendAsync(request, cancellationToken);
		}

		#endregion

		#endregion
	}
}
