using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
using SecuLink.RequestModels;
using SecuLink.ResponseModels;
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
    [EnableCors("local")]
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
        public async Task<IActionResult> CreateUser([FromBody] ConfirmForm user)
        {
            var u = await _userService.SelectNewUserByUsername(user.Username);

            if (u is null)
                return StatusCode(404);

            if (u.Pin != user.Pin)
                return StatusCode(403);

            await _userService.DeleteNew(user.Username);
            
            await _userService.Create(user.Username, u.FirstName, u.LastName, u.Role, u.Email, Encryptor.GetHashSha256(user.Password, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ"));

            await _cardService.Create(u.SerialNumber, (await _userService.SelectByUsername(user.Username)).Id);

            var User = await _userService.SelectByUsername(user.Username);

            string token = TokenGenerator.GenerateBasic(User.Username);
            await _tokenService.Create(token, User.Id);

            return Ok(new TR(token, await _authService.CheckIfAuthorized(u.Username), u.Username));
        } // 0

        [HttpPost]
        [Route("create/new")]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateForm user)
        {
            var result = await _authService.Authenticate(user.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var u = await _userService.SelectByUsername(user.Username);

            if (u is not null)
                return StatusCode(409);

            var n = await _userService.SelectNewUserByUsername(user.Username);

            if (n is not null)
                return StatusCode(409);

            var pin = await _userService.CreateNew(user.Username, user.FirstName, user.LastName, user.Role, user.Email, user.SerialNumber);

            return Ok(pin);
        } // 0

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditUser([FromBody] EditForm user)
        {
            var result = await _authService.Authenticate(user.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var u = await _userService.SelectByUsername(user.CurrentUsername);

            if (u is null)
                return StatusCode(404);

            await _userService.Edit(user.CurrentUsername, user.Username, user.FirstName, user.LastName, u.Role, user.Email);

            var c = await _cardService.SelectBySerialNumber(user.SerialNumber);

            if (c is null)
                return StatusCode(404);

            await _cardService.Edit(user.SerialNumber);

            return Ok();
        } // 0

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteForm user)
        {
            var result = await _authService.Authenticate(user.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var u = await _userService.SelectByUsername(user.Username);

            if (u is null)
                return StatusCode(404);

            await _userService.Delete(user.Username);

            var card = await _cardService.SelectByUserId(u.Id);
            var tk = await _tokenService.SelectByUserId(u.Id);

            if (card is not null)
                await _cardService.Delete(card.SerialNumber);
            if (tk is not null)
                await _tokenService.Delete(tk.UserId);

            return Ok();
        } // 0

        [HttpPost]
        [Route("delete/new")]
        public async Task<IActionResult> DeleteNewUser([FromBody] DeleteForm user)
        {
            var result = await _authService.Authenticate(user.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var u = await _userService.SelectNewUserByUsername(user.Username);

            if (u is null)
                return StatusCode(404);

            await _userService.DeleteNew(u.Username);

            return Ok();
        } // 0

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> GetUsers([FromBody] ListForm lf)
        {
            var result = await _authService.Authenticate(lf.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var list = await _userService.GetList();
            List<UserListItem> outList = new();
            if (lf.IsNew)
                CurrentState.ResetLS();
            else CurrentState.UsersLS += lf.NumOfElements;

            try
            {
                for (int i = CurrentState.UsersLS; i < lf.NumOfElements + CurrentState.UsersLS; i++)
                {
                    if (list.ElementAtOrDefault(i) is null)
                        break;
                    outList.Add(list.ElementAtOrDefault(i));
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
            return Ok(outList);
        } // 0

        [HttpPost]
        [Route("listnew")]
        public async Task<IActionResult> GetNewUsers([FromBody] ListForm lf)
        {
            var result = await _authService.Authenticate(lf.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var list = await _userService.GetListNew();
            List<NewUser> outList = new();
            if (lf.IsNew)
                CurrentState.ResetLS();
            else CurrentState.NewUsersLS += lf.NumOfElements;

            try
            {
                for (int i = CurrentState.NewUsersLS; i < lf.NumOfElements + CurrentState.NewUsersLS; i++)
                {
                    if (list.ElementAtOrDefault(i) is null)
                        break;
                    outList.Add(list.ElementAtOrDefault(i));
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
            return Ok(outList);
        } // 0

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginForm user)
        {
            var u = await _userService.SelectByUsername(user.Username);

            if (u is null)
                return StatusCode(404);

            bool result = Encryptor.GetHashSha256(user.Password, "P539Kw95nauPbZEymAwl2dT8AKcRFWjQ") == u.Password_Enc;

            if (!result)
                return StatusCode(403);

            var t = await _tokenService.SelectByUserId(u.Id);

            if (t is not null)
                await _tokenService.Delete(t.UserId);

            string token = TokenGenerator.GenerateBasic(u.Username);
            if(!await _authService.CheckIfAuthorized(u.Username))
                await _tokenService.Create(token, u.Id);
            else await _tokenService.Create(token, 600, DateTime.Now, u.Id);
            return Ok(new TR(token, await _authService.CheckIfAuthorized(u.Username), u.Username));
        } // 0
    }
}
