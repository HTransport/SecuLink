using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
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
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public TokenController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("{UserId}")]
        public async Task<IActionResult> CreateToken(int UserId) // kreiranje novog tokena će "pregaziti" već postojeći token
        {
            var t = await _tokenService.SelectByUserId(UserId);

            if (t is not null)
                await _tokenService.Delete(t.UserId);

            var u = await _userService.SelectById(UserId);

            string token = TokenGenerator.GenerateBasic(u.Username);
            await _tokenService.Create(token, UserId); // DEBUG - koristi najveci overload metode

            return Ok(token);
        } // 1

        [HttpGet("exp/{UserId}")]
        public async Task<IActionResult> CheckExpiration(int UserId)
        {
            var t = await _tokenService.SelectByUserId(UserId);
            DateTime dt = DateTime.Now;

            if(t is null)
                return Ok(false);

            if (t.DOC.AddSeconds(Convert.ToDouble(t.TTL_seconds)) >= dt)
                return Ok(true);

            await _tokenService.Delete(UserId);

            return Ok(false);
        } // 1

        [HttpDelete("{UserId}")]
        public async Task<IActionResult> DeleteToken(int UserId)
        {
            var t = await _tokenService.SelectByUserId(UserId);

            if (t is null)
                return Ok(false);

            await _tokenService.Delete(t.UserId);

            return Ok(true);
        } // 1
    }
}