using GatewayService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace GatewayService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GatewayController : ControllerBase
    {
        private readonly ILogger<GatewayController> _logger;
        private readonly IGatewayService _gatewayService;

        public GatewayController(ILogger<GatewayController> logger, IGatewayService gatewayService)
        {
            _logger = logger;
            _gatewayService = gatewayService;
        }

        [HttpGet("{service}/{*path}")]
        public async Task<IActionResult> ForwardGet(string service, string path)
        {
            var response = await _gatewayService.ForwardRequestAsync(service, path, HttpMethod.Get);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        [HttpPost("{service}/{*path}")]
        public async Task<IActionResult> ForwardPost(string service, string path)
        {
            string content = "";
            using (var reader = new StreamReader(Request.Body))
            {
                content = await reader.ReadToEndAsync();
            }
            
            var response = await _gatewayService.ForwardRequestAsync(service, path, HttpMethod.Post, content);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
} 