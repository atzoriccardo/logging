using System;
using System.Collections.Generic;
using System.Text;

namespace Vittoria.Infrastructure.CorrelationId
{
    /// <summary>
    /// 
    /// </summary>
    public class CorrelationIdContext
    {
        #region DYNAMIC

        #region FIELDS
        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Crea una nuova istanza della classe <see cref="Vittoria.Infrastructure.CorrelationId.CorrelationIdContext"/>
        /// </summary>
        /// <param name="correlationId">CorrelationId impostato nel contesto</param>
        /// <param name="headerKey">Chiave utilizzata per contenere il CorrelationId nella Request corrente.</param>
        public CorrelationIdContext(string correlationId, string headerKey)
        {
            CorrelationId = correlationId;
            HeaderKey = headerKey;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Restituisce il nome dell'header contenente il CorrelationId per la Request corrente.
        /// </summary>
        public string HeaderKey { get; }

        /// <summary>
        /// Restituisce il CorrelationId per la Request Corrente.
        /// </summary>
        public string CorrelationId { get; }

        #endregion

        #region METHODS
        #endregion

        #endregion
    }
}
