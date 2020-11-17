using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Vittoria.Infrastructure.Logging
{
    public class VittoriaJsonFormatter : ITextFormatter
    {   
        private readonly JsonValueFormatter _valueFormatter;

        public VittoriaJsonFormatter()
        {   
            _valueFormatter = new JsonValueFormatter(typeTagName: "$type");
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            // LogLevel
            output.Write("{\"Level\":\"");
            output.Write(logEvent.Level);

            // Time
            output.Write("\",\"Time\":\"");
            output.Write(logEvent.Timestamp.ToUnixTimeMilliseconds());

            // RequestId
            var hasCorrelationId = logEvent.Properties.TryGetValue("CorrelationId", out LogEventPropertyValue correlationId);
            output.Write("\",\"RequestId\":\"");

            if (hasCorrelationId)
            {
                output.Write(((ScalarValue)correlationId).Value);
            }
            else
            {
                output.Write("null");
            }

            // Message
            output.Write("\",\"Message\":");
            var message = logEvent.MessageTemplate.Render(logEvent.Properties);
            JsonValueFormatter.WriteQuotedJsonString(message, output);

            // Exception
            output.Write(",\"Exception\":");

            if (logEvent.Exception != null)
            {
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }
            else
            {
                output.Write("null");
            }

            // Http Request
            var hasHttpContext = logEvent.Properties.TryGetValue("HasHttpRequest", out LogEventPropertyValue context);

            if (hasHttpContext)
            {
                output.Write(",\"Http\":{");

                // Request
                output.Write("\"Request\":{");

                var path = logEvent.Properties.FirstOrDefault(x => x.Key == "Url").Value;
                output.Write("\"Path\":");
                output.Write(path.ToString());

                var method = logEvent.Properties.FirstOrDefault(x => x.Key == "Method").Value;
                output.Write(",\"Method\":");
                output.Write(method.ToString());

                var query = logEvent.Properties.FirstOrDefault(x => x.Key == "Query").Value;    
                output.Write(",\"Query\":");
                output.Write(query.ToString());

                var requestBody = logEvent.Properties.FirstOrDefault(x => x.Key == "RequestBody").Value;
                output.Write(",\"Body\":");
                JsonValueFormatter.WriteQuotedJsonString(requestBody.ToString(), output);

                var contentType = logEvent.Properties.FirstOrDefault(x => x.Key == "ContentType").Value;
                output.Write(",\"ContentType\":");
                output.Write(contentType.ToString());

                var scheme = logEvent.Properties.FirstOrDefault(x => x.Key == "Scheme").Value;
                output.Write(",\"Scheme\":");
                output.Write(scheme.ToString());

                var protocol = logEvent.Properties.FirstOrDefault(x => x.Key == "Protocol").Value;
                output.Write(",\"Protocol\":");
                output.Write(protocol.ToString());

                var userAgent = logEvent.Properties.FirstOrDefault(x => x.Key == "UserAgent").Value;
                output.Write(",\"UserAgent\":");
                output.Write(userAgent.ToString());
                
                output.Write("}");

                // Response
                var hasHttpResponse = logEvent.Properties.TryGetValue("HasHttpResponse", out LogEventPropertyValue response);

                if (hasHttpResponse)
                {
                    output.Write(",\"Response\":{");

                    var statusCode = logEvent.Properties.FirstOrDefault(x => x.Key == "StatusCode").Value;
                    output.Write("\"StatusCode\":");
                    output.Write(statusCode);

                    var responseBody = logEvent.Properties.FirstOrDefault(x => x.Key == "ResponseBody").Value;
                    output.Write(",\"Response\":");
                    output.Write(responseBody.ToString());
                    
                    var responseTime = logEvent.Properties.FirstOrDefault(x => x.Key == "ResponseTime").Value;
                    output.Write(",\"ResponseTime\":");
                    output.Write(responseTime);

                    var responseBytes = logEvent.Properties.FirstOrDefault(x => x.Key == "ResponseBytes").Value;
                    output.Write(",\"Bytes\":");
                    output.Write(responseBytes);

                    output.Write("}");
                }

                output.Write("}");

                // Host
                output.Write(",\"Host\":{");

                var hostname = logEvent.Properties.FirstOrDefault(x => x.Key == "Hostname").Value;
                output.Write("\"Hostname\":");
                output.Write(hostname.ToString());

                var forwardedHostname = logEvent.Properties.FirstOrDefault(x => x.Key == "ForwardedHostname").Value;
                output.Write(",\"ForwardedHostname\":");
                output.Write(forwardedHostname.ToString());

                var ip = logEvent.Properties.FirstOrDefault(x => x.Key == "Ip").Value;
                output.Write(",\"Ip\":");
                output.Write(ip.ToString());

                output.Write("}");

            }

            output.Write("}");
            output.Write("\n");
        }
    }
}
