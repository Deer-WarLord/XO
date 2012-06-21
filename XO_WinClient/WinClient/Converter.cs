using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinClient
{
    public static class Converter
    {
        public static List<string> GetPlayerList(string msg)
        {
            List<string> rv = new List<string>();
            
            foreach (string item in msg.Split(','))
            {
                rv.Add(item);
            }
            return rv;
        }
    }
}
