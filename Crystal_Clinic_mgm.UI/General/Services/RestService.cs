using Newtonsoft.Json;
using Crystal_Clinic_Mgm.UI.General.Weather;

namespace Crystal_Clinic_Mgm.UI.General.Services
{
    public class RestService
    {
        HttpClient _client;


        public RestService()
        {
            _client = new HttpClient();

        }

        public async Task<WeatherModelData?> GetWeatherData(string query)
        {
            WeatherModelData? weatherData = null;
            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherModelData?>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return weatherData;
        }
        public async Task<WeatherModelData?> GetWeatherData2(string query)
        {
            WeatherModelData? weatherData = null;
            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    weatherData = JsonConvert.DeserializeObject<WeatherModelData?>(content);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return weatherData;
        }
    }
}
