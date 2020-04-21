using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRApiRest.Hub;

namespace SignalRApiRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public WeatherForecastController(ILogger<WeatherForecastController> logger , IHubContext<NotificationHub> notificationHub)
        {
            _logger = logger;
            _notificationHub = notificationHub;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            _notificationHub.Clients.All.SendAsync("ReceiveMessage", "Lucas", "Hello");
            _notificationHub.Clients.Client("connectionId").SendAsync("RecieveMessage", "Lucas", "message");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
