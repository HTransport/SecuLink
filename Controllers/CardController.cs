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
            var a = await _cardService.Create(card.SerialNumber, card.Pin, card.UserId);
            return Ok(a);
        } // 1
        [HttpDelete("{SerialNumber}")]
        public async Task<IActionResult> DeleteCard(string SerialNumber)
        {
            var a = await _cardService.Delete(SerialNumber);
            return Ok(a);
        } // 1
        [HttpGet("{Username}")]
        public async Task<IActionResult> GetCardByUsername(string Username)
        {
            var user = await _userService.SelectByUsername(Username);
            var card = await _cardService.SelectByUserId(user.Id);
            return Ok(card);
        } // 1
    }
}
