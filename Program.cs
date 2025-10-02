
// this is an example of how to use azure events hubs to send messages.

using System.Threading.Tasks;
using Azure.Messaging.EventHubs.Producer;

public class Program
{
    private static readonly string connectionString = Environment.GetEnvironmentVariable("EVENT_HUB_CONNECTION") 
        ?? throw new InvalidOperationException("EVENT_HUBS_CONNECTION environment variable is not set.");
    private const string eventHubName = "myeventhubdemo";

    public static async Task Main(string[] args)
    {
        await using var producer = new EventHubProducerClient(connectionString, eventHubName);
        using EventDataBatch eventBatch = await producer.CreateBatchAsync();

        for (int i = 0; i < 10; i++)
        {
            eventBatch.TryAdd(new Azure.Messaging.EventHubs.EventData($"Message {i}"));
            Console.WriteLine($"Added Message {i} to the batch.");
        }

        await producer.SendAsync(eventBatch);
        Console.WriteLine("A batch of 10 events has been published.");
    }
}