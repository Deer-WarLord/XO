using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Servers
{
    public interface IServer
    {
        void OnAccept(IAsyncResult ar);
        void OnDataReceived(IConnection sender, DataReceivedEventArgs e);
        void OnDisconnect(IConnection sender, EventArgs e);

        event SocketAcceptEventHandler Connected;
        event DataReceivedEventHandler DataReceived;
        event SocketDisconnectedEventHandler Disconnected;
    }

    public delegate void SocketAcceptEventHandler(IConnection sender, EventArgs e);
}
