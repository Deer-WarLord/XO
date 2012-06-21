using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    public class MoveAction:IAction
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

            for (int i = 0; i < UserListStorage.activeUsers.Count; i++)
            {
                var current_user_pair = UserListStorage.activeUsers[i];


                if (current_user_pair.User1.Connection.GUID == m_sender.GUID)
                {
                    StateHandler(true, i);
                    break;
                }
                else if (current_user_pair.User2.Connection.GUID == m_sender.GUID)
                {
                    StateHandler(false, i);
                    break;
                }

            }

        }

        private void StateHandler(bool is_user1, int pair_index)
        {
            var current_user_pair = UserListStorage.activeUsers[pair_index];

            switch (current_user_pair.CurrentState)
            {
                case GameState.User1Move:
                    {

                        if (is_user1)
                        {
                            current_user_pair.CurrentState = GameState.User2Move;
                            current_user_pair.User2.Connection.Send(m_input_data);
                        }
                        else
                        { /*exception*/}


                        break;
                    }
                case GameState.User2Move:
                    {

                        if (!is_user1)
                        {
                            current_user_pair.CurrentState = GameState.User1Move;
                            current_user_pair.User1.Connection.Send(m_input_data); 
                        }
                        else
                        { /*exception*/}

                        break;
                    }
                default:
                    {
                        //exception
                        break;
                    }
            }
        }
    }
}
