using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
using SecuLink.RequestModels;
using SecuLink.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.Tools;
using System.Web.Http.Cors;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public AuthController(ITokenService tokenService, IAuthService authService)
        {
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost]
        [Route("op")]
        public async Task<IActionResult> AuthorizeUser([FromBody] NTE nte)
        {
            var t = await _tokenService.SelectByUserId(nte.EId);

            if (t is null)
                return Ok("e pa nemre");

            if (t.Content != nte.Token)
                return Ok("e pa nemre");

            DateTime dt = DateTime.Now;
            if (t.DOC.AddSeconds(Convert.ToDouble(t.TTL_seconds)) < dt)
            {
                await _tokenService.Delete(nte.EId);
                return Ok("e pa nemre");
            }

            var c = await _authService.Authorize(nte.Username);

            return Ok(c);
        } // 1

        [HttpPost]
        [Route("deop")]
        public async Task<IActionResult> UnuthorizeUser([FromBody] NTE nte)
        {
            var t = await _tokenService.SelectByUserId(nte.EId);

            if (t is null)
                return Ok("e pa nemre");

            if (t.Content != nte.Token)
                return Ok("e pa nemre");

            DateTime dt = DateTime.Now;
            if (t.DOC.AddSeconds(Convert.ToDouble(t.TTL_seconds)) < dt)
            {
                await _tokenService.Delete(nte.EId);
                return Ok("e pa nemre");
            }

            var c = await _authService.Unauthorize(nte.Username);

            return Ok(c);
        } // 1

        [HttpPost]
        [Route("check")]
        public async Task<IActionResult> CheckUserAuth([FromBody] NTE nte)
        {
            var t = await _tokenService.SelectByUserId(nte.EId);

            if (t is null)
                return Ok("e pa nemre");

            if (t.Content != nte.Token)
                return Ok("e pa nemre");

            DateTime dt = DateTime.Now;
            if (t.DOC.AddSeconds(Convert.ToDouble(t.TTL_seconds)) < dt)
            {
                await _tokenService.Delete(nte.EId);
                return Ok("e pa nemre");
            }

            var c = await _authService.CheckIfAuthorized(nte.Username);

            return Ok(c);
        } // 1
    }
}