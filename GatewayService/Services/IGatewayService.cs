using System.Net.Http;
using System.Threading.Tasks;

namespace GatewayService.Services
{
    public interface IGatewayService
    {
        Task<HttpResponseMessage> ForwardRequestAsync(string service, string path, HttpMethod method, string content = null);
    }
} 