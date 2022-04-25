using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketEnhanced
{
    public class MessageEventArgs : EventArgs
    {

        public string message { get; set; }

        public MessageEventArgs(string message)
        {
            this.message = message;
        }

    }
}
