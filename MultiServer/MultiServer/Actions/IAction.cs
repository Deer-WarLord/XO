using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer.Actions
{
    public interface IAction
    {
        IConnection Sender { set; }
        string InputData { set; }
        void Execute();
    }
}
