using BusinessLayer.Abstrack;
using BusinessLayer.Dto;
using BusinessLayer.Dto.Adress;
using BusinessLayer.Validation;
using CoreLayer.BusinessLayer.Model;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Filters;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerWorkFlow _customerWorkFlow;

        public CustomerController(ICustomerWorkFlow customerWorkFlow, IConfiguration configuration)
        {
            this._customerWorkFlow = customerWorkFlow;
        }

        [HttpGet,Route("GetNameAndImage"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetNameAndImage()
        {
            var result = await this._customerWorkFlow.GetClaim();
            return Ok(result);
        }

        [HttpPost, Route("UpdateProfilePhoto"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateProfilePhoto([FromBody]FileModel file)
        {
            var result = await this._customerWorkFlow.UpdateProfilePhoto(file);
            return Ok(result);
        }
    }
}
