using Microsoft.AspNetCore.Mvc;

namespace MessageConsumerService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Health check endpoint called");
        return Ok(new
        {
            Status = "Healthy",
            Service = "Message Consumer Service",
            Timestamp = DateTime.UtcNow
        });
    }
}
