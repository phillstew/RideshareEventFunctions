using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class RideCompleted
    {
        private readonly ILogger _logger;

        public RideCompleted(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RideCompleted>();
        }

        [Function("RideCompleted")]
        public void Run([EventHubTrigger("ridecompleted", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RideCompletedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RideCompletedEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserealized into a RideCompletedEvent");
            }

            _logger.LogInformation($"RideCompleted event triggered.");

            //todo: Mark the ride as completed, free up the driver + passanger, and notify the FE of the new state
        }
    }
}
