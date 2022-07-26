using Api.Base;
using BusinessLayer.Abstrack;
using BusinessLayer.Model;
using CoreLayer.DataAccess.Concrete;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class AccountController : BaseController
    {
        #region Variables
        IAccountWorkFlow _accountWorkFlow;
        #endregion
        #region constructor
        public AccountController(IAccountWorkFlow accountWorkFlow)
        {
            this._accountWorkFlow = accountWorkFlow;
        }
        #endregion

        [HttpPost,Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyAccount([FromBody] DataTableRequest request)
        {
            var result = await this._accountWorkFlow.GetMyAccountList(request);
            return Ok(result);
        }

        [HttpPost,ValidationFilter]
        public async Task<IActionResult> AddAccount([FromBody] AccountModel model)
        {
            var resılt = await _accountWorkFlow.AddAsync(model);
            return Ok(resılt);
        }

        [HttpPost,ValidationFilter]
        public async Task<IActionResult> AddAccountList([FromBody] AccountModelList model)
        {
            var result = await _accountWorkFlow.AddAsync(model.AccountModels);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount([FromBody] AccountModel model)
        {
            var result = await _accountWorkFlow.DeleteAsync(model);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAccounList([FromBody] List<AccountModel> model)
        {
            var result = await _accountWorkFlow.DeleteAsync(model);
            return Ok(result);
        }

        [HttpPut,ValidationFilter]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountModel model)
        {
            var result = await _accountWorkFlow.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPut,ValidationFilter]
        public async Task<IActionResult> UpdateAccountList([FromBody] AccountModelList model)
        {
            var result = await _accountWorkFlow.UpdateAsync(model.AccountModels);
            return Ok(result);
        }
    }
}
