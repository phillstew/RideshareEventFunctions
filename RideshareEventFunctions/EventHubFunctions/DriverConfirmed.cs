using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class DriverConfirmed
    {
        private readonly ILogger _logger;
        private readonly IRideShareEventProducer _rideShareEventProducer;
        private readonly IRideService _rideService;

        public DriverConfirmed(ILogger<DriverConfirmed> logger, IRideShareEventProducer rideShareEventProducer, IRideService rideService)
        {
            _logger = logger;
            _rideService = rideService;
            _rideShareEventProducer = rideShareEventProducer;
        }

        [Function("DriverConfirmed")]
        public async Task Run([EventHubTrigger("driverconfirmed", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            DriverConfirmedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<DriverConfirmedEvent>(input);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a DriverConfirmedEvent");
            }

            _logger.LogInformation($"DriverConfirmed event triggered.");

            await _rideShareEventProducer.SendEvent("ridestateupdated", new RideStateUpdatedEvent { RideId = model.RideId, State = "DRIVER_CONFIRMED" });


            // Mock time for driver arrival
            await Task.Delay(TimeSpan.FromSeconds(3));

            // Mock driver action to pick up rider
            await _rideShareEventProducer.SendEvent("riderpickedup", new RiderPickedUpEvent { RideId = model.RideId });
        }
    }
}
