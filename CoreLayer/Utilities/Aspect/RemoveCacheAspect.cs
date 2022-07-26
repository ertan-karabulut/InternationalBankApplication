using CoreLayer.Utilities.Cache.Abstrack;
using CoreLayer.Utilities.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;

namespace CoreLayer.Utilities.Aspect
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RemoveCacheAspect : MethodInterceptionBase
    {
        private ICacheWorkFlow _cacheWorkFlow;
        private string _pattern;

        public RemoveCacheAspect(string pattern = "")
        {
            this._cacheWorkFlow = ServiceTool.ServiceProvider.GetService<ICacheWorkFlow>();
            this._pattern = pattern;
        }

        public RemoveCacheAspect(Type cacheType, string pattern = "") : this(pattern)
        {
            if (!typeof(ICacheWorkFlow).IsAssignableFrom(cacheType))
                throw new System.Exception("Hatalı cache belirtildi.");
            this._cacheWorkFlow = (ICacheWorkFlow)Activator.CreateInstance(cacheType);
        }

        public override void OnSuccess(IInvocation invocation)
        {
            this._cacheWorkFlow.RemoveByPattern(this._pattern);
            base.OnSuccess(invocation);
        }
    }
}
