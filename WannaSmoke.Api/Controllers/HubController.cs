using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WannaSmoke.Api.Hubs;
using WannaSmoke.Infrastructure.Models;

namespace WannaSmoke.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HubController : Controller
    {
        private readonly IHubContext<ClientHub> _ClientHub;
        public HubController(IHubContext<ClientHub> hub)
        {
            _ClientHub = hub;
        }

        [HttpPost("/Smoke")]
        public async Task<IActionResult> GoSmoke([FromBody] GoSmokeRequest request)
        {
            //await _ClientHub.Clients.AllExcept(request.ConnectionID).SendAsync("Receive");
            await _ClientHub.Clients.All.SendAsync("Receive",request.Name);
            return Ok("Sent");
        }

    }
}