namespace StudentService.Services;

public interface IServiceBusService
{
    Task SendMessageAsync(string message);
}
