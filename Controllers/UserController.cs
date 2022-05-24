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
    //[EnableCors(origins: "https://localhost:3000", headers: "*", methods: "*")]
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
            var u = await _userService.SelectByUsername(user.Username);
            if (u.Username == user.Username)
            {
                var y = await _userService.Create(0, "User already exists", "User already exists", 0);
                return Ok(y);
            }
            var a = await _userService.Create(user.Username, Encryptor.GetHashSha256(user.Password_Enc));
            return Ok(a);
        } // 1
        [HttpDelete("{Username}")]
        public async Task<IActionResult> DeleteUser(string Username)
        {
            var u = await _userService.SelectByUsername(Username);
            if (u is null)
            {
                return Ok("User doesn't exist or already deleted");
            }
            await _userService.Delete(u.Id);
            var card = await _cardService.SelectByUserId(u.Id);
            if (card is null)
                return Ok("Deleted User");
            await _cardService.Delete(card.SerialNumber);
            return Ok("Deleted User and Card");
        } // 1
        [HttpGet("{Username}")]
        public async Task<IActionResult> GetUserByUsername(string Username)
        {
            var user = await _userService.SelectByUsername(Username);
            if (user is null)
            {
                var y = await _userService.Create(0, "User doesn't exist", "User doesn't exist", 0);
                return Ok(y);
            }
            return Ok(user);
        } // 1
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var u = await _userService.SelectByUsername(user.Username);
            if (u is null)
                return Ok(false);
            bool result = Encryptor.GetHashSha256(user.Password_Enc) == u.Password_Enc;
            return Ok(result);
        } // 1
    }
}
