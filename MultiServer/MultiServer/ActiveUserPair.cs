using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer
{
    public enum GameState
    {
        UserAwait, User1Move, User2Move, User1Accept, User2Accept, Empty, Bisy
    };

    public struct ActiveUserPair
    {
        private User m_user1;
        private User m_user2;
        private GameState m_current_state;

        public User User1 { get { return m_user1; } }
        public User User2 { get { return m_user2; } }

        public GameState CurrentState 
        {
            get { return m_current_state; }
            set { m_current_state = value; } 
        }

        public ActiveUserPair(User user1, User user2, GameState state)
        {
            m_user1 = user1;
            m_user2 = user2;
            m_current_state = state;
        }
    }
}
