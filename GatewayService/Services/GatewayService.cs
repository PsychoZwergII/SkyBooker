using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GatewayService.Services
{
    public class GatewayService : IGatewayService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GatewayService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> ForwardRequestAsync(string service, string path, HttpMethod method, string content = null)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration[$"ServiceUrls:{service}"];
            var request = new HttpRequestMessage(method, $"{baseUrl}/{path}");

            if (!string.IsNullOrEmpty(content))
            {
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            return await client.SendAsync(request);
        }
    }
} 