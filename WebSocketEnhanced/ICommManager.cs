using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketEnhanced
{
    public interface ICommManager
    {
        public event EventHandler<MessageEventArgs> MessageReceived;
        public Task OpenSocketAsync();
        public Task SendMessageAsync(string msg);
        public Task ListenAsync();
    }
}
