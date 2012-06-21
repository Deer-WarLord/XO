using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MultiServer;
using System.Net.Sockets;
using MultiServer.Servers;

namespace ServerDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            XO_ActionsHandler actions_handler = new XO_ActionsHandler();
            actions_handler.LoadDefault();

            Game gm = new Game(actions_handler);
            gm.Launch();
            Console.ReadLine();
        }
    }
}
