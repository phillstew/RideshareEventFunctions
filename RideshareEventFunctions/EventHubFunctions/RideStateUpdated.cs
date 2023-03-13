using Microsoft.Azure.Functions.Worker;
using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using RideshareEventFunctions.Models.HubEvents;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace RideshareEventFunctions.EventHubFunctions
{
    public class RideStateUpdated
    {
        private readonly ILogger _logger;

        public RideStateUpdated(ILogger<RideStateUpdated> logger) 
        { 
            _logger = logger;
        }

        [Function("negotiate")]
        public HttpResponseData Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequestData req,
        [SignalRConnectionInfoInput(HubName = "ridestate", ConnectionStringSetting = "AzureSignalRConnectionString", UserId = "{query.userid}")] string connectionInfo)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            response.WriteString(connectionInfo);
            return response;
        }

        [Function("SubscribeToRide")]
        [SignalROutput(HubName = "ridestate", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public SignalRGroupAction SubscribeToRide(
            [SignalRTrigger("ridestate", "messages", "subscribeToRide", "subscriptionId", "userId", ConnectionStringSetting = "AzureSignalRConnectionString")] SignalRInvocationContext invocationContext, 
            string subscriptionId,
            string userId)
        {
            _logger.LogInformation($"Connection {invocationContext.ConnectionId} sent a message. Message content: {subscriptionId}");
            return new SignalRGroupAction(SignalRGroupActionType.Add)
            {
                GroupName = subscriptionId,
                UserId = userId
            };
        }

        [Function("RideStateUpdated")]
        [SignalROutput(HubName = "ridestate", ConnectionStringSetting = "AzureSignalRConnectionString")]
        public SignalRMessageAction Run(
            [EventHubTrigger("ridestateupdated", Connection = "AzureEventHubConnectionString", IsBatched = false)] string input)
        {
            RideStateUpdatedEvent model = null;
            try
            {
                model = JsonSerializer.Deserialize<RideStateUpdatedEvent>(input);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Event was unable to be deserialized into a DriverConfirmedEvent");
            }

            _logger.LogInformation($"Ride {model.RideId} state updated to {model.State}");

            return new SignalRMessageAction("stateUpdated", new[] { JsonSerializer.Serialize(model) })
            {
                GroupName = $"ride-{model.RideId}"
            };
        }
    }
}
