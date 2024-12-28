using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMessage()
    {
        System.Console.WriteLine("SE PIDE AAAA!");
        return Ok(new { message = "¡Hola desde el backend con .NET! jijo" });
    }
}
