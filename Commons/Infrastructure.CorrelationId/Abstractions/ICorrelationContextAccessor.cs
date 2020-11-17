using System;
using System.Collections.Generic;
using System.Text;

namespace Vittoria.Infrastructure.CorrelationId
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICorrelationContextAccessor
    {
        #region DYNAMIC

        #region PROPERTIES
        #endregion

        #region METHODS

        /// <summary>
        /// Contesto della Request corrente.
        /// </summary>
        CorrelationIdContext Context { get; set; }

        #endregion

        #endregion
    }
}
