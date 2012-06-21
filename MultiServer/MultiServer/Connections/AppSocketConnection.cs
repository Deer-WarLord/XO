using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace MultiServer
{
    public class AppSocketConnection:IConnection
    {
        #region private members

        private byte[] dataBuffer;

        private StringBuilder dataString;

        private Socket m_connection_socket;

        private Guid m_guid;

        #endregion


        #region IConnection implementation

        public Socket ConnectionSocket { get { return m_connection_socket; } }

        public Guid GUID { get { return m_guid; } }

        public void Send(string str)
        {
            using (var stream = new NetworkStream(ConnectionSocket))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(str);
            }
        }

        public event DataReceivedEventHandler DataReceived;

        public event SocketDisconnectedEventHandler Disconnected;
        
        #endregion

        #region public constructors

        public AppSocketConnection(Socket socket)
            : this(socket, 255)
        {

        }

        public AppSocketConnection(Socket socket, int bufferSize)
        {
            m_connection_socket = socket;
            dataBuffer = new byte[bufferSize];
            dataString = new StringBuilder();
            m_guid = Guid.NewGuid();
            listen();
        }

        #endregion

        #region private methods
        private void onDataReceived(DataReceivedEventArgs e)
        {
            var handler = DataReceived;


            if (handler != null)
            { DataReceived(this, e); }

        }

        private void listen()
        {
            try
            {
                ConnectionSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, 0, readData, null);
            }
            catch 
            {
                var handler = Disconnected;


                if (handler != null)
                { Disconnected(this, EventArgs.Empty); };
            }
        }

        private void readData(IAsyncResult ar)
        {
            try
            {
                int sizeOfReceivedData = ConnectionSocket.EndReceive(ar);


                if (sizeOfReceivedData > 0)
                {
                    dataString.Append(Encoding.UTF8.GetString(dataBuffer, 0, sizeOfReceivedData));
                    int size = Encoding.UTF8.GetBytes(dataString.ToString().ToCharArray()).Length;
                    onDataReceived(new DataReceivedEventArgs(size, dataString.ToString()));
                    dataString = new StringBuilder();
                    listen();
                }

            }
            catch(SocketException)
            {
                var handler = Disconnected;


                if (handler != null)
                { Disconnected(this, EventArgs.Empty); }
            }

        }

        #endregion
    }
}
