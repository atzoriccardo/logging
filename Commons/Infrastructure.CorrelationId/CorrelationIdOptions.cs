using System;
using System.Collections.Generic;
using System.Text;

namespace Vittoria.Infrastructure.CorrelationId
{
    public class CorrelationIdOptions
    {
        public string RequestHeaderKey { get; set; } = "X-Correlation-ID";

        public bool AddLoggingToScope { get; set; } = true;

        public string LoggerScopeKey { get; set; } = "CorrelationId";

        public bool IncludeInResponse { get; set; } = true;

        
    }
}
