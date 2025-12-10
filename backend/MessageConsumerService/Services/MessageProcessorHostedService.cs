namespace MessageConsumerService.Services;

public class MessageProcessorHostedService : BackgroundService
{
    private readonly IServiceBusConsumerService _consumerService;
    private readonly ILogger<MessageProcessorHostedService> _logger;

    public MessageProcessorHostedService(
        IServiceBusConsumerService consumerService,
        ILogger<MessageProcessorHostedService> logger)
    {
        _consumerService = consumerService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Message Processor Hosted Service is starting");

        try
        {
            await _consumerService.StartProcessingAsync(stoppingToken);

            // Keep the service running
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Message Processor Hosted Service is stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Message Processor Hosted Service");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Message Processor Hosted Service is stopping");
        await _consumerService.StopProcessingAsync();
        await base.StopAsync(cancellationToken);
    }
}
