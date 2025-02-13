using System;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace IoT_Backend_Assignment
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IIpDetails> GetIpDetails(string ip)
        {
            var url = $"http://api.ipstack.com/{ip}?access_key=80193305d5b2b857d2d80ce89b8dceb4";
            HttpResponseMessage response;

            try
            {
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();

                var responseData = JsonSerializer.Deserialize<IpDetails>(jsonResponse);

                if (responseData.City == null)
                {
                    throw new IPServiceNotAvailableException("IPServiceNotAvailableException: IP address doesn't exist!", 500, "");
                }

                return responseData;
            }
            catch (Exception ex)
            {
                throw new IPServiceNotAvailableException("IPServiceNotAvailableException: ", 500, ex.Message);
            }
        }
    }
}
