using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace RideshareEventFunctions.RideCreated
{
    public class RideCreated
    {
        private readonly ILogger _logger;

        public RideCreated(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RideCreated>();
        }

        [Function("RideCreated")]
        [EventHubOutput("driverfound", Connection = "AzureEventHubConnectionString")]
        public string Run([EventHubTrigger("ridecreated", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            _logger.LogInformation($"First Event Hubs triggered message: {input}");

            return $"Driver {input} found for ride";
        }
    }
}
