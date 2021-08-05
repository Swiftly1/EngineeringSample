import * as vscode from 'vscode';
import { connect, Socket } from "net";

import 
{
  LanguageClient,
  StreamInfo,
  CancellationStrategy,
} from "vscode-languageclient/node";

let client: LanguageClient;
let socket: Socket;
export async function activate(context: vscode.ExtensionContext)
{
	const disposable = vscode.commands.registerCommand('extension.xd', async () =>
  {
     await connectFunc();

      var obj = 
      {
          jsonrpc: "2.0",
          method: "Test",
          params: []
      };

      var json = JSON.stringify(obj);
      sendMessage(socket, json);
	});

	context.subscriptions.push(disposable);
  
  /*let options = getOptions();
  client = new LanguageClient("xd", "xd", connectFunc, options);
  client.registerProposedFeatures();
  client.start();*/
}

async function sendMessage(socket: Socket, message: string)
{
  var closing = "\r\n";
  socket.write(`Content-Length: ${message.length}` + closing);
  socket.write(closing);
  socket.write(message);
}

const connectFunc = () => 
{
  return new Promise<StreamInfo>((resolve, reject) => 
  {
      if (socket !== undefined)
      {
        vscode.window.showInformationMessage('using existing pipe!');
        resolve({ writer: socket, reader: socket });
        return;
      }
      vscode.window.showInformationMessage('connecting to pipe!');
      socket = connect(`\\\\.\\pipe\\pipe_xd`);
      socket.on("connect", () => 
      {
        vscode.window.showInformationMessage('connected!');
        resolve({ writer: socket, reader: socket });
      });
      socket.on("error", (e) => 
      {
        vscode.window.showInformationMessage('err!' + e.message);
      });
  });
};

const getOptions = () =>
{
  let options = 
  {
    documentSelector: 
    [
      {
        language: "xd",
      },
      {
        pattern: "**/*.xd",
      },
    ],
    progressOnInitialization: true,
    connectionOptions:
     {
      maxRestartCount: 10,
      cancellationStrategy: CancellationStrategy.Message,
    },
    synchronize:
     {
      // Synchronize the setting section 'languageServerExample' to the server
      // configurationSection: "languageServerExample",
      // fileEvents: workspace.createFileSystemWatcher("**/*.cs"),
    },
  };

  return options;
};