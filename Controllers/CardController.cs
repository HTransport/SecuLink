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
    //
    // UKLANJANJE??
    //
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("React")]
    public class CardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public CardController(IUserService userService, ICardService cardService, ITokenService tokenService, IAuthService authService)
        {
            _userService = userService;
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
