using BusinessLayer.Abstrack;
using BusinessLayer.Dto.PhpneNumber;
using CoreLayer.DataAccess.Concrete.DataRequest;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class PhoneNumberController : BaseController
    {
        #region Variables
        private readonly IPhoneNumberWorkFlow _phoneNumberWorkFlow;
        #endregion
        #region Constructor
        public PhoneNumberController(IPhoneNumberWorkFlow phoneNumberWorkFlow)
        {
            this._phoneNumberWorkFlow = phoneNumberWorkFlow;
        }
        #endregion

        [HttpPost, Route("CustomerPhoneList"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerPhoneList([FromBody] DataTableRequest request)
        {
            var result = await this._phoneNumberWorkFlow.CustomerPhoneList(request);
            return Ok(result);
        }

        [HttpPost, Route("UpdatePhoneNumber"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdatePhoneNumber([FromBody] PhoneNumberUpdateDto request)
        {
            var result = await this._phoneNumberWorkFlow.UpdatePhoneNumber(request);
            return Ok(result);
        }

        [HttpPost, Route("AddPhoneNumber"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddPhoneNumber([FromBody] PhoneNumberCreateDto phone)
        {
            var result = await this._phoneNumberWorkFlow.AddPhoneNumber(phone);
            return Ok(result);
        }

        [HttpPost, Route("DeletePhoneNumber"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeletePhoneNumber([FromBody] PhoneNumberUpdateDto phone)
        {
            var result = await this._phoneNumberWorkFlow.DeletePhoneNumber(phone);
            return Ok(result);
        }
    }
}
