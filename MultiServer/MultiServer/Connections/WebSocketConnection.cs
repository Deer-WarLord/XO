using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;

namespace MultiServer
{
    public class WebSocketConnection:IConnection
    {
        #region Private members
        
        private byte[] dataBuffer;
        
        private StringBuilder dataString;
        
        private int m_mask_lenght = 4;

        private Socket m_connection_socket;

        private Guid m_guid;
        
        #endregion


        #region IConnection implementation
        
        public event DataReceivedEventHandler DataReceived;
        
        public event SocketDisconnectedEventHandler Disconnected;

        public Socket ConnectionSocket { get { return m_connection_socket; } }

        public Guid GUID { get { return m_guid; } }
        
        public void Send(string str)
        {

            if (ConnectionSocket.Connected)
            {
                byte[] bytesRow = Encoding.UTF8.GetBytes(str);
                List<byte> bytesFormatted = new List<byte>();
                bytesFormatted.Add(129);


                if (bytesRow.Length <= 125)
                {
                    bytesFormatted.Add((byte)bytesRow.Length);
                }
                else if (bytesRow.Length >= 126 && bytesRow.Length <= 65535)
                {
                    bytesFormatted.Add(126);
                    bytesFormatted.Add((byte)((bytesRow.Length >> 8) & 255));
                    bytesFormatted.Add((byte)(bytesRow.Length & 255));
                }
                else
                {
                    bytesFormatted.Add(127);
                    bytesFormatted.Add((byte)((bytesRow.Length >> 56) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length >> 48) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length >> 40) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length >> 32) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length >> 24) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length >> 16) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length >> 8) & 255));
                    bytesFormatted.Add((byte)((bytesRow.Length) & 255));
                }


                bytesFormatted.AddRange(bytesRow);


                try
                {
                    ConnectionSocket.Send(bytesFormatted.ToArray());
                }
                catch
                {
                    var handler = Disconnected;

                    if (handler != null)
                    { Disconnected(this, EventArgs.Empty); }

                }

            }
        }
        
        #endregion

        #region public constructors
        
        public WebSocketConnection(Socket socket)
            : this(socket, 255)
        {

        }

        public WebSocketConnection(Socket socket, int bufferSize)
        {
            m_connection_socket = socket;
            dataBuffer = new byte[bufferSize];
            dataString = new StringBuilder();
            shakeHands();
            m_guid = Guid.NewGuid();
            listen();
        }
        
        #endregion


        #region private methods
        private void shakeHands()
        {
            using (var stream = new NetworkStream(ConnectionSocket))
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {   
                string r = null;
                string WebSocketKey = String.Empty;


                while (r != "")
                {
                    r = reader.ReadLine();


                    if (r.StartsWith("Sec-WebSocket-Key"))
                    {
                        WebSocketKey = r.Replace("Sec-WebSocket-Key: ", "");
                    }

                }


                writer.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
                writer.WriteLine("Upgrade: WebSocket");
                writer.WriteLine("Connection: Upgrade");
                writer.WriteLine("Sec-WebSocket-Origin: " + Settings.WebSocketOrigin);
                writer.WriteLine("Sec-WebSocket-Location: " + Settings.WebSocketLocation);
                writer.WriteLine("Sec-WebSocket-Accept: " + generateAcceptHeader(WebSocketKey));
                writer.WriteLine("");
            }
        }

        private string generateAcceptHeader(string webSocketKey)
        {
            string magicKey = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            string result = webSocketKey + magicKey;
            string rethash = "";

            
            try
            {
                SHA1 hash = SHA1.Create();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] combined = encoder.GetBytes(result);
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error in HashCode : " + ex.Message);
            }


            return rethash;
        }

        private void onDataReceived(DataReceivedEventArgs e)
        {
            var handler = DataReceived;


            if (handler != null)
            { DataReceived(this, e); }

        }

        private void listen()
        {
            ConnectionSocket.BeginReceive(dataBuffer, 0, dataBuffer.Length, 0, readData, null);
        }

        private void readData(IAsyncResult ar)
        {
            int sizeOfReceivedData = ConnectionSocket.EndReceive(ar);


            if (sizeOfReceivedData > 0)
            {
                int message_length = dataBuffer[1] & 127;
                int indexFirstMask = 2;


                if (message_length == 126)
                { indexFirstMask = 4; }
                else if (message_length == 127)
                { indexFirstMask = 10; }


                byte[] mask = new byte[m_mask_lenght];
                Array.Copy(dataBuffer, indexFirstMask, mask, 0, m_mask_lenght);
                int start_data_byte = indexFirstMask + m_mask_lenght;
                byte[] real_data = new byte[dataBuffer.Length - start_data_byte];


                for (int i = start_data_byte, j = 0; i < real_data.Length; i++, j++)
                { real_data[j] = (byte)(dataBuffer[i] ^ mask[j % 4]); }


                dataString.Append(Encoding.UTF8.GetString(real_data, 0, message_length));
                int size = Encoding.UTF8.GetBytes(dataString.ToString().ToCharArray()).Length;
                onDataReceived(new DataReceivedEventArgs(size, dataString.ToString()));
                dataString = new StringBuilder();
                listen();
            }
            else
            {
                var handler = Disconnected;

                if (handler != null)
                { Disconnected(this, EventArgs.Empty); }

            }

        }
        #endregion

        //На будущие )

        public void Close()
        {
            ConnectionSocket.Close();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
