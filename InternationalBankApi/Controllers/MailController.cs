using BusinessLayer.Abstrack;
using BusinessLayer.Dto.Mail;
using CoreLayer.DataAccess.Concrete.DataRequest;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class MailController : BaseController
    {
        #region Variables
        private readonly IMailWorkFlow _mailWorkFlow;
        #endregion
        #region Constructor
        public MailController(IMailWorkFlow mailWorkFlow)
        {
            this._mailWorkFlow = mailWorkFlow;
        }
        #endregion

        [HttpPost, Route("CustomerMailList"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerMailList([FromBody] DataTableRequest request)
        {
            var result = await this._mailWorkFlow.CustomerMailList(request);
            return Ok(result);
        }

        [HttpPost, Route("UpdateMail")]
        public async Task<IActionResult> UpdateMail([FromBody] MailUpdateDto mail)
        {
            var result = await this._mailWorkFlow.UpdateMail(mail);
            return Ok(result);
        }

        [HttpPost, Route("DeleteMail")]
        public async Task<IActionResult> DeleteMail([FromBody] MailUpdateDto mail)
        {
            var result = await this._mailWorkFlow.DeleteMail(mail);
            return Ok(result);
        }

        [HttpPost, Route("AddMail")]
        public async Task<IActionResult> AddMail([FromBody] MailCreateDto mail)
        {
            var result = await this._mailWorkFlow.AddMail(mail);
            return Ok(result);
        }
    }
}
