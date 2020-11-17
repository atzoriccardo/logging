using System;
using System.Collections.Generic;
using System.Text;

namespace Vittoria.Infrastructure.CorrelationId
{
    /// <inheritdoc />
    public class CorrelationIdContextFactory : ICorrelationIdContextFactory
    {
        #region DYNAMIC

        #region FIELDS

        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Crea una nuova istanza della classe <see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdContextAccessor"/>
        /// </summary>
        /// <param name="correlationContextAccessor">Istanza del Context Accessor utilizzata per impostare il Context per la Request corrente.</param>
        public CorrelationIdContextFactory(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        #endregion

        #region PROPERTIES
        #endregion

        #region METHODS

        /// <inheritdoc />
        public CorrelationIdContext Create(string correlationId, string headerKey)
        {
            var context = new CorrelationIdContext(correlationId, headerKey);

            if (_correlationContextAccessor != null)
            {
                _correlationContextAccessor.Context = context;
            }

            return context;
        }

        #endregion

        #endregion
    }
}
