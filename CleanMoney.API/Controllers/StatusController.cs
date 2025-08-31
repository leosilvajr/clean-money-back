using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace CleanMoney.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            var assembly = Assembly.GetExecutingAssembly().GetName();

            var status = new
            {
                Application = assembly.Name,
                Version = assembly.Version?.ToString(),
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion.ToString(),
                CurrentDirectory = Environment.CurrentDirectory,
                LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                UtcTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                ServerIP = Dns.GetHostAddresses(Dns.GetHostName())
                              .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString(),
                Variables = new
                {
                    DotNetVersion = Environment.Version.ToString(),
                    ProcessorCount = Environment.ProcessorCount,
                    ContentRoot = AppContext.BaseDirectory
                },
                Status = "Healthy"
            };

            return Ok(status);
        }
    }
}
