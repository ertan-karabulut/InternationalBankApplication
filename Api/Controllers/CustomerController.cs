using Api.Base;
using BusinessLayer.Abstrack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class CustomerController : BaseController
    {
        ICustomerWorkFlow _customerWorkFlow;

        public CustomerController(ICustomerWorkFlow customerWorkFlow)
        {
            this._customerWorkFlow = customerWorkFlow;
        }

        public async Task<IActionResult> GetNameAndImage()
        {
            var result = await this._customerWorkFlow.GetClaim();
            return Ok(result);
        }
    }
}
