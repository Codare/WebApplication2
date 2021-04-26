using DotNetCore.CAP;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication2.Controllers
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
        private readonly ICapPublisher _capPublisher;

        public WeatherForecastController(
            ICapPublisher capPublisher,
            ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _capPublisher = capPublisher;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken cancellationToken)
        {
            var a3IncomingLead = new A3IncomingLead
            {
                FirstName = "Fred",
                LastName = "Bloggs",
                EmailAddress = "a@b.com",
                ZipCode = 12345,
                PhoneNumber = "202-555-4567",
            };

            await _capPublisher.PublishAsync("a3-local-consumer", a3IncomingLead, cancellationToken: cancellationToken);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [NonAction]
        [CapSubscribe("a3-local-consumer")]
        public async Task ReceiveMessage(A3IncomingLead incomingLeadEvent, CancellationToken cancellationToken = default)
        {
        }
    }
}
