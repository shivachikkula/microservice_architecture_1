using Azure.Messaging.ServiceBus;
using MessageConsumerService.Models;
using System.Text.Json;

namespace MessageConsumerService.Services;

public class ServiceBusConsumerService : IServiceBusConsumerService
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusProcessor _processor;
    private readonly ILogger<ServiceBusConsumerService> _logger;

    public ServiceBusConsumerService(IConfiguration configuration, ILogger<ServiceBusConsumerService> logger)
    {
        _logger = logger;
        var connectionString = configuration["ServiceBus:ConnectionString"];
        var queueName = configuration["ServiceBus:QueueName"];

        if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(queueName))
        {
            _logger.LogError("Service Bus connection string or queue name is not configured");
            throw new InvalidOperationException("Service Bus configuration is missing");
        }

        _client = new ServiceBusClient(connectionString);
        _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1,
            ReceiveMode = ServiceBusReceiveMode.PeekLock
        });

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        _logger.LogInformation("Service Bus Consumer Service initialized for queue: {QueueName}", queueName);
    }

    public async Task StartProcessingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Service Bus message processing");
        await _processor.StartProcessingAsync(cancellationToken);
    }

    public async Task StopProcessingAsync()
    {
        _logger.LogInformation("Stopping Service Bus message processing");
        await _processor.StopProcessingAsync();
        await _processor.DisposeAsync();
        await _client.DisposeAsync();
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        try
        {
            var body = args.Message.Body.ToString();
            _logger.LogInformation("Received message: {MessageId} - Body: {Body}",
                args.Message.MessageId, body);

            // Deserialize the message
            var studentMessage = JsonSerializer.Deserialize<StudentMessage>(body);

            if (studentMessage != null)
            {
                _logger.LogInformation(
                    "Processing student message - ID: {StudentId}, Name: {FirstName} {LastName}, Event: {EventType}",
                    studentMessage.Id,
                    studentMessage.FirstName,
                    studentMessage.LastName,
                    studentMessage.EventType);

                // Process the message (add your business logic here)
                await ProcessStudentMessage(studentMessage);

                // Complete the message
                await args.CompleteMessageAsync(args.Message);
                _logger.LogInformation("Message completed successfully: {MessageId}", args.Message.MessageId);
            }
            else
            {
                _logger.LogWarning("Failed to deserialize message: {MessageId}", args.Message.MessageId);
                await args.DeadLetterMessageAsync(args.Message, "DeserializationFailed", "Could not deserialize message body");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message: {MessageId}", args.Message.MessageId);

            // Dead letter the message if processing fails
            try
            {
                await args.DeadLetterMessageAsync(args.Message, "ProcessingFailed", ex.Message);
            }
            catch (Exception dlqEx)
            {
                _logger.LogError(dlqEx, "Failed to dead letter message: {MessageId}", args.Message.MessageId);
            }
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception,
            "Error in message processing - Source: {ErrorSource}, Entity Path: {EntityPath}",
            args.ErrorSource,
            args.EntityPath);

        return Task.CompletedTask;
    }

    private async Task ProcessStudentMessage(StudentMessage message)
    {
        // Add your business logic here
        // For example:
        // - Store in a different database
        // - Send notifications
        // - Trigger other workflows
        // - Update analytics/reporting systems

        _logger.LogInformation("Business logic processing for student {StudentId}", message.Id);

        // Simulate some processing
        await Task.Delay(100);

        _logger.LogInformation("Business logic completed for student {StudentId}", message.Id);
    }
}
