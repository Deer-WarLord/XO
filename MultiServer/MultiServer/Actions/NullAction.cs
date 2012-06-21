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

        public void Execute()
        {
            //stud
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
