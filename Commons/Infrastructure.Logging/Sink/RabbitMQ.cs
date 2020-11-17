using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vittoria.Infrastructure.Logging
{
    public class RabbitMQ : ILogEventSink
    {

        public void Emit(LogEvent logEvent)
        {
            var collectionId = logEvent.Properties.FirstOrDefault(x => x.Key == "CorrelationId");

            throw new NotImplementedException();
        }
    }
}
