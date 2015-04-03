using System;
using System.ComponentModel.Composition;
using HyperQube.Library;
using WebSocketSharp;

namespace HyperQube.Services
{
    [Export(typeof(IConnectionService))]
    class SocketService : IConnectionService
    {
        public IDisposable OpenConnection(string apiKey)
        {
            var socket = new WebSocket("wss://websocket.pushbullet.com/subscribe/" + apiKey);
            socket.ConnectAsync();

            return socket;
        }
    }
}
