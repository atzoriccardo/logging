using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Vittoria.Infrastructure.CorrelationId
{
    /// <inheritdoc />
    public class CorrelationIdContextAccessor : ICorrelationContextAccessor
    {
        #region DYNAMIC

        #region FIELDS

        private static readonly AsyncLocal<CorrelationIdContext> _context = new AsyncLocal<CorrelationIdContext>();

        #endregion

        #region CONSTRUCTORS
        #endregion

        #region PROPERTIES

        /// <inheritdoc />
        public CorrelationIdContext Context
        {
            get => _context.Value;
            set => _context.Value = value;
        }

        #endregion

        #region METHODS
        #endregion

        #endregion
    }
}
