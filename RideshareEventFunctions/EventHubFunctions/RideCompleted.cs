using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class RideCompleted
    {
        private readonly ILogger _logger;
        private readonly IRideService _rideService;
        private readonly IRideShareEventProducer _rideShareEventProducer;

        public RideCompleted(ILogger<RideCompleted> logger, IRideService rideService, IRideShareEventProducer rideShareEventProducer)
        {
            _logger = logger;
            _rideShareEventProducer = rideShareEventProducer;
            _rideService = rideService;
        }

        [Function("RideCompleted")]
        public async Task Run([EventHubTrigger("ridecompleted", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RideCompletedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RideCompletedEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a RideCompletedEvent");
            }

            _logger.LogInformation($"RideCompleted event triggered.");

            await _rideShareEventProducer.SendEvent("ridestateupdated", new RideStateUpdatedEvent { RideId = model.RideId, State = "RIDE_COMPLETED" });

            //todo: Mark the ride as completed, free up the driver + passanger, and notify the FE of the new state
            await _rideService.UpdateRideStatus(model.RideId, "COMPLETED");
        }
    }
}
