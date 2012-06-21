using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiServer.Servers;
using MultiServer.Actions;

namespace MultiServer
{
    public class Game
    {
        private IFactoryActionsHandler m_actions_handler;

        public Game(IFactoryActionsHandler actions_handler)
        {
            m_actions_handler = actions_handler;
        }

        public void Launch()
        {
            IServer app_server = new AppSocketServer();
            app_server.Connected += onConnect;
            app_server.DataReceived += onDataReceived;
            app_server.Disconnected += onDisconnect;
            Console.WriteLine("Start listen app server");
            IServer web_server = new WebSocketServer();
            web_server.Connected += onConnect;
            web_server.DataReceived += onDataReceived;
            web_server.Disconnected += onDisconnect;
            Console.WriteLine("Start listen web server");
        }

        private void onDataReceived(IConnection sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(sender.GUID + " send: " + e.Data);
            IAction current_action = m_actions_handler.GetAction(e.Data);
            current_action.Sender = sender;
            current_action.InputData = e.Data;
            current_action.Execute();
        }

        private void onDisconnect(IConnection sender, EventArgs e)
        {
            Console.WriteLine(sender.GUID + " disconnected");
            int index = UserListStorage.inactiveUsers.FindIndex((current_user) => { return current_user.Connection.GUID == sender.GUID; });

            if (index == -1)
            {
                
                int pair_index = UserListStorage.activeUsers.FindIndex((current_pair) =>
                {
                    return (current_pair.User1.Connection.GUID == sender.GUID || current_pair.User2.Connection.GUID == sender.GUID);
                });

                IConnection notify_sender = (UserListStorage.activeUsers[pair_index].User1.Connection.GUID == sender.GUID) ?
                    UserListStorage.activeUsers[pair_index].User2.Connection : UserListStorage.activeUsers[pair_index].User1.Connection;

                notify_sender.Send("{\"error\":\"your opponent disconnected\"}");
            }
        }

        private void onConnect(IConnection sender, EventArgs e)
        {
            UserListStorage.inactiveUsers.Add(new User(sender));
            Console.WriteLine(sender.GUID + " connected");
        }
    }
}
