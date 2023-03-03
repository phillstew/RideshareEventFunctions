using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class DriverFound
    {
        private readonly ILogger _logger;

        public DriverFound(ILogger<DriverFound> logger)
        {
            _logger = logger;
        }

        [Function("DriverFound")]
        public void Run([EventHubTrigger("driverfound", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            DriverFoundEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<DriverFoundEvent>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event was unable to be deserealized into a DriverFoundEvent");
            }

            _logger.LogInformation($"Found a driver: {model.DriverId}");

            // todo: Notify FE of potential driver

        }
    }
}
