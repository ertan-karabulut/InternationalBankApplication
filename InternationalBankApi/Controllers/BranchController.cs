using BusinessLayer.Abstrack;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class BranchController : BaseController
    {
        private readonly IBranchWorkFlow _branchWorkFlow;
        public BranchController(IBranchWorkFlow branchWorkFlow)
        {
            this._branchWorkFlow = branchWorkFlow;
        }

        [HttpGet, Route("GetBranchList")]
        public async Task<IActionResult> GetBranchList()
        {
            var result = await this._branchWorkFlow.BranchList();
            return Ok(result);
        }
    }
}
