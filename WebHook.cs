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

        [Function("Payload")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            //read payload into string
            string? payload = req.ReadAsString();
            _logger.LogInformation($"Event payload: {payload}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(payload);

            return response;
        }

        // create function returning query string
        [Function("QueryString")]
        public HttpResponseData QueryString([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            //read query string into string
            string? query = req.Url.Query;
            _logger.LogInformation($"Query string: {query}");

            //parse the query string into a dictionary
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(query);
            // iterate over queryDictionary and serialize it into json string using system.text.json
            string json = System.Text.Json.JsonSerializer.Serialize(queryDictionary);
            _logger.LogInformation($"Query string as json: {json}");
            

            

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString(query);

            return response;
        }        
    }
}
