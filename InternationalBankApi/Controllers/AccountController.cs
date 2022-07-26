using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Dto;
using BusinessLayer.Dto.Account;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Filters;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using EntityLayer.Models;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class AccountController : BaseController
    {
        #region Variables
        private readonly IAccountWorkFlow _accountWorkFlow;
        #endregion
        #region constructor
        public AccountController(IAccountWorkFlow accountWorkFlow)
        {
            this._accountWorkFlow = accountWorkFlow;
        }
        #endregion

        [HttpPost, Route("MyAccount"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyAccount([FromBody] DataTableRequest request)
        {
            var result = await this._accountWorkFlow.GetMyAccountList(request);
            return Ok(result);
        }

        [HttpPost, Route("AddAccount")]
        public async Task<IActionResult> AddAccount([FromBody] AccountCreatDto model)
        {
            var result = await _accountWorkFlow.AddAsync(model);
            return Ok(result);
        }

        [HttpPost, Route("AddAccountList")]
        public async Task<IActionResult> AddAccountList([FromBody] AccountCreatDtoList model)
        {
            var result = await _accountWorkFlow.AddAsync(model);
            return Ok(result);
        }

        [HttpPost, Route("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] AccountUpdateDto model)
        {
            var result = await _accountWorkFlow.DeleteAsync(model);
            return Ok(result);
        }

        [HttpPost, Route("DeleteAccounList")]
        public async Task<IActionResult> DeleteAccounList([FromBody] AccountUpdateDtoList model)
        {
            var result = await _accountWorkFlow.DeleteAsync(model);
            return Ok(result);
        }

        [HttpPost, Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountUpdateDto model)
        {
            var result = await _accountWorkFlow.UpdateAsync(model);
            return Ok(result);
        }

        [HttpPost, Route("UpdateAccountList")]
        public async Task<IActionResult> UpdateAccountList([FromBody] AccountUpdateDtoList model)
        {
            var result = await _accountWorkFlow.UpdateAsync(model);
            return Ok(result);
        }

        [HttpGet, Route("AccountDetail"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> AccountDetail(int accountId)
        {
            var result = await _accountWorkFlow.AccountDetail(accountId);
            return Ok(result);
        }

        [HttpPost, Route("CloseAccount"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> CloseAccount([FromBody] AccountUpdateDto request)
        {
            var result = await _accountWorkFlow.CloseAccount(request);
            return Ok(result);
        }

        [HttpPost, Route("AccountHistory"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> AccountHistory([FromBody] DataTableRequest request)
        {
            var result = await _accountWorkFlow.AccountHistory(request);
            return Ok(result);
        }
    }
}
