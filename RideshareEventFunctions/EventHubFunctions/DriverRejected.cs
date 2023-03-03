using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class DriverRejected
    {
        private readonly ILogger _logger;

        public DriverRejected(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DriverConfirmed>();
        }

        [Function("DriverRejected")]
        public void Run([EventHubTrigger("driverrejected", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            DriverRejectedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<DriverRejectedEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserealized into a DriverRejectedEvent");
            }

            _logger.LogInformation($"DriverRejected event triggered.");

            // todo: Search for a new driver. Add history item for driver ride rejects so we don't try to match the same driver up twice?
            // Maybe we split up searching for a driver into a separate function
        }
    }
}
