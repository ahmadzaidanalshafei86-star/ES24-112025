using ES.Web.Models;
using System.Text.Json;
using System.Text;

namespace ES.Web.Services
{
    public class SilosApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://46.185.128.246:8484/SILOSAPI";

        public SilosApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PostJobRequestAsync(SilosJobRequestDto request)
        {
            try
            {
                var url = $"{_apiBaseUrl}/POST_JOB_REQUEST_INFO";
                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null // Keep exact casing
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                Console.WriteLine($"Error posting to SILOS API: {ex.Message}");
                return false;
            }
        }
    }
}
