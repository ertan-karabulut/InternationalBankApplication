using Castle.DynamicProxy;
using CoreLayer.Utilities.Cache.Abstrack;
using CoreLayer.Utilities.Ioc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CoreLayer.Utilities.Enum;
using Newtonsoft.Json;
using CoreLayer.Utilities.Messages;

namespace CoreLayer.Utilities.Aspect
{
    //Sadece metodlarda kullanılacak
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAspect : MethodInterceptionBase
    {
        private ICacheWorkFlow _cacheWorkFlow;
        int _duration;
        TimeEnum _timeEnum;

        public override int Priority { get => int.MaxValue - 1; set => base.Priority = int.MaxValue - 1; }
        public CacheAspect(Type cacheWorkFlow, int duration = 30, TimeEnum timeEnum = TimeEnum.Minute)
        {
            if (!typeof(ICacheWorkFlow).IsAssignableFrom(cacheWorkFlow))
                throw new System.Exception("Hatalı cache belirtildi.");
            this._cacheWorkFlow = (ICacheWorkFlow)Activator.CreateInstance(cacheWorkFlow);
            this._duration = duration;
            this._timeEnum = timeEnum;
        }
        public CacheAspect(int duration = 30, TimeEnum timeEnum = TimeEnum.Minute)
        {
            this._cacheWorkFlow = ServiceTool.ServiceProvider.GetService<ICacheWorkFlow>();
            this._duration = duration;
            this._timeEnum = timeEnum;
        }
        public override void Intercept(IInvocation invocation)
        {
            StringBuilder logText = new StringBuilder();
            logText.AppendLine("Cache okuma işlemi başladı");
            string key = this.CreateKey(invocation);
            logText.AppendLine($"Key başarıyla oluşturuldu. Key {key}");
            LogMessage logMessage = ServiceTool.ServiceProvider.GetService<LogMessage>();
            if (this._cacheWorkFlow.IsAdd(key))
            {
                logText.AppendLine("Veriler ön bellekten okundu.");
                invocation.ReturnValue = this._cacheWorkFlow.Get(key);
                logMessage.InsertLog(logText.ToString(), "Intercept", "CacheAspect.cs");
                return;
            }
            logText.AppendLine("Ön bellekte veriler bulunamadı");
            logMessage.InsertLog(logText.ToString(), "Intercept", "CacheAspect.cs");
            base.Intercept(invocation);
        }
        public override void OnSuccess(IInvocation invocation)
        {
            StringBuilder logText = new StringBuilder();
            LogMessage logMessage = ServiceTool.ServiceProvider.GetService<LogMessage>();
            logText.AppendLine("Ön belleğe kaydetme işlemi başladı.");
            string key = this.CreateKey(invocation);
            DateTime cacheDateTime = DateTime.Now.AddMinutes(this._duration * (UInt32)this._timeEnum);
            this._cacheWorkFlow.Add(key, invocation.ReturnValue,cacheDateTime);
            logText.AppendLine($"Veriler ön belleğe kaydedildi. Key : {key} CacheDateTime : {cacheDateTime}");
            logMessage.InsertLog(logText.ToString(), "OnAfter", "CacheAspect.cs");
            base.OnAfter(invocation);
        }
        private string CreateKey(IInvocation invocation)
        {
            string methodFullName = $"{invocation.Method.ReflectedType.Namespace}.{invocation.Method.ReflectedType.Name}.{invocation.Method.Name}";
            List<object> argumentList = invocation.Arguments.ToList();
            return $"{methodFullName}({string.Join(",", argumentList.Select(x => x?.ToString() ?? "<null>"))})";
        }
    }
}
