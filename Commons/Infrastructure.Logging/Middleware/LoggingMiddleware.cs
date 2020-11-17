using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vittoria.Infrastructure.Logging
{
    public class LoggingMiddleware
    {
        private const string UserAgentHeaderKey = "User-Agent";
        private const string ForwardedHostHeaderKey = "x-forwarded-host";
        private const string CorrelationIdHeaderKey = "X-Correlation-ID";

        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly Dictionary<string, object> _loggingDictionary;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loggingDictionary = new Dictionary<string, object>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await BuildIncomingRequest(httpContext);

                using (_logger.BeginScope(new Dictionary<string, object>() { { "CorrelationId", httpContext.Request.Headers[CorrelationIdHeaderKey].ToString() } }))
                {
                    await _next(httpContext);
                }

                await BuildCompletedRequest(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            var result = JsonConvert.SerializeObject(new { UserMessage = "Impossibile completare l'operazione" });
            var data = Encoding.UTF8.GetBytes(result);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            
            await BuildCompletedRequest(context);
        }

        private async Task BuildIncomingRequest(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var hasRequestBody = request.ContentLength > 0;
            var body = string.Empty;

            if (hasRequestBody)
            {
                body = await GetRequestBody(request);
            }

            var query = request.Query.ToList();
            var forwardedHostname = request.Headers[ForwardedHostHeaderKey].ToString();
            var userAgent = request.Headers[UserAgentHeaderKey].ToString();
            var correlationId = GetOrCreateCorrelationId(request);

            _loggingDictionary.Add("HasHttpRequest", true);
            _loggingDictionary.Add("CorrelationId", correlationId);
            _loggingDictionary.Add("Method", request.Method.ToString());
            _loggingDictionary.Add("Url", request.Path);
            _loggingDictionary.Add("RequestBody", (hasRequestBody) ? body : null);
            _loggingDictionary.Add("Query", (query != null && query.Count > 0) ? query : null);
            _loggingDictionary.Add("Scheme", request.Scheme);
            _loggingDictionary.Add("ContentType", request.ContentType);
            _loggingDictionary.Add("Protocol", request.Protocol);
            _loggingDictionary.Add("Ip", httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            _loggingDictionary.Add("Hostname", RemovePort(request.Host.ToString()));
            _loggingDictionary.Add("ForwardedHostname", (!string.IsNullOrEmpty(forwardedHostname)) ? forwardedHostname : null);
            _loggingDictionary.Add("UserAgent", (!string.IsNullOrEmpty(userAgent)) ? userAgent : null);

            using (_logger.BeginScope(_loggingDictionary))
            {
                _logger.LogInformation("Incoming Request");
            }
        }

        private async Task BuildCompletedRequest(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var response = httpContext.Response;

            var responseBody = string.Empty;
            var passedMicroSecond = default(decimal);
            var responseBytes = default(long);

            var originalResponseBody = response.Body;

            using (var responseBodyStream = new MemoryStream())
            {
                //var bodyStream = response.Body;
                response.Body = responseBodyStream;

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                responseBodyStream.Seek(0, SeekOrigin.Begin);

                await responseBodyStream.CopyToAsync(originalResponseBody);

                //timeWatch.Stop();
                //passedMicroSecond = timeWatch.ElapsedMilliseconds / 1000m;

                //responseBodyStream.Position = 0;
                //responseBody = new StreamReader(responseBodyStream).ReadToEnd();
                //responseBytes = response.ContentLength ?? responseBodyStream.Length;

                //responseBodyStream.Position = 0;
                //responseBodyStream.CopyToAsync(bodyStream).GetAwaiter().GetResult();
            }

            _loggingDictionary.Add("HasHttpResponse", (!string.IsNullOrEmpty(responseBody)));
            _loggingDictionary.Add("StatusCode", httpContext.Response.StatusCode);
            _loggingDictionary.Add("ResponseBody", (!string.IsNullOrEmpty(responseBody)) ? responseBody : null);
            _loggingDictionary.Add("ResponseTime", passedMicroSecond);
            _loggingDictionary.Add("ResponseBytes", responseBytes);

            using (_logger.BeginScope(_loggingDictionary))
            {
                _logger.LogInformation("Completed Request");
            }

            _loggingDictionary.Clear();   
        }

        private string GetOrCreateCorrelationId(HttpRequest request)
        {
            var hasCorrelationId = request.Headers.TryGetValue(CorrelationIdHeaderKey, out var cid);
            var correlationId = hasCorrelationId ? cid.FirstOrDefault() : null;

            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                request.Headers.Add(CorrelationIdHeaderKey, correlationId);
            }

            return correlationId;
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            var result = string.Empty;

            request.EnableBuffering();

            using (var buffer = new MemoryStream())
            {
                request.Body.Seek(0, SeekOrigin.Begin);
                await request.Body.CopyToAsync(buffer);
                result = Encoding.UTF8.GetString(buffer.ToArray());
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }

        private string RemovePort(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            var hostComponents = host.Split(":");
            return hostComponents[0];
        }
    }
}
