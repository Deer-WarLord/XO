using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiServer.Actions;

namespace MultiServer
{
    public class XO_ActionsHandler:IFactoryActionsHandler
    {
        private Dictionary<string, IAction> m_actions_list;
        
        public XO_ActionsHandler() 
        {
            m_actions_list = new Dictionary<string, IAction>();
        }

        public void LoadDefault()
        {
            m_actions_list = new Dictionary<string, IAction>() { { "connect", new ConnectAction() },
                                                                 { "ok", new AgreeInviteAction() },
                                                                 { "cancel", new CancelInviteAction() }, 
                                                                 { "invite", new InviteAction() }, 
                                                                 { "move", new MoveAction() }, 
                                                                 { "replay", new RePlayAction() }, 
                                                                 { "win", new WinAction() },};
        }

        public void AddAction(string action_name, IAction action_handler)
        {
            m_actions_list.Add(action_name, action_handler);
        }

        public void RemoveAction(string action_name)
        {
            m_actions_list.Remove(action_name);
        }

        public IAction GetAction(string action_string)
        {
            IAction find_action;
            string action_name = JSONConverter.FromJson(action_string)["action"];
            return (m_actions_list.TryGetValue(action_name, out find_action)) ? find_action : new NullAction();
        }
    }
}
