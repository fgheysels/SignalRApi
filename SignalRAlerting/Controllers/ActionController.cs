using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRAlerting.Hubs;
using SignalRAlerting.Models;

namespace SignalRAlerting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly IHubContext<ActionHub, IActionClient> _signalRHub;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionController"/> class.
        /// </summary>
        public ActionController(IHubContext<ActionHub, IActionClient> hub)
        {
            _signalRHub = hub;
        }

        [HttpPost("launch")]
        public ActionResult<IEnumerable<string>> Launch(string action)
        {
            return Accepted(new
            {
                //ConnectionId = _signalRHub.
            });
        }
        
        [HttpPost("respond")]
        public async Task<ActionResult> Respond([FromBody] ActionResponse response)
        {
            if (response == null)
            {
                return BadRequest();
            }

            if (String.IsNullOrWhiteSpace(response.ClientId))
            {
                await _signalRHub.Clients.All.Alert(response.Message);
            }
            else
            {
                await _signalRHub.Clients.Clients(response.ClientId).Alert(response.Message);
            }

            return Ok();
        }
    }
}
