using System;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using Newtonsoft.Json;

namespace WebSocketEnhanced
{
    public class WebSocketEnhancedCore : ICommManager
    {

        // create object
        // subscribe to messagerecieved
        // await object.Listen()

        string port { get; set; }
        private ClientWebSocket socket;
        private Uri destinationURI;

        public event EventHandler<MessageEventArgs> MessageReceived;

        public WebSocketEnhancedCore(string _port, string destination = "localhost")
        {
            this.port = _port;
            this.destinationURI = new Uri("ws://" + destination + ":" + this.port);
            socket = new ClientWebSocket();
            
        }

        public async Task OpenSocketAsync()
        {
            await socket.ConnectAsync(this.destinationURI, CancellationToken.None);
        }

        /// <summary>
        /// Sends a supplied <c>string</c> over websocket
        /// </summary>
        /// <param name="msg">String to send over websocket.</param>
        /// <returns>Task</returns>
        public async Task SendMessageAsync(string msg)
        {
            await socket.SendAsync(Encoding.ASCII.GetBytes(msg), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task ListenAsync()
        {
            using (var ms = new MemoryStream())
            {
                while (socket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;

                    do
                    {
                        var socketBuffer = WebSocket.CreateClientBuffer(1024, 16);
                        result = await socket.ReceiveAsync(socketBuffer, CancellationToken.None);
                        ms.Write(socketBuffer.Array, socketBuffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var msg = Encoding.UTF8.GetString(ms.ToArray());

                        OnRaiseMessageReceived(new MessageEventArgs(msg));

                    }

                    ms.SetLength(0);
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Position = 0;

                }
            }
        }

        protected virtual void OnRaiseMessageReceived(MessageEventArgs e)
        {

            EventHandler<MessageEventArgs> msgEvent = MessageReceived;

            if (msgEvent != null)
            {
                msgEvent?.Invoke(this, e);
            }
        }

    }
}
