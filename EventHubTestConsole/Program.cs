using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;
using System.Text.Json;


while (true)
{
    Console.WriteLine("Select an option to test:");
    Console.WriteLine("Press S to send a RideCreatedEvent");
    Console.WriteLine("Press Q to quit...");

    var input = Console.ReadKey();
    Console.WriteLine();

    switch (input.Key)
    {
        case ConsoleKey.S:
            int id;
            string idInput;
            do
            {
                Console.WriteLine("Please enter a ride ID? (int)");
                idInput = Console.ReadLine();
            } while (!int.TryParse(idInput, out id));

            await SendRideCreatedEvent(id);
            break;
        case ConsoleKey.Q:
            return;
    }
}


async Task SendRideCreatedEvent(int id)
{
    EventHubProducerClient producerClient = new EventHubProducerClient(
    "Endpoint=sb://pstewarttest.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IqkxlawjLHWkaoIYUDc6XvWqJyOx0xEoU+AEhKQPNeQ=",
    "ridecreated");

    using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

    for (int i = 0; i < 1; i++)
    {
        if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new RideCreatedEvent { RideId = id })))))
        {
            throw new Exception($"Event {i} is too large for the batch and cannot be sent.");
        }
    }

    try
    {

        await producerClient.SendAsync(eventBatch);
        Console.WriteLine($"Send a batch of messages {eventBatch.Count}");
    }
    finally
    {
        await producerClient.DisposeAsync();
    }
}

public class RideCreatedEvent
{
    public int RideId { get; set; }

    public DateTimeOffset DateTimeCreated { get; set; } = DateTimeOffset.Now;
}