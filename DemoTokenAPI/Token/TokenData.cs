using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTokenAPI.Token
{
    public class TokenData
    {
        [Flags]
        public enum RoleEnum
        {
            User = 1,
            Admin = 2
        }

        public string Username { get; set; }
        public RoleEnum Role { get; set; }
    }
}
