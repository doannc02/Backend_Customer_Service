using Dapr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IChiba.Customer.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]

public class NotiCustomerController : ControllerBase
{
    [Topic("ichiba-customer-notification-pubsub", "Notification")]
    [HttpPost("notification")]

    public IActionResult HandleOrderEvent([FromBody] object eventMessage)
    {
        Console.WriteLine($"Received event: {eventMessage}");
        return Ok();
    }
}
