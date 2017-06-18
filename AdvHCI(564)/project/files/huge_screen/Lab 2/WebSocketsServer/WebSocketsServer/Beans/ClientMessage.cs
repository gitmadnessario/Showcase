using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeMiniServer.Beans
{
    /// <summary>
    /// Bean data class used for manipulating
    /// audio settings received from the clients.
    /// </summary>
    public class ClientMessage
    {
        public string       MessageType { get; set; }
        public List<string> Contents    { get; set; }

        public ClientMessage() : this("", new List<string>()) { }

        public ClientMessage(string type, List<string> contents)
        {
            MessageType = type;
            Contents    = contents;
        }
    }
}
