using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRAlerting.Hubs
{
    public interface IActionHub
    {
        Task Alert(string message);
    }

    public class ActionHub : Hub<IActionHub>
    {
        public async Task Alert(string connectionId, string message)
        {
            await Clients.Client(connectionId).Alert(message);
        }

        public async Task BroadcastAlert(string message)
        {
            await Clients.All.Alert(message);
        }
    }
}
