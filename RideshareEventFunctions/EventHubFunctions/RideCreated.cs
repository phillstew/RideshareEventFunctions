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
        private readonly IRideShareEventProducer _rideShareEventProducer;

        public RideCreated(ILogger<RideCreated> logger, IRideShareEventProducer rideShareEventProducer)
        {
            _logger = logger;
            _rideShareEventProducer = rideShareEventProducer;
        }

        [Function("RideCreated")]
        [EventHubOutput("driverrequested", Connection = "AzureEventHubConnectionString")]
        public async Task<string> Run(
            [EventHubTrigger("ridecreated", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RideCreatedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RideCreatedEvent>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a RideCreatedEvent");
            }

            await _rideShareEventProducer.SendEvent("ridestateupdated", new RideStateUpdatedEvent { RideId = model.RideId, State = "CREATED" });

            _logger.LogInformation($"RideCreated function hit.");


            await Task.Delay(TimeSpan.FromSeconds(5));

            // The two methods of sending events
            // 1. Send through our RideShareEventProducer _eventProducer.SendEvent("driverfound", new DriverFoundEvent { DriverId = 5 });
            // 2. Send as JSON through [EventHubOutput]
            // Todo: Centralize json conversion between handlers
            return JsonSerializer.Serialize(new DriverRequestedEvent { RideId = model.RideId });
        }
    }
}
