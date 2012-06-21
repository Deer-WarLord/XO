using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer
{
    public static class Settings
    {

        public static int WebSocketPort { get { return 8181; } }

        public static int AppSocketPort { get { return 9191; } }

        public static int MaxConnections { get { return 10; } }

        public static string AppSocketAddress { get { return "127.0.0.1"; } }

        public static string WebSocketOrigin { get { return "http://localhost:8080"; } }

        public static string WebSocketLocation { get { return "ws://localhost:8181"; } }

    }
}
