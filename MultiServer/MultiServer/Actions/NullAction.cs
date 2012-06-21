using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    public class NullAction:IAction
    {
        private IConnection m_sender;
        private string m_input_data;
        private string m_message;

        public NullAction()
        {
            m_message = "{\"error\":\"error\"}";
        }

        public NullAction(string message)
        {
            m_message = message;
        }

        public void Execute()
        {
            m_sender.Send(m_message);
        }

        public IConnection Sender
        {
            set { m_sender = value; }
        }


        public string InputData
        {
            set { m_input_data = value; }
        }
    }
}
