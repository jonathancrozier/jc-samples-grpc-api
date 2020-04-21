using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JC.Samples.Grpc.Api
{
    /// <summary>
    /// Exposes greeter endpoints (sample).
    /// </summary>
    public class GreeterService : Greeter.GreeterBase
    {
        #region Reaodnlys

        private readonly ILogger<GreeterService> _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">The logger instance</param>
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Returns a reply to the caller (sample).
        /// </summary>
        /// <param name="request">The Hello Request message</param>
        /// <param name="context">The context for the RPC call</param>
        /// <returns>A Hello Reply containing a greeting message</returns>
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        #endregion
    }
}