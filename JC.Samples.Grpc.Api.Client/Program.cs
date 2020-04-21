using Grpc.Net.Client;
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

            // Inform the user that the program has completed.
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }

        #endregion
    }
}