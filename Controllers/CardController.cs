using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
using SecuLink.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using SecuLink.RequestModels;
namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class CardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly ITokenService _tokenService;

        public CardController(IUserService userService, ICardService cardService, ITokenService tokenService)
        {
            _userService = userService;
            _cardService = cardService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCard([FromBody] CTE cte)
        {
            var t = await _tokenService.SelectByUserId(cte.EId);

            if (t is null)
                return Ok("e pa nemre");

            if (t.Content != cte.Token)
                return Ok("e pa nemre");

            var c = await _cardService.SelectBySerialNumber(cte.SerialNumber);

            if (c is not null)
                return Ok(false);
            var a = await _cardService.Create(cte.SerialNumber, cte.UserId);
            return Ok(a);

        } // 1
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteCard([FromBody] NTE nte)
        {
            var t = await _tokenService.SelectByUserId(nte.EId);

            if (t is null)
                return Ok("e pa nemre");

            if (t.Content != nte.Token)
                return Ok("e pa nemre");

            var u = await _userService.SelectByUsername(nte.Username);

            if (u is null)
                return Ok(false);

            var c = await _cardService.SelectByUserId(u.Id);

            if (c is null)
                return Ok(false);

            await _cardService.Delete(c.SerialNumber);
            return Ok(true);
        } // 1
        [HttpPost]
        [Route("get")]
        public async Task<IActionResult> GetCardByUsername([FromBody] NTE nte)
        {
            var t = await _tokenService.SelectByUserId(nte.EId);

            if (t is null)
                return Ok("e pa nemre");
            if (t.Content != nte.Token)
                return Ok("e pa nemre");

            var user = await _userService.SelectByUsername(nte.Username);
            if (user is null)
                return Ok("User with bound card doesn't exist");
            var card = await _cardService.SelectByUserId(user.Id);
            if (card is null)
                return Ok("Card doesn't exist");
            return Ok(card);
        } // 1
    }
}
