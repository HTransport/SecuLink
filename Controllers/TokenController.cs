using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
using SecuLink.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Tools;
using Microsoft.AspNetCore.Cors;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("React")]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public TokenController(IUserService userService, ITokenService tokenService, IAuthService authService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpGet("{Content}")]
        public async Task<IActionResult> CheckExpiration(string Content)
        {
            return StatusCode(await _authService.Authenticate(Content, _tokenService));
        } // 0

        [HttpDelete("{Content}")]
        public async Task<IActionResult> Logout(string Content)
        {
            var t = await _tokenService.SelectByContent(Content);

            if (t is null)
                return StatusCode(404);

            await _tokenService.Delete(t.UserId);

            return Ok();
        } // 1
    }
}