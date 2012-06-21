using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    public class ConnectAction:IAction
    {
        private IConnection m_sender;
        private string m_input_data;


        public IConnection Sender
        {
            set { m_sender = value; }
        }

        public string InputData
        {
            set { m_input_data = value; }
        }

        public ConnectAction() { ;}

        public void Execute()
        {
            List<string> user_list = new List<string>();

            
            for (int i = 0; i < UserListStorage.inactiveUsers.Count; i++)
            {

                if (UserListStorage.inactiveUsers[i].Connection.GUID == m_sender.GUID)
                {
                    UserListStorage.inactiveUsers[i].Nickname = JSONConverter.FromJson(m_input_data)["nickname"];
                    break;
                }
                else if (UserListStorage.inactiveUsers[i].Nickname != "")
                {
                    user_list.Add(UserListStorage.inactiveUsers[i].Nickname);
                }

            }


            m_sender.Send(JSONConverter.ToJson(user_list.ToArray()));
        }

    }
}
