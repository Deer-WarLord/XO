using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;

namespace WinClient
{
    public class NetServer
    {
        /// буфер для входящих данных
        byte[] bytes;
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;
        Socket sender;

        public NetServer()
        {
            bytes = new byte[1024];
            //ipHost = Dns.GetHostEntry("127.0.0.1");
            //ipAddr = ipHost.AddressList[1];
            ipEndPoint = new IPEndPoint(IPAddress.Loopback, 9191);
            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// соединяем сокет с удаленной конечной точкой
        /// </summary>
        public void Connect()
        {
            sender.Connect(ipEndPoint);
        }

        /// <summary>
        /// Отправляем данные через сокет
        /// </summary>
        /// <param name="message">смс</param>
        public void Send(string message)
        {
            byte[] msg = Encoding.ASCII.GetBytes(message);
            /// Отправляем данные через сокет
            int bytesSent = sender.Send(msg);
        }

        /// <summary>
        /// Получаем ответ от удаленного сервера
        /// </summary>
        /// <returns>ответ</returns>
        public string Receive()
        {
            string message = string.Empty;
            int bytesRec = sender.Receive(bytes);
            message = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            return message;
        }

        /// <summary>
        /// освобождаем сокет
        /// </summary>
        public void Close()
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
