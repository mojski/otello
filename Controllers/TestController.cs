namespace Otello.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")] [ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;

    public TestController(ILogger<TestController> logger)
    {
        this.logger = logger;
    }

    public IActionResult Test()
    {
        var guid = Guid.NewGuid();
        this.logger.LogError("Test error {guid}", guid);

        return this.Ok($"Test error {guid}");
    }
}
