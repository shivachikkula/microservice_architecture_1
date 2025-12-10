namespace MessageConsumerService.Models;

public class StudentMessage
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string EventType { get; set; } = string.Empty;
}
