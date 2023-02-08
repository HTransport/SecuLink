using Microsoft.AspNetCore.Mvc;
using SecuLink.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecuLink.RequestModels;
using SecuLink.ResponseModels;
using Microsoft.AspNetCore.Cors;
using SecuLink.Tools;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("local")]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public CardController(ICardService cardService, ITokenService tokenService, IAuthService authService)
        {
            _cardService = cardService;
            _tokenService = tokenService;
            _authService = authService;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> GetCards([FromBody] ListForm lf)
        {
            var result = await _authService.Authenticate(lf.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var list = await _cardService.GetList();
            List<CU> outList = new();
            if (lf.IsNew)
                CurrentState.ResetLS();
            else CurrentState.CardsLS += lf.NumOfElements;

            try
            {
                for (int i = CurrentState.CardsLS; i < lf.NumOfElements + CurrentState.CardsLS; i++)
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
        } // 1
    }
}
