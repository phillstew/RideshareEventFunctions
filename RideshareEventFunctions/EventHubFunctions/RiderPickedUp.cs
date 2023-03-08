using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class RiderPickedUp
    {
        private readonly ILogger _logger;
        private readonly IRideService _rideService;
        private readonly IRideShareEventProducer _rideShareEventProducer;

        public RiderPickedUp(ILogger<RiderPickedUp> logger, IRideService rideService, IRideShareEventProducer rideShareEventProducer)
        {
            _logger = logger;
            _rideShareEventProducer = rideShareEventProducer;
            _rideService = rideService;
        }

        [Function("RiderPickedUp")]
        public async Task Run([EventHubTrigger("riderpickedup", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RiderPickedUpEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RiderPickedUpEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a RiderPickedUpEvent");
            }

            _logger.LogInformation($"RiderPickedUp event triggered.");

            await _rideService.UpdateRideStatus(model.RideId, "PICKED_UP");
            await _rideShareEventProducer.SendEvent("ridestateupdated", new RideStateUpdatedEvent { RideId = model.RideId, State = "PICKED_UP" });

            // Mock out actual drive time
            await Task.Delay(TimeSpan.FromSeconds(5));
            await _rideShareEventProducer.SendEvent("ridecompleted", new RideCompletedEvent { RideId = model.RideId });
        }
    }
}
