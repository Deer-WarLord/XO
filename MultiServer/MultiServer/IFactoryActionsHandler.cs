using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiServer.Actions;

namespace MultiServer
{
    public interface IFactoryActionsHandler
    {
        void AddAction(string action_name, IAction action_handler);
        void RemoveAction(string action_name);
        IAction GetAction(string action_string);
    }
}
