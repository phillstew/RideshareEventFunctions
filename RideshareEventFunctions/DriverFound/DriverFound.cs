using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace RideshareEventFunctions.DriverFound
{
    public class DriverFound
    {
        private readonly ILogger _logger;

        public DriverFound(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DriverFound>();
        }

        [Function("RideCreated")]
        //[EventHubOutput("demohub", Connection = "AzureEventHubConnectionString")]
        public void Run([EventHubTrigger("driverfound", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            _logger.LogInformation($"Found a driver: {input}");
        }
    }
}
