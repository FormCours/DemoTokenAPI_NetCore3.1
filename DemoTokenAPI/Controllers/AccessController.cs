using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoTokenAPI.Models;
using DemoTokenAPI.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoTokenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccessController : ControllerBase
    {
        protected TokenManager TokenManager { get; }

        public AccessController(TokenManager tokenManager)
        {
            this.TokenManager = tokenManager;
        }


        [HttpPost]
        public IActionResult GetToken([FromBody] UserCredential credential)
        {
            if(credential == null)
            {
                return BadRequest();
            }

            bool isAdmin = credential.Username == "Balthazar";

            TokenResult token = TokenManager.GenerateJwtToken(new TokenData()
            {
                Username = credential.Username,
                Role = isAdmin ? TokenData.RoleEnum.Admin : TokenData.RoleEnum.User
            });

            return Ok(token);
        }
    }
}