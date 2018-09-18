using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWeb.Controllers.api
{
    [Route("api/protected")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        //[HttpGet, Route("get")]
        //[HttpGet, Route("get"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet, Route("get"), Authorize(Roles = "Manager", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<JsonResult> Get(string inputMessage)
        {
            var msg = "Hello " + User.Identity.Name + " with message: " + inputMessage;

            return new JsonResult(new ChatMessage() { Message = msg });
        }


        public class ChatMessage
        {
            public string Message { get; set; }
        }


    }
}