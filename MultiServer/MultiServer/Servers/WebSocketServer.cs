using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace MultiServer.Servers
{
    public class WebSocketServer: IServer
    {
        #region private methods

        Socket m_server_socket;

        List<IConnection> m_connection_list;

        #endregion

        public WebSocketServer()
        {
            m_server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Loopback, Settings.WebSocketPort);
            m_server_socket.Bind(ipLocal);
            m_server_socket.Listen(Settings.MaxConnections);
            m_server_socket.BeginAccept(new AsyncCallback(OnAccept), null);
        }

        #region IServer implementation

        public void OnAccept(IAsyncResult ar)
        {

            try
            {
                Socket clientSocket = m_server_socket.EndAccept(ar);
                IConnection new_client = new WebSocketConnection(clientSocket);
                new_client.DataReceived += OnDataReceived;
                new_client.Disconnected += OnDisconnect;
                m_connection_list.Add(new_client);
                var handler = Connected;


                if (handler != null)
                { Connected(new_client, new EventArgs()); }


                m_server_socket.BeginAccept(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message, "SGSserverTCP");
            }
            
        }

        public void OnDataReceived(IConnection sender, DataReceivedEventArgs e)
        {
            var handler = DataReceived;


            if (handler != null)
            { DataReceived(sender, e); }

        }

        public void OnDisconnect(IConnection sender, EventArgs e)
        {
            var handler = Disconnected;


            if (handler != null)
            { Disconnected(sender, e); }

        }

        public event SocketAcceptEventHandler Connected;

        public event DataReceivedEventHandler DataReceived;

        public event SocketDisconnectedEventHandler Disconnected;

        #endregion
    }
}
