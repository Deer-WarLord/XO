using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace MultiServer
{
    public interface IConnection
    {
        Socket ConnectionSocket { get; }
        Guid GUID { get; }
        void Send(string str);
        event DataReceivedEventHandler DataReceived;
        event SocketDisconnectedEventHandler Disconnected;
    }

    public class DataReceivedEventArgs
    {
        public int Size { get; private set; }
        public string Data { get; private set; }
        public DataReceivedEventArgs(int size, string data)
        {
            Size = size;
            Data = data;
        }
    }

    public delegate void DataReceivedEventHandler(IConnection sender, DataReceivedEventArgs e);
    public delegate void SocketDisconnectedEventHandler(IConnection sender, EventArgs e);
}
