using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthServices services) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLoginDto request)
        {
            var res = await services.LogIn(request);
            return Ok(res);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto request)
        {
           var res= await services.Register(request);
            return Ok(res);
        }


    }
}
