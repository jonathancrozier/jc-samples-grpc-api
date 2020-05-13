using Flurl;
using Flurl.Http;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JC.Samples.Grpc.Api.Client
{
    /// <summary>
    /// Main Program class.
    /// </summary>
    class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the program</param>
        /// <returns><see cref="Task"/></returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine("Getting todos...");
            Console.WriteLine();

            // Connect to the gRPC service and retrieve a list of todos.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var client   = new Todos.TodosClient(channel);
            var response = await client.GetTodosAsync(new TodoRequest { UserId = 2 });

            // Display the number of todos found.
            var    todos            = response.Todos;
            string todoCountMessage = $"Found {todos.Count()} todos";

            Console.WriteLine(todoCountMessage);
            Console.WriteLine("".PadLeft(todoCountMessage.Length, '='));
            Console.WriteLine();

            // Output the todo titles.
            foreach (var todo in todos)
            {
                Console.WriteLine($"Title: {todo.Title}");
            }

            Console.WriteLine();
            Console.WriteLine("Deleting todos...");
            
            // Build a Configuration object for retrieving JSON and User Secret settings.
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets("95fad9a5-87ef-467e-9e12-2407793c0af7")
                .Build();
            
            // Get an Auth Token from Auth0.
            var tokenResponse = await config["Auth0:Domain"]
                .AppendPathSegment("oauth/token")
                .PostUrlEncodedAsync(new
                {
                    audience      = config["Auth0:Audience"],
                    client_id     = config["Auth0:ClientId"],
                    client_secret = config["Auth0:ClientSecret"],
                    grant_type    = "client_credentials"
                })
                .ReceiveJson();
            
            // Delete all todos.
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {tokenResponse.access_token}");

            await client.DeleteTodosAsync(new Empty(), headers);

            Console.WriteLine("Deleted all todos");

            // Inform the user that the program has completed.
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }

        #endregion
    }
}