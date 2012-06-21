using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    public class WinAction:IAction
    {
        private IConnection m_sender;
        private string m_input_data;


        IConnection IAction.Sender
        {
            set { m_sender = value; }
        }

        string IAction.InputData
        {
            set { m_input_data = value; }
        }

        void IAction.Execute()
        {
            var current_user_pair = UserListStorage.activeUsers.Find(delegate(ActiveUserPair current_pair)
            {
                return (current_pair.User1.Connection.GUID == m_sender.GUID || current_pair.User2.Connection.GUID == m_sender.GUID);
            });


            current_user_pair.CurrentState = GameState.Empty;
            Dictionary<string, string> action_obj = new Dictionary<string, string>();
            action_obj.Add("action", "invite");
            action_obj.Add("nickname", current_user_pair.User1.Nickname);
            current_user_pair.User2.Connection.Send(JSONConverter.ToJson(action_obj));
            action_obj["nickname"] = current_user_pair.User2.Nickname;
            current_user_pair.User1.Connection.Send(JSONConverter.ToJson(action_obj));
        }
    }
}
