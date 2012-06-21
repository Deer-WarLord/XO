using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiServer
{
    public static class UserListStorage
    {
        public static List<User> inactiveUsers = new List<User>();
        public static List<ActiveUserPair> activeUsers = new List<ActiveUserPair>();
    }
}
