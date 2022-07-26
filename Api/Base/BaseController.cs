using BusinessLayer.Model;
using CoreLayer.Utilities.Cache.Abstrack;
using CoreLayer.Utilities.Filters;
using CoreLayer.Utilities.Helpers;
using CoreLayer.Utilities.Ioc;
using CoreLayer.Utilities.Messages;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Concreate;
using EntityLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Base
{
    [ExceptionFilter(typeof(ExceptionLogRepository))]
    [Authorize]
    //[LogFilter(typeof(InformationLogRepository))]
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration configuration;
        private readonly HelperWorkFlow helperWorkFlow;

        public BaseController()
        {
            this.configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            this.helperWorkFlow = new HelperWorkFlow();
        }

        protected int GetUserId()
        {
            return this.helperWorkFlow.GetUserId();
        }
    }
}
