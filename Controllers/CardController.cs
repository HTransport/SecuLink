using Microsoft.AspNetCore.Mvc;
using SecuLink.Models;
using SecuLink.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[EnableCors(origins: "https://localhost:3000", headers: "*", methods: "*")]
    public class CardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;

        public CardController(IUserService userService, ICardService cardService)
        {
            _userService = userService;
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] Card card)
        {
            var c = await _cardService.SelectBySerialNumber(card.SerialNumber);
            if (c.SerialNumber == card.SerialNumber)
            {
                var y = await _cardService.Create(0, "Card already exists", 0, 0, 0);
                return Ok(y);
            }
            var a = await _cardService.Create(card.SerialNumber, card.Pin, card.UserId);
            return Ok(a);
        } // 1
        [HttpDelete("{SerialNumber}")]
        public async Task<IActionResult> DeleteCard(string SerialNumber)
        {
            var c = await _cardService.SelectBySerialNumber(SerialNumber);
            if (c is null)
            {
                return Ok(false);
            }
            var a = await _cardService.Delete(SerialNumber);
            return Ok(a);
        } // 1
        [HttpGet("{Username}")]
        public async Task<IActionResult> GetCardByUsername(string Username)
        {
            var user = await _userService.SelectByUsername(Username);
            if (user is null)
            {
                var y = await _cardService.Create(0, "User with bound card doesn't exist", 0, 0, 0);
                return Ok(y);
            }
            var card = await _cardService.SelectByUserId(user.Id);
            if (card is null)
            {
                var y = await _cardService.Create(0, "Card doesn't exist", 0, 0, 0);
                return Ok(y);
            }
            return Ok(card);
        } // 1

    }
}
