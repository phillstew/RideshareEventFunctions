using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class RiderPickedUp
    {
        private readonly ILogger _logger;

        public RiderPickedUp(ILogger<RiderPickedUp> logger)
        {
            _logger = logger;
        }

        [Function("RiderPickedUp")]
        public void Run([EventHubTrigger("riderpickedup", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RiderPickedUpEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RiderPickedUpEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserealized into a RiderPickedUpEvent");
            }

            _logger.LogInformation($"RiderPickedUp event triggered.");

            // todo: Update ride state and notify FE
        }
    }
}
