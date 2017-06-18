using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketsServer.Interfaces;

namespace OfficeMiniServer.Handlers
{
    public class WebSocketsHandler : MessageObservable
    {

        public static List<IWebSocketConnection> Sockets   { get; set; }
        public static WebSocketServer            Server    { get; set; }

        public List<MessageObserver>             Observers { get; set; }

        private static WebSocketsHandler         _webSocketsHandler;

        //Used for events coming from the clients
        public event EventHandler                GetFromClients = delegate { };

        private WebSocketsHandler()
        {
            Sockets = new List<IWebSocketConnection>();
            //Get Socket URL from App settings.
            string socketURL = ConfigurationManager.AppSettings["socket_url"];

            Server = new WebSocketServer(socketURL);

            FleckLog.Level = LogLevel.Debug;

            Server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    //1 socket is served each time, for load balancing reasons.
                    if (Sockets.Count > 1) {
                        foreach (IWebSocketConnection spesific_socket in Sockets) {
                            spesific_socket.Close();
                        }
                    }
                    Sockets.Add(socket);

                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    Sockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    this.GetFromClients(message, new EventArgs());
                };
            });

            this.GetFromClients += GetMessageFromClients;

            this.Observers = new List<MessageObserver>();
        }

        public static WebSocketsHandler Instance()
        {
            if(_webSocketsHandler == null)
            {
                throw new InvalidOperationException("WebSocketsHandler not initialized.");
            }
            return _webSocketsHandler;
        }

        public static WebSocketsHandler Initialize()
        {
            return _webSocketsHandler = new WebSocketsHandler();
        }


        public void SendToClients(string message)
        {
            Sockets.ToList().ForEach(s => s.Send(message));
        }

        private void GetMessageFromClients(object sender, EventArgs e)
        {

            Observers.ForEach(o => o.Notify(sender.ToString()));
        }

        public void Register(MessageObserver observer)
        {
            Observers.Add(observer);
        }

        public void Unregister(MessageObserver observer)
        {
            Observers.Remove(observer);
        }
    }
}

