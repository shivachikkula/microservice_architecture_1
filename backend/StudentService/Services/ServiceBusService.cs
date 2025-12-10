using Azure.Messaging.ServiceBus;
using System.Text.Json;

namespace StudentService.Services;

public class ServiceBusService : IServiceBusService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;
    private readonly ILogger<ServiceBusService> _logger;

    public ServiceBusService(IConfiguration configuration, ILogger<ServiceBusService> logger)
    {
        _logger = logger;
        var connectionString = configuration["ServiceBus:ConnectionString"];
        var queueName = configuration["ServiceBus:QueueName"];

        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(queueName))
        {
            _logger.LogWarning("Service Bus connection string or queue name is not configured");
            _client = null!;
            _sender = null!;
            return;
        }

        _client = new ServiceBusClient(connectionString);
        _sender = _client.CreateSender(queueName);
    }

    public async Task SendMessageAsync(string message)
    {
        try
        {
            if (_sender == null)
            {
                _logger.LogWarning("Service Bus sender is not initialized. Message not sent.");
                return;
            }

            var serviceBusMessage = new ServiceBusMessage(message)
            {
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString()
            };

            await _sender.SendMessageAsync(serviceBusMessage);
            _logger.LogInformation("Message sent to Service Bus: {MessageId}", serviceBusMessage.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to Service Bus");
            throw;
        }
    }
}
