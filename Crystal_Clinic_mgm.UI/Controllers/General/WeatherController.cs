using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.UI.General.Services;
using Crystal_Clinic_Mgm.UI.General.Weather;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Crystal_Clinic_Mgm.UI.Controllers.General
{
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        //http://api.openweathermap.org/data/2.5/weather?q=kabul,af&APPID=ebbe64ef9a33fd5015c56360cbf25e12       
        public UI.General.Services.RestService Rs { get; set; }
        public WeatherController()
        {
            Rs = new RestService();
        }
        [HttpGet("GetWeather")]
        public IActionResult Get()
        {
            string city = "";
            string tokenKey = AppConfig.WeatherToken;
            if (city == "")
            {
                city = "Kabul";
            }
            string prova = $"https://api.openweathermap.org/data/2.5/weather?q=+{city}&units=metric&appid={tokenKey}";
            WeatherModelData results = Rs.GetWeatherData(prova).Result ?? new();
            if (city != "")
            {
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("GetWeather/{city}")]
        public IActionResult Get(string city)
        {
            string tokenKey = AppConfig.WeatherToken;
            string prova = $"https://api.openweathermap.org/data/2.5/weather?q=+{city}&units=metric&appid={tokenKey}";
            WeatherModelData results = Rs.GetWeatherData(prova).Result ?? new();
            if (results != null)
            {
                return Ok(results);
            }
            else if (results == null)
            {
                return Ok(null);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpGet("GetWeather12")]
        public async Task<IActionResult> Getweather()//, string country)
        {
            GeoInfoProvider geoInfoProvider = new();
            var ctryInfo = await geoInfoProvider.GetGeoInfo();
            return Ok(ctryInfo);
        }

    }
}
