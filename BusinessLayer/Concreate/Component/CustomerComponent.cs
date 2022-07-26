using BusinessLayer.Dto.Adress;
using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.BusinessLayer.Model;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Enum;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.Component
{
    public class CustomerComponent : ComponentBase
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        #endregion
        #region Constructor
        public CustomerComponent(IHostingEnvironment env, IUnitOfWork unow): base(env)
        {
            this._unow = unow;
        }
        #endregion

        protected async Task<IResult<string>> SaveProfilPhoto(FileModel file)
        {
            var result = ResultInjection.Result<string>();
            string path = Path.Combine(this.Env.ContentRootPath, "Upload", FolderEnum.UserPhotos.ToString());
            string fileName = await base.helperWorkFlow.SaveFileAsync(path, file.FileName, Convert.FromBase64String(file.Data));
            if (!string.IsNullOrEmpty(fileName))
            {
                result.ResultObje = fileName;
                result.SetTrue();
            }
            return result;
        }

        protected IResult DeleteProfilePhoto(string fileName)
        {
            var result = ResultInjection.Result();
            string folder = FolderEnum.UserPhotos.ToString();
            string path = Path.Combine(this.Env.ContentRootPath, "Upload", folder, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                result.SetTrue();
            }
            return result;
        }
    }
}
