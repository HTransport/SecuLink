using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
using SecuLink.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;

        public UserController(IUserService userService, ICardService cardService)
        {
            _userService = userService;
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var a = await _userService.Create(user.Username, user.Password_Enc);
            return Ok(a);
        } // 1
        [HttpDelete("{Username}")]
        public async Task<IActionResult> DeleteUser(string Username)
        {
            var user = await _userService.SelectByUsername(Username);
            await _userService.Delete(user.Id);
            var card = await _cardService.SelectByUserId(user.Id);
            if (card is null)
                return Ok("Deleted User");
            await _cardService.Delete(card.SerialNumber);
            return Ok("Deleted User and Card");
        } // 1
        [HttpGet("g/{Username}")]
        public async Task<IActionResult> GetUserByUsername(string Username)
        {
            var user = await _userService.SelectByUsername(Username);
            return Ok(user);
        } // 1
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var u = await _userService.SelectByUsername(user.Username);
            if (u is null)
                return Ok(false);
            bool result = user.Password_Enc == u.Password_Enc;
            return Ok(result);
        } // 1
    }
}
