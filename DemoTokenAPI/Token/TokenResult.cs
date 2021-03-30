using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTokenAPI.Token
{
    public class TokenResult
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public TokenResult(string token, DateTime expirationDate)
        {
            this.Token = token;
            this.ExpirationDate = expirationDate;
        }
    }
}
