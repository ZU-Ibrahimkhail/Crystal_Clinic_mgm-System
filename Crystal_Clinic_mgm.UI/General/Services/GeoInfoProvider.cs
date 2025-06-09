using Crystal_Clinic_Mgm.Common.AppConfig;
namespace Crystal_Clinic_Mgm.UI.General.Services
{
    public class GeoInfoProvider
    {
        private readonly HttpClient _httpClient;
        //create constructor and call HttpClient
        public GeoInfoProvider()
        {
            _httpClient = new HttpClient();
            //{
            //    Timeout = TimeSpan.FromSeconds(5)
            //};
        }
        private async Task<string> GetIPAddress()
        {
            var ipAddress = await _httpClient.GetAsync($"https://ipinfo.io/ip");
            if (ipAddress.IsSuccessStatusCode)
            {
                var json = await ipAddress.Content.ReadAsStringAsync();
                return json.ToString();
            }
            return "";// "180.94.88.14";
        }
        public async Task<string> GetGeoInfo()
        {
            //I have already created this function under GeoInfoProvider class.
            var ipAddress = await GetIPAddress();
            // When geting ipaddress, call this function and pass ipaddress as given below
            string tokenKey = AppConfig.WeatherToken;
            var response = await _httpClient.GetAsync($"http://api.ipstack.com/" + ipAddress + "?access_key=" + tokenKey + "");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return json;
            }
            return string.Empty;
        }
    }
}
