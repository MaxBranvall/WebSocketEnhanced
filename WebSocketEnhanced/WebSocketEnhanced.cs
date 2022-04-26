using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Text.Encodings;
using System.IO;
using System.Threading;

namespace WebSocketEnhanced
{
    public class WebSocketEnhancedCore
    {

        // create object
        // subscribe to messagerecieved
        // await object.Listen()

        string port { get; set; }
        private ClientWebSocket socket;
        private Uri destinationURI;

        public event EventHandler<MessageEventArgs> MessageReceived;

        public WebSocketEnhancedCore(string _port)
        {
            this.port = _port;
            this.destinationURI = new Uri("ws://localhost:" + this.port);
            socket = new ClientWebSocket();
            
        }

        public async Task OpenSocket()
        {
            await socket.ConnectAsync(this.destinationURI, CancellationToken.None);
        }

        public async Task SendMessage(string msg)
        {
            await socket.SendAsync(Encoding.ASCII.GetBytes(msg), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Listen()
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

    //class TestClass
    //{
    //    public async Task testing()
    //    {
    //        var ws = new WebSocketEnhanced(22751);
    //        ws.MessageReceived += HandleMessage;

    //        await ws.Listen();


    //    }

    //    void HandleMessage(object sender, MessageEventArgs e)
    //    {
    //        Console.WriteLine("From c#: " + e.message);
    //    }

    //}
}
