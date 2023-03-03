using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RideshareEventFunctions.Configs;
using RideshareEventFunctions.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RideshareEventFunctions.Services
{
    internal class RideShareEventProducer : IRideShareEventProducer
    {
        private readonly EventHubsConfig _config;
        private readonly Dictionary<string, EventHubProducerClient> _eventHubClients;

        public RideShareEventProducer(IConfiguration config)
        {
            _config = new EventHubsConfig { AzureEventHubConnectionString = config["AzureEventHubConnectionString"] };
            _eventHubClients = new Dictionary<string, EventHubProducerClient>();
        }

        public async Task SendEvent<T>(string hubName, T eventToSend)
        {
            EventHubProducerClient client = null;
            if (_eventHubClients.ContainsKey(hubName))
            {
                client = _eventHubClients[hubName];
            }
            else
            {
                client = new EventHubProducerClient(
                    _config.AzureEventHubConnectionString,
                    hubName);
                _eventHubClients.Add(hubName, client);
            }

            using EventDataBatch eventBatch = await client.CreateBatchAsync();

            var jsonData = JsonSerializer.Serialize(eventToSend);
            if (!eventBatch.TryAdd(new EventData(jsonData)))
            {
                throw new Exception($"Event is too large for the batch and cannot be sent.");
            }

            await client.SendAsync(eventBatch);
        }
    }
}
