using Api.Base;
using BusinessLayer.Abstrack;
using BusinessLayer.Model;
using CoreLayer.Utilities.Filters;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using BusinessLayer.Validation;
using CoreLayer.BusinessLayer.Model;
using CoreLayer.DataAccess.Concrete;
using EntityLayer.Models;

namespace Api.Controllers
{
    public class TokenController : BaseController
    {
        #region constructor
        private ILogonWorkFlow _logonWorkFlow;
        public TokenController(ILogonWorkFlow logonWorkFlow)
        {
            this._logonWorkFlow = logonWorkFlow;
        }
        #endregion

        [AllowAnonymous,HttpPost,ValidationFilter]
        public IActionResult GetToken([FromBody] TokenModel model)
        {
            var result = this._logonWorkFlow.Logon(model);
            return Ok(result);
        }

        [AllowAnonymous,HttpGet]
        public IActionResult test()
        {
            return Ok();
        }
    }
}
