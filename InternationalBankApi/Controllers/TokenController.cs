using BusinessLayer.Abstrack;
using BusinessLayer.Dto;
using CoreLayer.Utilities.Filters;
using CoreLayer.Utilities.Result.Abstrack;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class TokenController : BaseController
    {
        private readonly ILogonWorkFlow _logonWorkFlow;
        #region constructor
        public TokenController(ILogonWorkFlow logonWorkFlow)
        {
            this._logonWorkFlow = logonWorkFlow;
        }
        #endregion

        [AllowAnonymous, HttpPost, Route("GetToken"), ValidationFilter]
        public async Task<IActionResult> GetToken([FromBody] TokenRequestDto model)
        {
            var result = await this._logonWorkFlow.Logon(model);
            return Ok(result);
        }

        [HttpPost, Route("RefreshToken"),AllowAnonymous]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto model)
        {
            var result = this._logonWorkFlow.RefreshToken(model);
            return Ok(result);
        }
    }
}
