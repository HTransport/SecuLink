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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;


        public UserController(IUserService userService, ICardService cardService, ITokenService tokenService, IAuthService authService)
        {
            _userService = userService;
            _cardService = cardService;
            _tokenService = tokenService;
            _authService = authService;
        }
        
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody] NUTE nute)
        {
            var u = await _userService.SelectNewUserByUsername(nute.Username);

            if (u is null)
                return Ok(false);

            if(u.Pin != nute.Pin)
                return Ok(false);

            await _userService.DeleteNew(nute.Username);
            
            var a = await _userService.Create(nute.Username, Encryptor.GetHashSha256(nute.Password_Enc, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ"));
            return Ok(a);
        } // 1

        [HttpPost]
        [Route("create/root")]
        public async Task<IActionResult> CreateUserDirect([FromBody] UEX user)
        {
            if (user.Key != "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ")
                return Ok("e pa nemre");

            var u = await _userService.SelectByUsername(user.Username);

            if (u is not null)
                return Ok(false);

            var a = await _userService.Create(user.Username, Encryptor.GetHashSha256(user.Password_Enc, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ"));
            return Ok(a);
        } // 1

        [HttpPost]
        [Route("create/new")]
        public async Task<IActionResult> CreateNewUser([FromBody] NTE nte)
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

            var u = await _userService.SelectByUsername(nte.Username);

            if (u is not null)
                return Ok(false);

            var n = await _userService.SelectNewUserByUsername(nte.Username);

            if (n is not null)
                return Ok(false);

            var a = await _userService.CreateNew(nte.Username);
            return Ok(a);
        } // 1

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] UTE ute)
        {
            var t = await _tokenService.SelectByUserId(ute.EId);

            if(t is null)
                return Ok("e pa nemre");

            if (t.Content != ute.Token)
                return Ok("e pa nemre");

            DateTime dt = DateTime.Now;
            if (t.DOC.AddSeconds(Convert.ToDouble(t.TTL_seconds)) < dt)
            {
                await _tokenService.Delete(ute.EId);
                return Ok("e pa nemre");
            }

            var u = await _userService.SelectByUsername(ute.Username);

            if (u is null)
                return Ok(false);

            await _userService.Delete(u.Id);

            var card = await _cardService.SelectByUserId(u.Id);
            var tk = await _tokenService.SelectByUserId(u.Id);
            var au = await _authService.CheckIfAuthorized(u.Username);

            if (card is not null)
                await _cardService.Delete(card.SerialNumber);
            if (tk is not null)
                await _tokenService.Delete(tk.UserId);
            if (au)
                await _authService.Unauthorize(u.Username);

            return Ok(true);
        } // 1

        [HttpPost]
        [Route("delete/new")]
        public async Task<IActionResult> DeleteNewUser([FromBody] NTE nte)
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

            var u = await _userService.SelectNewUserByUsername(nte.Username);

            if (u is null)
                return Ok(false);

            await _userService.DeleteNew(u.Username);

            return Ok(true);
        } // 1

        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> GetUserByUsername([FromBody] NTE nte)
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

            var u = await _userService.SelectByUsername(nte.Username);

            if (u is null)
                return Ok(false);

            return Ok(u);
        } // 1

        [HttpPost]
        [Route("get/new")]
        public async Task<IActionResult> GetNewUserByUsername([FromBody] NTE nte)
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

            var u = await _userService.SelectNewUserByUsername(nte.Username);

            if (u is null)
                return Ok(false);

            return Ok(u);
        } // 1

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var u = await _userService.SelectByUsername(user.Username);

            if (u is null)
                return Ok(false);

            bool result = Encryptor.GetHashSha256(user.Password_Enc, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ") == u.Password_Enc;

            if (!result)
                return Ok(result);

            var t = await _tokenService.SelectByUserId(u.Id);

            if (t is not null)
                await _tokenService.Delete(t.UserId);

            string token = TokenGenerator.GenerateBasic(u.Username);
            await _tokenService.Create(token, u.Id);

            return Ok(token);
        } // 1
    }
}
