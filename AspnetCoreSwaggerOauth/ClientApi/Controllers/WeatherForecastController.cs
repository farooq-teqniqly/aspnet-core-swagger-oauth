using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Newtonsoft.Json;

namespace ClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDownstreamWebApi _weatherApi;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IDownstreamWebApi weatherApi,
            ILogger<WeatherForecastController> logger)
        {
            _weatherApi = weatherApi;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var response = await this._weatherApi.CallWebApiForAppAsync(
                "WeatherApi",
                options =>
                {
                    options.HttpMethod = HttpMethod.Get;
                });

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);
        }

        [HttpGet("save")]
        public async Task<IEnumerable<WeatherForecast>> Save()
        {
            var response = await this._weatherApi.CallWebApiForAppAsync(
                "WeatherApi",
                options =>
                {
                    options.HttpMethod = HttpMethod.Get;
                    options.RelativePath = "save";
                });

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);
        }

    }
}
