using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GBelenky.WebHook
{
    public class WebHook
    {
        private readonly ILogger _logger;

        public WebHook(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WebHook>();
        }

        [Function("WebHook")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "options")] HttpRequestData req)
        {
            //read payload into string
            string? payload = req.ReadAsString();
            _logger.LogInformation($"Event payload: {payload}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(payload);

            return response;
        }
    }
}
