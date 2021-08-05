using Serilog;
using StreamJsonRpc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace LanguageServerProtocol
{
    public class RPCHandlers
    {
        [JsonRpcMethod("Test")]
        public object asd()
        {
            Log.Warning("works");
            return "";
        }

        [JsonRpcMethod(RPCMethods.InitializeName)]
        public object Initialize(JToken arg)
        {
            Log.Information("Initialization");

            var serializer = new JsonSerializer()
            {
                ContractResolver = new JsonStuff.ResourceOperationKindContractResolver()
            };

            var param = arg.ToObject<InitializeParams>();
            var clientCapabilities = param?.Capabilities;

            var capabilities = new ServerCapabilities
            {
                TextDocumentSync = new TextDocumentSyncOptions(),
                CompletionProvider = new CompletionOptions(),
                SignatureHelpProvider = new SignatureHelpOptions(),
                ExecuteCommandProvider = new ExecuteCommandOptions(),
                DocumentRangeFormattingProvider = false,
            };

            capabilities.TextDocumentSync.Change = TextDocumentSyncKind.Incremental;
            capabilities.TextDocumentSync.OpenClose = true;
            capabilities.TextDocumentSync.Save = new SaveOptions { IncludeText = true };
            capabilities.CodeActionProvider = clientCapabilities?.Workspace?.ApplyEdit ?? true;
            capabilities.DefinitionProvider = true;
            capabilities.ReferencesProvider = true;
            capabilities.DocumentSymbolProvider = true;
            capabilities.WorkspaceSymbolProvider = false;
            capabilities.RenameProvider = true;
            capabilities.HoverProvider = true;
            capabilities.DocumentHighlightProvider = true;

            return new InitializeResult { Capabilities = capabilities };
        }
    }
}
