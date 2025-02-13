using System;
using System.Text.Json.Serialization;

namespace IoT_Backend_Assignment
{
    public class IpDetails : IIpDetails
    {
        [JsonPropertyName("city")]
        public String City { get; set; }

        [JsonPropertyName("country_name")]
        public String Country { get; set; }

        [JsonPropertyName("continent_name")]
        public String Continent { get; set; }

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    public class IPInfoProvider : IIPInfoProvider
    {
        private readonly ApiClient _apiClient;

        public IPInfoProvider()
        {
            _apiClient = new ApiClient();
        }

        public IIpDetails GetDetails(string ip)
        {
            var task = _apiClient.GetIpDetails(ip);
            task.Wait();
            return task.Result;
        }
    }
}
