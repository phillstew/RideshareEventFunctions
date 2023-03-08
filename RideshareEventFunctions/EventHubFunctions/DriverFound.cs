using System;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class DriverFound
    {
        private readonly ILogger _logger;
        private readonly IRideShareEventProducer _rideShareEventProducer;

        public DriverFound(ILogger<DriverFound> logger, IRideShareEventProducer rideShareEventProducer)
        {
            _logger = logger;
            _rideShareEventProducer = rideShareEventProducer;
        }

        [Function("DriverFound")]
        public async Task Run([EventHubTrigger("driverfound", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            DriverFoundEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<DriverFoundEvent>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a DriverFoundEvent");
            }

            _logger.LogInformation($"Found a driver: {model.DriverId}");

            await _rideShareEventProducer.SendEvent("ridestateupdated", new RideStateUpdatedEvent { RideId = model.RideId, State = "DRIVER_FOUND" });

            // Temporarily mock an Accept/Reject message
            await Task.Delay(TimeSpan.FromSeconds(5));

            // Pseudorandom
            Random random = new Random();
            var randomVal = random.Next(1, 10);
            if (randomVal == 10)
                await _rideShareEventProducer.SendEvent("driverrejected", new DriverRejectedEvent { RideId = model.RideId, DriverId = model.DriverId });
            else
                await _rideShareEventProducer.SendEvent("driverconfirmed", new DriverConfirmedEvent { RideId = model.RideId, DriverId = model.DriverId });
        }
    }
}
