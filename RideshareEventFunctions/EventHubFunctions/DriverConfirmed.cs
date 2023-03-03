using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class DriverConfirmed
    {
        private readonly ILogger _logger;

        public DriverConfirmed(ILogger<DriverConfirmed> logger)
        {
            _logger = logger;
        }

        [Function("DriverConfirmed")]
        public void Run([EventHubTrigger("driverconfirmed", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            DriverConfirmedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<DriverConfirmedEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserealized into a DriverConfirmedEvent");
            }

            _logger.LogInformation($"DriverConfirmed event triggered.");

            //todo: Update ride state and notify FE of update
        }
    }
}
