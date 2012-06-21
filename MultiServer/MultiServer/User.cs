using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer
{
    public class User
    {
        private string m_nickname;
        private IConnection m_user_connection;

        private GameState m_current_state;

        public GameState CurrentState
        {
            get { return m_current_state; }
            set { m_current_state = value; }
        }

        public string Nickname 
        {
            get { return m_nickname; }
            set { m_nickname = value; } 
        }

        public IConnection Connection 
        {
            get { return m_user_connection; } 
        }

        public User(IConnection current_connection)
        {
            m_nickname = "";
            m_current_state = GameState.Empty;
            m_user_connection = current_connection;
        }
    }
}
