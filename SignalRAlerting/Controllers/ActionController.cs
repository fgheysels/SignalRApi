using System;
using System.Collections.Generic;
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
        private readonly IHubContext<ActionHub, IActionClient> _hub;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionController"/> class.
        /// </summary>
        public ActionController(IHubContext<ActionHub, IActionClient> hub)
        {
            _hub = hub;
        }

        [HttpPost("launch")]
        public ActionResult<IEnumerable<string>> Launch(string connectionId, string action)
        {
            return Accepted(new
            {
                ConnectionId = connectionId
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
                await _hub.Clients.All.Alert(response.Message);
            }
            else
            {
                await _hub.Clients.Clients(response.ClientId).Alert(response.Message);
            }

            return Ok();
        }
    }
}
