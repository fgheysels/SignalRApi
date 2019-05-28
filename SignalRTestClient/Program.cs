using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRTestClient
{
    class Program
    {
        private const string ActionHubUrl = "http://localhost:3132/hubs/actions"; // This is the URL that is registered on the API.

        private static HubConnection connection;

        static async Task Main(string[] args)
        {
            Console.WriteLine("SignalR Test-Client");
            Console.WriteLine("Connecting to SignalR Hub ...");

            connection = new HubConnectionBuilder().WithUrl(ActionHubUrl).Build();

            connection.Closed += async (exception) =>
            {
                Console.WriteLine("Connection was closed, trying to reconnect.");
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                await connection.StartAsync();
                Console.WriteLine("Reconnected");
            };

            connection.On<string>("Alert", (m) => Console.WriteLine(m));

            await connection.StartAsync();

            Console.WriteLine("Connected!");

            Console.WriteLine("Press q to quit, m to invoke an action");

            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey();

            } while (keyInfo.Key != ConsoleKey.Q);
        }

    }
}
