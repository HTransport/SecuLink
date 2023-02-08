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
using System.Net.NetworkInformation;

namespace SecuLink.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("local")]
    public class ReaderController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;
        private readonly ILogService _logService;


        public ReaderController(IUserService userService, ICardService cardService, ITokenService tokenService, IAuthService authService, ILogService logService)
        {
            _userService = userService;
            _cardService = cardService;
            _tokenService = tokenService;
            _authService = authService;
            _logService = logService;
        }

        [HttpPost]
        [Route("logs")]
        public async Task<IActionResult> GetLogs([FromBody] ListForm lf)
        {
            var result = await _authService.Authenticate(lf.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var list = await _logService.GetList();
            List<LogListItem> outList = new();
            if (lf.IsNew)
                CurrentState.ResetLS();
            else CurrentState.LogLS += lf.NumOfElements;

            try
            {
                for (int i = CurrentState.LogLS; i < lf.NumOfElements + CurrentState.LogLS; i++)
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
        [Route("setup")]
        public async Task<IActionResult> SetupReader([FromBody] DeviceAuthForm daf)
        {
            string MAC = Encryptor.Decrypt_Aes(daf.MAC_enc, daf.Key);

            if (MAC != daf.MAC)
                return StatusCode(403);

            await _authService.AddReader(MAC, daf.Role);

            string MAC_api = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();

            string MAC_enc = Encryptor.Encrypt_Aes(MAC_api, daf.Key);
            return Ok(new DeviceAuthForm() { MAC = MAC_api, MAC_enc = MAC_enc, Key = daf.Key });
        } // 0

        [HttpPost]
        [Route("remove")]
        public async Task<IActionResult> RemoveReaderByReader([FromBody] DeviceAuthForm daf)
        {
            string MAC = Encryptor.Decrypt_Aes(daf.MAC_enc, daf.Key);

            if (MAC != daf.MAC)
                return StatusCode(403);

            if (!await _authService.CheckReader(MAC))
                return StatusCode(404);

            await _authService.RemoveReader(MAC);

            return Ok();
        } // 0

        [HttpPost]
        [Route("remove/app")]
        public async Task<IActionResult> RemoveReaderByApp([FromBody] ReaderForm rf)
        {
            var result = await _authService.Authenticate(rf.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            if (!await _authService.CheckReader(rf.MAC))
                return StatusCode(404);

            await _authService.RemoveReader(rf.MAC);

            return Ok();
        } // 0

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> GetReaders([FromBody] ListForm lf)
        {
            var result = await _authService.Authenticate(lf.Token, _tokenService);
            if (result != 200)
                return StatusCode(result);

            var list = await _authService.GetReaders();
            List<ReaderListItem> outList = new();
            if (lf.IsNew)
                CurrentState.ResetLS();
            else CurrentState.ReaderLS += lf.NumOfElements;

            try
            {
                for (int i = CurrentState.ReaderLS; i < lf.NumOfElements + CurrentState.ReaderLS; i++)
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
        [Route("open")]
        public async Task<IActionResult> Open([FromBody] LogForm lf)
        {
            await _logService.Create(lf.SerialNumber, lf.MAC);

            if (!await _authService.CheckReader(lf.MAC))
                return StatusCode(403);

            var Role = (await _userService.SelectById((await _cardService.SelectBySerialNumber(lf.SerialNumber)).UserId)).Role;
            var readerRole = await _authService.GetReaderRole(lf.MAC);

            if (Role || Role == readerRole)
                return Ok();

            return StatusCode(403);
        } // 0
    }
}
