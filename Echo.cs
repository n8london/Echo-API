using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace GBelenky.WebHook
{
    public class Echo
    {
        private readonly ILogger _logger;

        public Echo(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Echo>();
        }

        [Function("Echo")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get")] HttpRequestData req)
        {
            // is it GET or POST request
            string? method = req.Method;
            _logger.LogInformation($"Method: {method}");
            string responseString = "No payload, no query string";

            if (method == "GET")
            {
                string? query = req.Url.Query;
                if (!String.IsNullOrEmpty(query))
                {
                    responseString = query;
                }
                _logger.LogInformation($"Query string: {responseString}");
            }
            else
            {
                string? payload = req.ReadAsString();
                if (!String.IsNullOrEmpty(payload))
                {
                    responseString = payload;
                }
                _logger.LogInformation($"Event payload: {responseString}");
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            response.WriteString(responseString);

            return response;
        }
    }
}
