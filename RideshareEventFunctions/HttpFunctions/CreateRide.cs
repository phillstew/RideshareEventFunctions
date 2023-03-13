using System;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using RideshareEventFunctions.Models.HubEvents;
using RideshareEventFunctions.Services.Interfaces;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class CreateRide
    {
        private readonly ILogger _logger;
        private readonly IRideShareEventProducer _eventProducer;

        public CreateRide(ILogger<RideCreated> logger, IRideShareEventProducer rideShareEventProducer)
        {
            _logger = logger;
            _eventProducer = rideShareEventProducer;
        }

        [Function("CreateRide")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, int rideId)
        {
            _logger.LogInformation("CreateRide Http function entered");

            var hubName = "ridecreated";
            var ride = new RideCreatedEvent()
            {
                RideId = rideId,
                DateTimeCreated = DateTime.Now
            };
            await _eventProducer.SendEvent(hubName, ride); // todo: correct Event to send?

            var msg = string.Format("EventHub Event sent: {0} with id {1} at {2}", hubName, ride.RideId, ride.DateTimeCreated);
            _logger.LogInformation(msg);

            var response = req.CreateResponse(HttpStatusCode.OK);

            return response;
        }
    }
}