using System;
using System.Collections.Generic;
using System.Text;

namespace Vittoria.Infrastructure.Logging.Sink
{
    public class RabbitMQOptions
    {
        public string EndpointList { get; set; }

        public string VirtualHost { get; set; }


    }
}
