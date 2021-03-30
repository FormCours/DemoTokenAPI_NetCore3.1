using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoTokenAPI.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoTokenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HelloController : ControllerBase
    {
        protected TokenManager TokenManager { get; }

        public HelloController(TokenManager tokenManager)
        {
            this.TokenManager = tokenManager;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("nologin")]
        public IActionResult SayHello()
        {
            return Ok(new
            {
                message = $"Bonjour !"
            });
        }

        [HttpGet]
        [Authorize]
        [Route("login")]
        public IActionResult SayHelloToUser()
        {
            TokenData data = TokenManager.GetDataToken(HttpContext.User);

            return Ok(new
            {
                message = $"Bonjour {data.Username} !"
            });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin")]
        public IActionResult SayHelloToAdmin()
        {
            return Ok(new
            {
                message = $"Bonjour monsieur l'admin !"
            });
        }
    }
}