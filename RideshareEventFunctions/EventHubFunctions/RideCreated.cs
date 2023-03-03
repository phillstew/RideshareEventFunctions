using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class RideCreated
    {
        private readonly ILogger _logger;

        public RideCreated(ILogger<RideCreated> logger)
        {
            _logger = logger;
        }

        [Function("RideCreated")]
        [EventHubOutput("driverfound", Connection = "AzureEventHubConnectionString")]
        public async Task<string> Run([EventHubTrigger("ridecreated", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RideCreatedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RideCreatedEvent>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event was unable to be deserealized into a RideCreatedEvent");
            }

            _logger.LogInformation($"RideCreated function hit.");

            // todo: Perform driver search

            await Task.Delay(TimeSpan.FromSeconds(5));

            // The two methods of sending events
            // 1. Send through our RideShareEventProducer _eventProducer.SendEvent("driverfound", new DriverFoundEvent { DriverId = 5 });
            // 2. Send as JSON through [EventHubOutput]
            // Todo: Centralize json conversion between handlers
            return JsonSerializer.Serialize(new DriverFoundEvent { DriverId = 5 });
        }
    }
}
