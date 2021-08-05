using System;
using Serilog;
using StreamJsonRpc;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServerProtocol
{
    public class Server
    {
        public const string PipeName = "pipe_xd";

        private JsonRpc RPC { get; set; }

        private NamedPipeServerStream pipe { get; set; }

        public async Task StartAsync()
        {
            Log.Information($"Setting Up Logger");

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .WriteTo.Console()
                            .CreateLogger();

            while (true)
            {
                var cancelSrc = new CancellationTokenSource();
                var token = cancelSrc.Token;

                try
                {
                    Log.Information($"Opening NamedPipe - {PipeName}");
                    pipe = new NamedPipeServerStream(PipeName);
                    await pipe.WaitForConnectionAsync();

                    Log.Information("RPC Listening");

                    RPC = JsonRpc.Attach(pipe, new RPCHandlers());

                    RPC.Disconnected += (a, b) => RPC_Disconnected(a, b, cancelSrc);

                    await RPC.Completion;
                }
                catch (OperationCanceledException)
                {
                    Log.Information("RPC Pipe closed");
                }
                catch (Exception ex)
                {
                    Log.Information(ex.ToString());
                }
                finally
                {
                    // TODO: rethink whether it makes sense
                    await pipe.DisposeAsync();
                    RPC?.Dispose();
                }
            }
        }

        private void RPC_Disconnected(object sender, JsonRpcDisconnectedEventArgs e, CancellationTokenSource token)
        {
            Log.Information("Disconnected");
            token.Cancel();
        }
    }
}
