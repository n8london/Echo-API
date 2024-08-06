using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace Company.Function
{
    public class Echo
    {
        private readonly ILogger<Echo> _logger;

        public Echo(ILogger<Echo> logger)
        {
            _logger = logger;
        }

        [Function("SendEchoRequest")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
        
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            var requestBodyJson = JsonDocument.Parse("{}");
            
            if(!string.IsNullOrEmpty(requestBody))
            {
                requestBodyJson = JsonDocument.Parse(requestBody);
            }

            string requestHeaders = req.Headers?.ToString() ?? string.Empty;

            // Get all query parameters
            var queryParameters = req.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            return new OkObjectResult(new
            {
                RequestBody = requestBodyJson,
                RequestHeaders = requestHeaders,
                QueryParameters = queryParameters
            });
        }
    }
}
