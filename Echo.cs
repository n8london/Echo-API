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
            string? responseString = null;

            if (method == "GET")
            {
                string? query = req.Url.Query;
                _logger.LogInformation($"Query string: {query}");
                responseString = query;
            }
            else
            {
                //read payload into string
                string? payload = req.ReadAsString();
                _logger.LogInformation($"Event payload: {payload}");
                responseString = payload;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (responseString != null)
            {
                response.WriteString(responseString);
            }

            return response;
        }
    }
}
