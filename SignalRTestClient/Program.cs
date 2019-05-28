using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignalRTestClient
{
    class Program
    {
        private const string ActionHubUrl = "http://localhost:3132/hubs/actions"; // This is the URL that is registered on the API.

        private static HubConnection _connection;

        static async Task Main(string[] args)
        {
            Console.WriteLine("SignalR Test-Client");
            Console.WriteLine("Connecting to SignalR Hub ...");

            _connection = new HubConnectionBuilder().WithUrl(ActionHubUrl).Build();

            _connection.Closed += async (exception) =>
            {
                Console.WriteLine("Connection was closed, trying to reconnect.");
                await Task.Delay(TimeSpan.FromMilliseconds(50));
                await _connection.StartAsync();
                Console.WriteLine("Reconnected");
            };

            _connection.On<string>("Alert", (m) => Console.WriteLine(m));

            await _connection.StartAsync();

            Console.WriteLine("Connected!");
            Console.WriteLine("Press q to quit, m to invoke an action");

            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey();

                if (keyInfo.Key == ConsoleKey.M)
                {
                    await LaunchActionAsync();
                }


            } while (keyInfo.Key != ConsoleKey.Q);
        }

        private static readonly HttpClient Http = new HttpClient();

        private static async Task LaunchActionAsync()
        {
            Console.WriteLine();
            Console.WriteLine("Launching action by posting to API");

            // Since there seems to be no way to get the Id of the SignalR connection in the API controller,
            // we'll retrieve the connectionId on the client-side and send it with our request.
            // When we have authenticated users/clients, we can use the OnConnected method of the Hub and register
            // a mapping connectionId <> userId.  On the server-side, we could then use the userId to retrieve the 
            // connectionId from the Hub.
            var connectionId = await _connection.InvokeAsync<string>("GetConnectionId");

            var request = new HttpRequestMessage(HttpMethod.Post, $"http://localhost:3132/api/action/launch?connectionId={connectionId}&action=test");

            var response = await Http.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                var jsonContent = (JObject) JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

                Console.WriteLine($"Action accepted; client connection ID = {jsonContent.Value<string>("connectionId")}");
                Console.WriteLine("By invoking the Respond API operation with this connection ID, a message will be pushed back to this client.");
            }
            else
            {
                Console.WriteLine("Response from server: " + response.StatusCode);
            }
        }

    }
}
