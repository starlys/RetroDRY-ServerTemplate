using System;
using Microsoft.AspNetCore.Mvc;

namespace MyServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        //TODO:Set up your authorization framework. For example, change GetSession to accept a user ID and password, authenticate with
        //your database, then use an actual user instead of GenericUser to start the session
        [HttpGet("newsession")]
        public object GetSession()
        {
            var retroverse = Globals.Retroverse;
            string sessionKey = retroverse.CreateSession(AppUser.GenericUser);
            return new
            {
                sessionKey
            };
        }

        //TODO:If you need other API endpoints besides authentication and RetroDRY endpoints, you can add them to this MainController
        //or create new controller classes.
    }
}