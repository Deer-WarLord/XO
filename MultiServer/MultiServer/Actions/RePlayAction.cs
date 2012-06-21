using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    public class RePlayAction : IAction
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
            string answer = JSONConverter.FromJson(m_input_data)["type"];


            for (int i = 0; i < UserListStorage.activeUsers.Count; i++)
            {
                var current_user_pair = UserListStorage.activeUsers[i];


                if (current_user_pair.User1.Connection.GUID == m_sender.GUID)
                {
                    StateHandler(true, answer, i);
                    break;
                }
                else if (current_user_pair.User2.Connection.GUID == m_sender.GUID)
                {
                    StateHandler(false, answer, i);
                    break;
                }

            }

        }

        private void sendStartGame(int pair_index)
        {
            ActiveUserPair aup = UserListStorage.activeUsers[pair_index];

            string user1_type = string.Empty;
            string user2_type = string.Empty;
            Random rnd = new Random();


            if (rnd.Next(0, 2) == 0)
            {
                user1_type = "x";
                user2_type = "o";
                aup.CurrentState = GameState.User1Move;
            }
            else
            {
                user1_type = "o";
                user2_type = "x";
                aup.CurrentState = GameState.User2Move;
            }

            Dictionary<string, string> action_obj = new Dictionary<string, string>();
            action_obj.Add("action", "play");
            action_obj.Add("type", user1_type);
            aup.User1.Connection.Send(JSONConverter.ToJson(action_obj));
            action_obj["type"] = user2_type;
            aup.User2.Connection.Send(JSONConverter.ToJson(action_obj));
        }

        private void StateHandler(bool is_user1, string answer, int pair_index)
        {
            var current_user_pair = UserListStorage.activeUsers[pair_index];
            Dictionary<string, string> action_obj = new Dictionary<string, string>();


            if (answer == "cancel")
            {
                action_obj.Add("action", "cancel");


                if (is_user1)
                { current_user_pair.User2.Connection.Send(JSONConverter.ToJson(action_obj)); }
                else
                { current_user_pair.User1.Connection.Send(JSONConverter.ToJson(action_obj)); }

                UserListStorage.inactiveUsers.Find(delegate(User current_user)
                {
                    return (current_user.Connection.GUID == current_user_pair.User1.Connection.GUID);
                }).CurrentState = GameState.Empty;

                UserListStorage.inactiveUsers.Find(delegate(User current_user)
                {
                    return (current_user.Connection.GUID == current_user_pair.User2.Connection.GUID);
                }).CurrentState = GameState.Empty;

                UserListStorage.activeUsers.Remove(current_user_pair);
            }
            else if (answer == "ok")
            {

                switch (current_user_pair.CurrentState)
                {
                    case GameState.User1Accept:
                        {

                            if (!is_user1)
                            { sendStartGame(pair_index); }
                            else
                            { /*exception*/}


                            break;
                        }
                    case GameState.User2Accept:
                        {

                            if (is_user1)
                            { sendStartGame(pair_index); }
                            else
                            { /*exception*/}

                            break;
                        }
                    case GameState.Empty:
                        {
                            current_user_pair.CurrentState = (is_user1) ? GameState.User1Accept : GameState.User2Accept;
                            break;
                        }
                    default:
                        {
                            //exception
                            break;
                        }
                }

            }
            else
            {
                //exception
            }
        }
    }
}
