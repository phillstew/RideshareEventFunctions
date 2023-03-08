using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RideshareEventFunctions.EventHubFunctions
{
    public  class DriverRequested
    {
        private readonly ILogger _logger;
        private readonly IRideService _rideService;
        private readonly IRideShareEventProducer _rideShareEventProducer;

        public DriverRequested(ILogger<DriverRequested> logger, IRideService rideService, IRideShareEventProducer rideShareEventProducer)
        {
            _logger = logger;
            _rideShareEventProducer = rideShareEventProducer;
            _rideService = rideService;
        }

        [Function("DriverRequested")]
        [EventHubOutput("driverfound", Connection = "AzureEventHubConnectionString")]
        public async Task<string> Run([EventHubTrigger("driverrequested", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            DriverRequestedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<DriverRequestedEvent>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a DriverRequestedEvent");
            }

            _logger.LogInformation($"DriverRequested event triggered.");

            await _rideShareEventProducer.SendEvent("ridestateupdated", new RideStateUpdatedEvent { RideId = model.RideId, State = "DRIVER_REQUESTED" });

            var driver = await _rideService.FindDriverForRide(model.RideId);

            // todo: Look up the history for a ride and find the next available driver who has not rejected this ride
            // lock that driver so they cannot receive any more requests and wait for that driver to accept/reject

            return JsonSerializer.Serialize(new DriverFoundEvent { DriverId = driver.DriverID });
        }
    }
}
