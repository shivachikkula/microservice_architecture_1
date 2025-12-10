namespace MessageConsumerService.Services;

public interface IServiceBusConsumerService
{
    Task StartProcessingAsync(CancellationToken cancellationToken);
    Task StopProcessingAsync();
}
