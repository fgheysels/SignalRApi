using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRAlerting.Hubs
{
    public interface IActionClient
    {
        Task Alert(string message);
    }

    public class ActionHub : Hub<IActionClient>
    {
        public async Task Alert(string connectionId, string message)
        {
            await Clients.Client(connectionId).Alert(message);
        }

        public async Task BroadcastAlert(string message)
        {
            await Clients.All.Alert(message);
        }

        public async Task<string> GetConnectionId()
        {
            return await Task.FromResult(Context.ConnectionId);
        }
    }
}
