using DC.Api.Extensions;
using DC.Utils.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IpController : ControllerBase
    {
        private readonly ILogger<IpController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IpController(IHttpContextAccessor httpContextAccessor, ILogger < IpController > logger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        [HttpGet("getIp")]
        public List<string> GetIp()
        {
           
            return new List<string>() 
            {
                this.HttpContext.GetTrueIP(),
                this.HttpContext.Connection.RemoteIpAddress.ToString(),
                _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()
               };
        }


        [HttpGet("get")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
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
