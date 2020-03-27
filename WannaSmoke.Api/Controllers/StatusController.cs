using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WannaSmoke.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : Controller
    {
        public StatusController()
        {

        }

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}