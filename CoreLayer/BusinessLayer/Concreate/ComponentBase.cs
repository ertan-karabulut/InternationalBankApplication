using CoreLayer.Utilities.Ioc;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CoreLayer.Utilities.Helpers;
using CoreLayer.Utilities.Messages;
using CoreLayer.Utilities.Aspect;
using Microsoft.AspNetCore.Hosting;
using CoreLayer.Utilities.Enum;
using System.IO;
using Newtonsoft.Json;

namespace CoreLayer.BusinessLayer.Concreate
{
    public class ComponentBase
    {
        protected ComponentBase()
        {
            this.Configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            this.LogMessage = ServiceTool.ServiceProvider.GetService<LogMessage>();
            this.helperWorkFlow = new HelperWorkFlow();
        }

        protected ComponentBase(IHostingEnvironment env) : this()
        {
            this.Env = env;
        }

        #region ProtectedMethod
        protected async Task<string> GetUserPhoto(string fileName)
        {
            string result = string.Empty;
            StringBuilder logText = new StringBuilder();
            LogMessage logMessage = ServiceTool.ServiceProvider.GetService<LogMessage>();
            logText.AppendLine($"İşlem ekli parametrelerle başladı. fileName : {fileName}");
            try
            {
                string folder = FolderEnum.UserPhotos.ToString();
                string filePath = Path.Combine(this.Env.ContentRootPath, "Upload", folder, fileName);
                logText.AppendLine($"Kullanıcı profil resim yolu. {filePath}");
                result = await helperWorkFlow.GetImageBase64(filePath);
                logText.AppendLine($"Dönen değer. result : {result}");
            }
            catch (Exception ex)
            {
                logText.AppendLine($"Hata : {ex.ToString()}");
            }
            logMessage.InsertLog(logText.ToString(), "GetUserPhoto", "WorkFlowBase.cs");
            return result;
        }

        #endregion
        #region ProtectedField
        protected IConfiguration Configuration;
        protected HelperWorkFlow helperWorkFlow;
        protected IHostingEnvironment Env;
        protected LogMessage LogMessage;
        #endregion
    }
}
