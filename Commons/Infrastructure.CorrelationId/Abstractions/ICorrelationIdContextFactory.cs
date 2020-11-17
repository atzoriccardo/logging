using System;
using System.Collections.Generic;
using System.Text;

namespace Vittoria.Infrastructure.CorrelationId
{
    /// <summary>
    /// Factory per la creazione di un'istanza del <see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdContext"/>
    /// </summary>
    public interface ICorrelationIdContextFactory
    {
        #region DYNAMIC

        #region PROPERTIES
        #endregion

        #region METHODS

        /// <summary>
        /// Crea una nuova instanza del <see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdContext"/> con il CorrelationId impostato per la request corrente.
        /// </summary>
        /// <param name="correlationId">CorrelationId impostato nel contesto</param>
        /// <param name="headerKey">Chiave utilizzata per contenere il CorrelationId nella Request corrente.</param>
        /// <returns></returns>
        CorrelationIdContext Create(string correlationId, string headerKey);

        #endregion

        #endregion
    }
}
