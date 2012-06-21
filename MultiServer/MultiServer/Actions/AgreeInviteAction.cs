using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    class AgreeInviteAction:IAction
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
            
            int proposer_index = UserListStorage.inactiveUsers.FindIndex(delegate(User current_user)
            {
                return current_user.Connection.GUID == m_sender.GUID;
            });


            int opponent_index = UserListStorage.inactiveUsers.FindIndex(delegate(User current_user)
            {
                return current_user.Nickname == opponent;
            });


            if (UserListStorage.inactiveUsers[opponent_index].CurrentState == GameState.Empty)
            {
                handleAction(opponent_index, proposer_index);
            }
            else
            {
                //exception
            }

        }

        private void handleAction(int opponent_index, int proposer_index)
        {
            User proposer = UserListStorage.inactiveUsers[opponent_index];
            User opponent = UserListStorage.inactiveUsers[proposer_index];

            string opponent_type = string.Empty;
            string proposer_type = string.Empty;
            Random rnd = new Random();
            ActiveUserPair aup = new ActiveUserPair(proposer, opponent, GameState.Empty);

            
            if (rnd.Next(0, 2) == 0)
            {
                opponent_type = "x";
                proposer_type = "o";
                aup.CurrentState = GameState.User2Move;
            }
            else
            {
                opponent_type = "o";
                proposer_type = "x";
                aup.CurrentState = GameState.User1Move;
            }


            UserListStorage.activeUsers.Add(aup);
            Dictionary<string, string> action_obj = new Dictionary<string, string>();
            action_obj.Add("action", "play");
            action_obj.Add("type", opponent_type);
            string action_json = JSONConverter.ToJson(action_obj);
            opponent.CurrentState = GameState.Bisy;
            opponent.Connection.Send(action_json);
            proposer.CurrentState = GameState.Bisy;
            action_obj["type"] = proposer_type;
            proposer.Connection.Send(action_json);
        }
    }
}
