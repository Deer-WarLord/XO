using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    class CancelInviteAction:IAction
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
            string opponent = JSONConverter.FromJson(m_input_data)["opponent"];
            string inviter = String.Empty;

            int opponent_index = UserListStorage.inactiveUsers.FindIndex(delegate(User current_user)
            {
                return current_user.Nickname == opponent;
            });


            if (UserListStorage.inactiveUsers[opponent_index].CurrentState == GameState.Empty)
            {
                Dictionary<string, string> action_obj = new Dictionary<string, string>();
                action_obj.Add("action", "cancel");
                string action_json = JSONConverter.ToJson(action_obj);
                UserListStorage.inactiveUsers[opponent_index].Connection.Send(action_json);
            }
            else
            {
                //exception
            }

        }
    }
}
